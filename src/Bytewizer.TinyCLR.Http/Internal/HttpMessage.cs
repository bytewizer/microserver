using System;
using System.IO;
using System.Collections;

using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Http.Query;

namespace Bytewizer.TinyCLR.Http.Internal
{
    internal class HttpMessage
    {
        public void Decode(HttpContext context)
        {
            ParserMode mode = ParserMode.FirstLine;

            var reader = new StreamReader(context.Channel.InputStream);
            reader.BaseStream.ReadTimeout = 50000;

            string line;
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
                            // Set request body is empty line detected
                            var contentLength = context.Request.ContentLength;
                            if (contentLength > 0)
                            {
                                var buffer = new byte[(int)contentLength];

                                // TODO:  When processing at a fast rate using SSL a read will result in all zeros sometimes.
                                // This in turn writes all zeros to the body creating an empty body of data.
                                // As far as I can tell this is only happing when using SslStream.
                                reader.BaseStream.Read(buffer, 0, buffer.Length);
                                context.Request.Body.Write(buffer, 0, buffer.Length);
                                context.Request.Body.Position = 0;
                            }
                        }
                        else
                        {
                            int seperatorIndex = line.IndexOf(": ");

                            if (seperatorIndex > 1)
                            {
                                var name = line.Substring(0, seperatorIndex);
                                var value = line.Substring(seperatorIndex + 1);

                                context.Request.Headers[name] = value;
                            }
                        }

                        break;
                }

            } while (!reader.EndOfStream);
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
                writer.WriteLine($"{protocol} {statusCode} {reasonPhrase}");

                // Set response date if not found in headers
                if (response.Headers[HeaderNames.Date] == null)
                {
                    response.Headers[HeaderNames.Date] = DateTime.UtcNow.ToString("R");
                }

                // Process headers
                if (response.Headers != null && response.Headers.Count > 0)
                {
                    foreach (HeaderValue header in response.Headers)
                    {
                        writer.WriteLine($"{header.Key}: {header.Value}");
                    }
                }

                writer.Write("\r\n");
                writer.Flush();

                // copy message body to output stream
                if (response.Body.Length > 0)
                {
                    context.Response.Body.Position = 0;
                    context.Channel.Write(context.Response.Body);
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