using System;
using System.IO;
using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Http.Query;
using Bytewizer.TinyCLR.Http.Cookies;
using Bytewizer.TinyCLR.Http.Extensions;

namespace Bytewizer.TinyCLR.Http.Internal
{
    internal class HttpMessage
    {
        public void Decode(HttpContext context)
        {
            ParserMode mode = ParserMode.FirstLine;

            using (var reader = new StreamReader(context.Channel.InputStream))
            {
                string line;
                var body = new StringBuilder();

                do
                {
                    line = reader.ReadLine();
                    switch (mode)
                    {
                        case ParserMode.FirstLine:
                            reader.SkipWhiteSpace();

                            string[] requestLine = line.Split(new char[] { ' ' }, 3);
                            if (requestLine.Length != 3)
                            {
                                throw new Exception("Expected first line to contain three words in accordance HTTP specification.");
                            }

                            if (!requestLine[2].ToLower().StartsWith("http/"))
                            {
                                throw new Exception($"Status line for requests should end with the HTTP version. Your line ended with { requestLine[2] }.");
                            }

                            context.Request.Method = requestLine[0];
                            context.Request.Path = requestLine[1];

                            if (requestLine[1].Contains("?"))
                            {
                                var parameters = requestLine[1].Split('?')[1];

                                var parsed = QueryValue.TryParseList(parameters, out ArrayList result);
                                if (parsed)
                                {
                                    context.Request.Query = new QueryCollection(result);
                                }
 
                                context.Request.Path = requestLine[1].Split('?')[0];
                            }
                            else
                            {
                                context.Request.Path = requestLine[1];
                            }

                            context.Request.Path = requestLine[1].Split('?')[0];

                            if (requestLine.Length >= 3)
                            {
                                context.Request.Protocol = requestLine[2];
                            }
                            else
                            {
                                context.Request.Protocol = HttpProtocol.Http11;
                            }

                            mode = ParserMode.Headers;
                            break;

                        case ParserMode.Headers:

                            if (string.IsNullOrEmpty(line))
                            {
                                mode = ParserMode.Body;
                                continue;
                            }

                            int seperatorIndex = line.IndexOf(": ");

                            if (seperatorIndex > 1)
                            {
                                var name = line.Substring(0, seperatorIndex);
                                var value = line.Substring(seperatorIndex + 1);

                                context.Request.Headers[name] = value;
                            }
                            break;

                        case ParserMode.Body:

                            // TODO: Improve performace by not reading every body line 
                            //Stream req = context.Session.InputStream;
                            //req.Seek(0, System.IO.SeekOrigin.Begin);
                            //string body = new StreamReader(req).ReadToEnd();
                            body.AppendLine(line);

                            break;
                    }
                
                } while (!reader.EndOfStream);

                // Set request body
                var bytes = Encoding.UTF8.GetBytes(body.ToString());
                if (bytes.Length > 0)
                {
                    context.Request.Body = new MemoryStream(bytes);
                    //context.Request.Body.Write(bytes, 0, bytes.Length);
                    context.Request.Body.Position = 0;
                }

                // Set request cookies
                var cookieParser = new HttpCookieParser();
                var headers = context.Request.Headers as HeaderDictionary;
                var cookies = headers[HeaderNames.Cookie];
                if (cookies != null)
                {
                    context.Request.Cookies = cookieParser.Parse(cookies);
                }
            }
        }

        public void Encode(HttpContext context)
        {
            var response = context.Response;

            using (var writer = new StreamWriter(context.Channel.OutputStream))
            {
                // Set protocol line
                var protocol = context.Request.Protocol ?? HttpProtocol.Http11;

                // Set status code
                var statusCode = response.StatusCode > 0
                    ? response.StatusCode : StatusCodes.Status404NotFound;

                // Set reason phrase from status code
                var reasonPhrase = HttpReasonPhrase.Get(statusCode);

                // Write first line
                writer.Write($"{protocol} {statusCode} {reasonPhrase}\r\n");

                // Set response date if not found in headers
                if (response.Headers[HeaderNames.Date] == null)
                {
                    response.Headers[HeaderNames.Date] = DateTime.UtcNow.ToString("R");
                }

                // Set response content length if not found in headers
                if (response.Headers[HeaderNames.ContentLength] == null)
                {
                    response.Headers[HeaderNames.ContentLength] = "0";
                }

                // Process headers
                if (response.Headers != null && response.Headers.Count > 0)
                {
                    foreach (HeaderValue header in response.Headers)
                    {
                        writer.Write($"{header.Key}: {header.Value}\r\n");
                    }
                }

                if (response.Cookies is CookieCollection cookies && cookies.Count > 0)
                {
                    foreach (CookieValue cookie in cookies)
                    {
                        writer.Write($"Set-Cookie: {cookie.Value}\r\n");
                    }
                }

                writer.Write("\r\n");
                writer.Flush();

                // copy message body to output stream
                if (response.Body.Length > 0)
                {
                    context.Response.Body.Position = 0;
                    context.Response.Body.CopyTo(context.Channel.OutputStream);
                    context.Response.Body.Dispose();
                }
            }
        }

        internal enum ParserMode
        {
            FirstLine,
            Headers,
            Body
        }
    }
}