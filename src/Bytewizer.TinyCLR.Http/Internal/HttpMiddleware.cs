using System;
using System.Net.Sockets;
using System.Threading;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http.Internal
{
    internal class HttpMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly HttpMessage _httpMessage;
        private readonly HttpServerOptions _httpOptions;

        public HttpMiddleware(ILogger logger, HttpServerOptions options)
        {
            _logger = logger;
            _httpOptions = options;
            _httpMessage = new HttpMessage();
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            // Wait for request bytes
            if (WaitForData(context.Channel.Client, 10000))
            {
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
            }

            // Check message size
            if (context.Channel.InputStream.Length < _httpOptions.Limits.MinMessageSize
            || context.Channel.InputStream.Length > _httpOptions.Limits.MaxMessageSize)
            {
                _logger.InvalidMessageLimit(
                    context.Channel.InputStream.Length,
                    _httpOptions.Limits.MinMessageSize,
                    _httpOptions.Limits.MaxMessageSize
                    );

                return;
            }

            _httpMessage.Decode(context);

            next(context);

           _httpMessage.Encode(context);
        }

        protected bool WaitForData(Socket socket, int timeout)
        {
            var startTicks = DateTime.UtcNow.Ticks;

            do
            {
                if ((DateTime.UtcNow.Ticks - startTicks) / TimeSpan.TicksPerMillisecond > timeout)
                {
                    return true;
                }

                Thread.Sleep(10);

            } while (socket.Available == 0);

            return false;
        }
    }
}