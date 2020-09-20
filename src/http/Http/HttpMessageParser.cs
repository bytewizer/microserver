using System;
using System.IO;
using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Http.Extensions;

namespace Bytewizer.TinyCLR.Http
{
    public static class HttpMessageParser
    {
        public static void Decode(HttpContext context)
        {
            ParserMode mode = ParserMode.FirstLine;

            var body = new StringBuilder();
            var reader = new StreamReader(context.Session.InputStream);
           
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                switch (mode)
                {
                    case ParserMode.FirstLine:
                        reader.SkipWhiteSpace();

                        string[] requestLine = line.Split(new char[] { ' ' }, 3);
                        if (requestLine.Length != 3)
                            throw new Exception("Expected first line to contain three words in accordance HTTP specification.");

                        if (!requestLine[2].ToLower().StartsWith("http/"))
                            throw new Exception($"Status line for requests should end with the HTTP version. Your line ended with { requestLine[2] }.");

                        context.Request.Method = requestLine[0];
                        context.Request.PathBase = requestLine[1];

                        if (requestLine[1].Contains("?"))
                        {
                            var parameters = requestLine[1].Split('?')[1];

                            var parsed = QueryValue.TryParseList(parameters, out ArrayList result);
                            if (parsed)
                            {
                                context.Request.Query = new QueryCollection(result);
                            }
                            else
                            {
                                context.Request.Query = QueryCollection.Empty;
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

                            context.Request.Headers.Add(name, value);
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
            }

            var bytes = Encoding.UTF8.GetBytes(body.ToString());
            if (bytes.Length > 0)
            {
                context.Request.Body = new MemoryStream(bytes);
                context.Request.Body.Position = 0;
            }

            var cookieParser = new HttpCookieParser();
            var cookies =  context.Request.Headers.Cookie;
            if (cookies != null)
            {
                context.Request.Cookies = cookieParser.Parse(cookies);
            }
        }

        public static void Encode(HttpContext context)
        {
            if (context.Session.InputStream == null)
            {
                return;
            }
            
            var outputWriter = new StreamWriter(context.Session.InputStream);

            var response = context.Response;

            // set status line
            var protocol = context.Request.Protocol;
            var statusCode = context.Response.StatusCode;
            var reasonPhrase = HttpReasonPhrase.Get(statusCode);

            outputWriter.Write($"{protocol} {statusCode} {reasonPhrase}\r\n");

            // process headers
            if (response.Headers != null && response.Headers.Count > 0)
            {
                foreach (HeaderValue header in response.Headers)
                {
                    outputWriter.Write($"{header.Key}: {header.Value}\r\n");
                }
            }

            if (response.Cookies != null && response.Cookies.Count > 0)
            {
                //Set-Cookie: <name>=<value>[; <name>=<value>][; expires=<date>][; domain=<domain_name>][; path=<some_path>][; secure][; httponly]
                foreach (CookieValue item in response.Cookies as CookieCollection)
                {
                    var cookie = item.Value;
                    outputWriter.Write($"Set-Cookie: {cookie}\r\n");
                }
            }

            outputWriter.Write("\r\n");
            outputWriter.Flush();

            // copy message body to input stream
            if (response.Body != null && response.Body.Length > -1)
            {
                context.Response.Body.Position = 0;
                context.Response.Body.CopyTo(context.Session.InputStream);
                context.Response.Body.Dispose();
            }
        }
    }
}
