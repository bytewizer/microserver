using System;
using System.Text;

using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Http.WebSockets;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http.Features
{
    ///<inheritdoc/>
    public class HttpWebSocketFeature : IHttpWebSocketFeature
    {
        private readonly HttpContext _context;
        private readonly WebSocketOptions _options;

        public HttpWebSocketFeature(HttpContext context, WebSocketOptions options)
        {
            _context = context;
            _options = options;
        }

        ///<inheritdoc/>
        public bool IsWebSocketRequest
        {
            get
            {
                return CheckSupportedWebSocketRequest(_context.Request);
            }
        }

        public WebSocket Accept(string[] subProtocols)
        {
            if (!IsWebSocketRequest)
            {
                return null;
            }

            string key = _context.Request.Headers[HeaderNames.SecWebSocketKey];

            GenerateResponseHeaders(_context, key, subProtocols);

            return new WebSocket();
        }

        private static bool CheckSupportedWebSocketRequest(HttpRequest request)
        {
            bool validUpgrade = false;
            bool validConnection = false;
            bool validKey = false;
            bool validVersion = false;

            if (!string.Equals(request.Method, HttpMethods.Get))
            {
                return false;
            }

            foreach (HeaderValue pair in request.Headers)
            {
                if (string.Equals(HeaderNames.Connection, pair.Key))
                {
                    if (string.Equals("Upgrade", pair.Value))
                    {
                        validConnection = true;
                    }
                }
                else if (string.Equals(HeaderNames.Upgrade, pair.Key))
                {
                    if (string.Equals("websocket", pair.Value))
                    {
                        validUpgrade = true;
                    }
                }
                else if (string.Equals(HeaderNames.SecWebSocketVersion, pair.Key))
                {
                    if (string.Equals("13", pair.Value))
                    {
                        validVersion = true;
                    }
                }
                else if (string.Equals(HeaderNames.SecWebSocketKey, pair.Key))
                {
                    validKey = IsRequestKeyValid(pair.Value);
                }
            }

            return validConnection && validUpgrade && validVersion && validKey;
        }

        public static bool IsRequestKeyValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            try
            {
                byte[] data = Convert.FromBase64String(value);
                return data.Length == 16;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void GenerateResponseHeaders(HttpContext context, string key, string[] subProtocols)
        {
            var header = new StringBuilder();
            header.Append("HTTP/1.1 101 Switching Protocols\r\n");
            header.Append("Upgrade: websocket\r\n");
            header.Append("Connection: Upgrade\r\n");
            header.Append($"Sec-WebSocket-Accept: {CreateResponseKey(key)}\r\n");
            //if (subProtocols != null)
            //{
            //    sb.Append($"Sec-WebSocket-Protocol: {targetProtocol}\r\n");
            //}
            header.Append("\r\n");

            context.Channel.Write(header.ToString());
        }

        public static string CreateResponseKey(string requestKey)
        {
            if (requestKey == null)
            {
                throw new ArgumentNullException(nameof(requestKey));
            }

            var algorithm = SHA1.Create();

            string merged = requestKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            byte[] mergedBytes = Encoding.UTF8.GetBytes(merged);
            byte[] hashedBytes = algorithm.ComputeHash(mergedBytes);

            if (hashedBytes.Length != 20)
            {
                throw new InvalidOperationException("Could not compute the hash for the 'Sec-WebSocket-Accept' header.");
            }

            return Convert.ToBase64String(hashedBytes);
        }

    }
}