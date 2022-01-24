using System.Net.Sockets;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Sntp.Internal
{
    /// <summary>
    /// A middleware for creating SNTP packet used for communication to and from a network time server.
    /// </summary>
    public class SntpMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly SntpServerOptions _sntpOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpMiddleware"/> class.
        /// </summary>
        public SntpMiddleware(ILoggerFactory loggerFactory, SntpServerOptions options)
        {
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sntp");
            _sntpOptions = options;
        }

        /// <inheritdoc/>
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            //Time at the server when the request arrived from the client
            var receiveTimestamp = _sntpOptions.TimeSource;

            var ctx = context as ISocketContext;

            var remoteEndpoint = ctx.Channel.Connection.RemoteEndpoint;

            var length = (int)ctx.Channel.InputStream.Length;
            var buffer = new byte[length];
            ctx.Channel.InputStream.Read(buffer, 0, length);

            var request = new NtpPacket(buffer);

            _logger.SocketTransport(NtpPacket.Print(request));

            if (request.Mode == NtpMode.Client)
            {
                var response = new NtpPacket
                {
                    Mode = NtpMode.Server,
                    //LeapIndicator = LeapIndicator.NoWarning,  TODO: Not working
                    Stratum = _sntpOptions.Stratum,
                    VersionNumber = _sntpOptions.VersionNumber,
                    Poll = _sntpOptions.PollInterval,
                    Precision = _sntpOptions.Precision,

                    // Time at the client when the request departed for the server
                    OriginTimestamp = request.TransmitTimestamp,

                    // Time when the system clock was last set or corrected
                    ReferenceTimestamp = _sntpOptions.ReferenceTimestamp,

                    // Time at the server when the request arrived from the client
                    ReceiveTimestamp = receiveTimestamp,

                    // Time at the client when the reply arrived from the server
                    DestinationTimestamp = _sntpOptions.TimeSource
                };

                if (response.Stratum == Stratum.Primary)
                {
                    response.ReferenceId = _sntpOptions.ReferenceId;
                }
                else
                {
                    response.ReferenceIPAddress = IPAddressHelper.FromIPAddress(_sntpOptions.ReferenceIPAddress);
                }

                // Time at the server when the response left for the client (in local time)
                response.TransmitTimestamp = _sntpOptions.TimeSource.ToLocalTime();

                ctx.Channel.SendTo(
                        response.Bytes,
                        0,
                        response.Bytes.Length,
                        SocketFlags.None,
                        remoteEndpoint
                    );

                next(ctx);
            }
        }
    }
}