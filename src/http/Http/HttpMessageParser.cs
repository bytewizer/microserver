using System;
using System.IO;
using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.Http
{
    public static class HttpMessageParser
    {
        public static void Decode(HttpContext context)
        {
            ParserMode mode = ParserMode.FirstLine;

            var reader = new StreamReader(context.Session.InputStream);

            while (reader.Peek() != -1)
            {
                switch (mode)
                {
                    case ParserMode.FirstLine:
                        reader.SkipWhiteSpace();

                        string[] requestLine = reader.ReadLine().Split(new char[] { ' ' }, 3);
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
                        var line = reader.ReadLine();

                        if (string.IsNullOrEmpty(line))
                        {
                            mode = ParserMode.Body;
                            continue;
                        }

                        int split = line.IndexOf(": ");

                        if (split > 1)
                        {
                            var name = line.Substring(0, split);
                            var value = line.Substring(split + 1);

                            context.Request.Headers.Add(name, value);
                        }

                        //if ((line != null) || (line.Length <= 0))
                        //{
                        //    int delimiterIndex = line.IndexOf(": ");
                        //    if (delimiterIndex != -1)
                        //    {
                        //        var offset = 2;
                        //        var name = line.Substring(0, delimiterIndex);
                        //        var value = line.Substring(delimiterIndex + offset, line.Length - delimiterIndex - offset);
                        //        context.Request.Headers.Add(name, value);
                        //    }
                        //}

                        break;

                    case ParserMode.Body:
                        var body = reader.ReadToEnd();

                        // Normalize line endings to CRLF, which is required for headers, etc.
                        //body = body.Replace("\r\n", "\n").Replace("\n", "\r\n");
                        //body = string.Concat(line, "\r\n", body);

                        var bytes = Encoding.UTF8.GetBytes(body);
                        if (bytes.Length > 0)
                        {
                            context.Request.Body = new MemoryStream();
                            context.Request.Body.Write(bytes, 0, bytes.Length);
                        }

                        break;
                }
            }
        }

        public static void Encode(HttpContext context)
        {
            context.Session.OutputStream = new MemoryStream();
            var outputWriter = new StreamWriter(context.Session.OutputStream);

            var response = context.Response;

            // set status line
            var protocol = context.Request.Protocol;
            var statusCode = context.Response.StatusCode;
            var reasonPhrase = HttpReasonPhrase.Get(statusCode);
            
            outputWriter.Write($"{protocol} {statusCode} {reasonPhrase}\r\n");

            // set zero content length
            if (response.Body != null && response.Body.Length > -1)
            {
                response.Headers.ContentLength = response.Body.Length;
            }

            // process headers
            if (response.Headers != null && response.Headers.Count > 0)
            {
                foreach (HeaderValue header in response.Headers)
                {
                    outputWriter.Write($"{header.Key}: {header.Value}\r\n");  // note the space after :
                }
            }

            // TODO: process cookies

            outputWriter.Write("\r\n");
            outputWriter.Flush();

            // copy message body to output stream
            if (response.Body != null && response.Body.Length > -1)
            {
                CopyBody(context, 1024);
            }
        }

        internal static void CopyBody(HttpContext context, int bufferSize)
        {
            var buffer = new byte[bufferSize];

            context.Response.Body.Position = 0;

            int count;
            while ((count = context.Response.Body.Read(buffer, 0, buffer.Length)) != 0)
            {
                context.Session.OutputStream.Write(buffer, 0, count);
            }
        }

        private enum ParserMode
        {
            FirstLine,
            Headers,
            Body
        }
    }

}
