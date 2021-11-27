using Bytewizer.TinyCLR.Ftp.Features;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal class FtpMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly FileProvider _fileProvider;
        private readonly FtpServerOptions _ftpOptions;

        public FtpMiddleware(ILogger logger, FtpServerOptions options)
        {
            _logger = logger;
            _ftpOptions = options;
            _fileProvider = new FileProvider(_ftpOptions.RootPath);
        }

        protected override void Invoke(FtpContext context, RequestDelegate next)
        {
            // Check message size
            if (context.Channel.InputStream.Length < _ftpOptions.Limits.MinMessageSize
                || context.Channel.InputStream.Length > _ftpOptions.Limits.MaxMessageSize)
            {
                _logger.InvalidMessageLimit(
                    context.Channel.InputStream.Length,
                    _ftpOptions.Limits.MinMessageSize,
                    _ftpOptions.Limits.MaxMessageSize
                    );

                return;
            }

            var feature = new SessionFeature();
            context.Features.Set(typeof(ISessionFeature), feature);

            next(context);

            _ = new FtpSession(_logger, context, _fileProvider, _ftpOptions);
        }
    }
}