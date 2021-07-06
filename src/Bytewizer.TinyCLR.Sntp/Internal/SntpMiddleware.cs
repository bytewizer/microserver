using System;
using System.Net;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Sntp.Internal
{
    internal class SntpMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly SntpServerOptions _sntpOptions;

        public SntpMiddleware(ILoggerFactory loggerFactory, SntpServerOptions options)
        {
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sntp");
            _sntpOptions = options;
        }

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

            _logger.Log( // Make packet logger
                LogLevel.Information,
                new EventId(100, "Unhandled Exception"),
                NtpPacket.Print(request),
                null
                );

            if (request.Mode == NtpMode.Client)
            {
                if (string.IsNullOrEmpty(_sntpOptions.Server))
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
                        var ip = new IPAddress(new byte[4] { 0, 0, 0, 0 });
                        response.ReferenceIdAddress = IPAddressHelper.FromIPAddress(ip);
                    }

                    // Time at the server when the response left for the client
                    response.TransmitTimestamp = _sntpOptions.TimeSource;

                    ctx.Channel.SendTo(
                            response.Bytes,
                            0,
                            response.Bytes.Length,
                            SocketFlags.None,
                            remoteEndpoint
                        );

                    next(ctx);
                }
                else
                {
                    NtpPacket packet;

                    if (IPAddressHelper.TryParse(_sntpOptions.Server, out IPAddress ip))
                    {
                        var endpoint = new IPEndPoint(ip, 123);
                        using (var ntp = new NtpClient(endpoint))
                        {
                            ntp.Timeout = TimeSpan.FromSeconds(5);
                            packet = ntp.Query(request);
                        }
                    }
                    else
                    {
                        using (var ntp = new NtpClient(_sntpOptions.Server, 123))
                        {
                            ntp.Timeout = TimeSpan.FromSeconds(5);
                            packet = ntp.Query(request);
                        }
                    }

                    ctx.Channel.SendTo(
                            packet.Bytes,
                            0,
                            packet.Bytes.Length,
                            SocketFlags.None,
                            remoteEndpoint
                        );

                    next(ctx);
                }
            }
        }
    }
}