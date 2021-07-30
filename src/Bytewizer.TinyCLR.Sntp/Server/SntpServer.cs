using System;
using System.Net;
using System.Threading;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Represents an implementation of the <see cref="SntpServer"/> for creating web servers.
    /// </summary>
    public class SntpServer : IServer
    {
        private Timer _pollingTimer;
        private readonly int _pollingDelay = 1000;

        private readonly SocketServer _sntpServer;
        private readonly ILogger _logger = NullLogger.Instance;
        private readonly ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;
        private readonly SntpServerOptions _sntpOptions = new SntpServerOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        public SntpServer()
        {
            Initialize();
            _sntpOptions.Server = "pool.ntp.org";
            _sntpServer = new SocketServer(NullLoggerFactory.Instance, _sntpOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="SntpServer"/> specific features.</param>
        public SntpServer(ServerOptionsDelegate configure)
        {
            Initialize();

            configure(_sntpOptions);

            _sntpServer = new SocketServer(NullLoggerFactory.Instance, _sntpOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="SntpServer"/> specific features.</param>
        public SntpServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sntp");

            Initialize();

            configure(_sntpOptions);

            _sntpServer = new SocketServer(_loggerFactory, _sntpOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="SntpServer"/> specific features.</param>
        public SntpServer(ILoggerFactory loggerFactory, ServerOptions options)
        {
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sntp");
            _sntpServer = new SocketServer(loggerFactory, options);
        }

        private void Initialize()
        {
            // RFC limits message size to a 48 byte minimum and 68 byte maximum
            _sntpOptions.Limits.MinMessageSize = 48;
            _sntpOptions.Limits.MaxMessageSize = 68;

            // Set listening port 
            _sntpOptions.Listen(123, listener =>
            {
                listener.UseUdp();
                listener.Broadcast = true;
            });

            // Set pipleline options
            _sntpOptions.Pipeline(app =>
            {
                app.UseSntp(_loggerFactory, _sntpOptions);
            });
        }

        /// <inheritdoc/>
        public bool Start()
        {
            bool state = false;

            if (!string.IsNullOrEmpty(_sntpOptions.Server))
            {
                int retry = 0;

                while (true)
                {
                    try
                    {
                        var hostEntry = Dns.GetHostEntry(_sntpOptions.Server);

                        if ((hostEntry != null) && (hostEntry.AddressList.Length > 0))
                        {
                            var i = 0;
                            while (hostEntry.AddressList[i] == null) i++;
                            {
                                var address = hostEntry.AddressList[i];
                                state = StartPolling(address);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (retry >= 3)
                        {
                            _logger.TimesyncException(ex);
                            break;
                        }

                        _logger.InvalidNameResolve(_sntpOptions.Server);

                        retry++;
                        continue;
                    }
                }
            }
            else
            {
                state = true;
            }
            
            if (state == true && _sntpOptions.Listening)
            {
                state = _sntpServer.Start();
            }

            return state;
        }

        /// <inheritdoc/>
        public bool Stop()
        {
            if (_pollingTimer != null)
            {
                _pollingTimer.Dispose();
            }

            return _sntpServer.Stop();
        }

        private bool StartPolling(IPAddress address)
        {
            _sntpOptions.ReferenceIPAddress = address;

            if (_sntpOptions.Stratum == Stratum.Unspecified)
            {
                _sntpOptions.Stratum = Stratum.Secondary;
            }

            try
            {
                _pollingTimer = new Timer(new TimerCallback(Synchronize), address, _pollingDelay, (int)_sntpOptions.SyncInterval.TotalMilliseconds);

            }
            catch (Exception ex)
            {
                _logger.ServiceExecption("Error starting time synchronization polling timer.", ex);
                return false;
            }

            return true;
        }

        private void Synchronize(object state)
        {
            int retry = 0;

            IPEndPoint endpoint = new IPEndPoint((IPAddress)state, 123);

            while (true)
            {
                try
                {
                    using (var ntp = new SntpClient(endpoint, _sntpOptions.SyncTimeout))
                    {
                        var accurateTime = DateTime.UtcNow + ntp.GetCorrectionOffset();

                        _sntpOptions.TimeSource = accurateTime;
                        _logger.TimesyncMessage(accurateTime, _sntpOptions.Server);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    if (retry >= 3)
                    {
                        _logger.TimesyncException(ex);
                        break;
                    }

                    retry++;
                    continue;
                }
            }
        }
    }
}