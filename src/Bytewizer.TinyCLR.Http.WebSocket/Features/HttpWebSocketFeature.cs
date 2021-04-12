using System;
using System.IO;
using System.Text;

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

            if (request.Method != HttpMethods.Get)
            {
                return false;
            }

            var connection = request.Headers[HeaderNames.Connection];
            var upgrade = request.Headers[HeaderNames.Upgrade];
            var version = request.Headers[HeaderNames.SecWebSocketVersion];
            var key = request.Headers[HeaderNames.SecWebSocketKey];

            if (connection == "Upgrade")
            {
                if (upgrade == "websocket")
                {
                    validConnection = true;
                }
            }

            if (upgrade == "websocket") // TODO: Remove?           
            {
                validUpgrade = true;
            }

            if (version == "13")
            {
                validVersion = true;
            }

            if (!string.IsNullOrEmpty(key))
            {
                if (Convert.FromBase64String(key).Length == 16)
                {
                    validKey = true;
                }
            }

            return validConnection && validUpgrade && validVersion && validKey;
        }

        public static void GenerateResponseHeaders(HttpContext context, string key, string[] subProtocols)
        {
            if (subProtocols != null)
            {
                //TODO
            }
            
            var header = "HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\n" +
                $"Upgrade: websocket\r\nSec-WebSocket-Accept: {CreateResponseKey(key)}\r\n\r\n";
            
            context.Channel.Write(header);
     

            //response.Headers[HeaderNames.Connection] = HeaderNames.Upgrade;
            //response.Headers[HeaderNames.Upgrade] = "websocket";
            //response.Headers[HeaderNames.SecWebSocketAccept] = CreateResponseKey(key);
            //if (!string.IsNullOrEmpty(subProtocol))
            //{
            //    response.Headers[HeaderNames.SecWebSocketProtocol] = subProtocol;
            //}

            //response.StatusCode = StatusCodes.Status101SwitchingProtocols;
        }

        public static string CreateResponseKey(string requestKey)
        {
            // "The value of this header field is constructed by concatenating /key/, defined above in step 4
            // in Section 4.2.2, with the string "258EAFA5-E914-47DA-95CA-C5AB0DC85B11", taking the SHA-1 hash of
            // this concatenated value to obtain a 20-byte value and base64-encoding"
            // https://tools.ietf.org/html/rfc6455#section-4.2.2

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