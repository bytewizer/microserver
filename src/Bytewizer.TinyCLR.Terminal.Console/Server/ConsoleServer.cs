using System;
using System.Diagnostics;
using System.Threading;

using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;

using GHIElectronics.TinyCLR.Devices.UsbClient;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents an implementation of the <see cref="ConsoleServer"/> for creating terminal servers.
    /// </summary>
    public class ConsoleServer : IServerService
    {
        private readonly ILogger _logger;
        private readonly ConsoleServerOptions _options;

        private Cdc _listenClient;
        private Thread _thread;
        private UsbClientController _controller;

        private readonly object _lock = new object();

        internal readonly ManualResetEvent _startedEvent = new ManualResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="ConsoleServer"/> specific features.</param>
        public ConsoleServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, new ConsoleServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="ConsoleServer"/> specific features.</param>
        public ConsoleServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : this(loggerFactory, new ConsoleServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="ConsoleServer"/> specific features.</param>
        public ConsoleServer(ILoggerFactory loggerFactory, ConsoleServerOptions options)
        {
            _options = options;
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Terminal.Console");
        }

        private void Initialize(ServerOptionsDelegate configure)
        {
            try
            {
                _controller = UsbClientController.GetDefault();
            }
            catch (Exception ex)
            {
                _logger.UnhandledException(ex);
                throw;
            }

            _options.Pipeline(app =>
            {
                app.Use(new ConsoleMiddleware(_logger, _options));
            });

            configure(_options);
        }

        /// <summary>
        /// Gets a value that indicates whether usb controller is actively listening for client connections.
        /// </summary>
        protected bool Active { get; private set; } = false;

        ///<inheritdoc/>
        public bool Start()
        {
            if (Active)
            {
                return true;
            }

            // Don't return until thread that calls Accept is ready to listen
            _startedEvent.Reset();

            lock (_lock)
            {
                try
                {
                    var clientSetting = new UsbClientSetting
                    {
                        Mode = UsbClientMode.Cdc,
                        MaxPower = RawDevice.MAX_POWER,
                        ProductName = _options.ProductName,
                        SerialNumber = _options.SerialNumber,
                        InterfaceName = _options.InterfaceName
                    };

                    _listenClient = new Cdc(_controller, clientSetting);
                    _listenClient.DeviceStateChanged += DeviceStateChanged;
                    _listenClient.DataReceived += DataReceived;

                    _thread = new Thread(() =>
                    {
                        if (_listenClient != null)
                        {
                            Active = true;
                            AcceptConnection();
                        }
                    });
                    _thread.Start();

                    _listenClient.Enable();

                    // Waits for thread that calls Accept to start
                    _startedEvent.WaitOne();

                    //_logger.StartingService(_listener.ActivePort);

                    return true;
                }
                catch (Exception ex)
                {
                    Active = false;
                    _logger.UnhandledException( ex);
                    return false;
                }
            }
        }

        ///<inheritdoc/>
        public bool Stop()
        {
            if (!Active)
            {
                return true;
            }

            try
            {
                Active = false;

                _listenClient.Disable();
                _controller.Dispose();
                //_logger.StoppingService(_listener.ActivePort);

                return true;
            }
            catch (Exception ex)
            {
                //_logger.StoppingService(_listener.ActivePort, ex);
                return false;
            }
        }

        /// <summary>
        /// Accepted connection listening thread
        /// </summary>
        internal void AcceptConnection()
        {
            // Signal the start method to continue
            _startedEvent.Set();

            //while (Active)
            //{
                //try
                //{
                    while (_listenClient.DeviceState != DeviceState.Configured)
                    {
                        // 1 = Attached
                        // 2 = Powered
                        // 3 = Default
                        // 4 = Address
                        // 5 = Configured
                        // 6 = Suspended

                        Debug.WriteLine($"Loop State:{_listenClient.DeviceState}");
                        
                        Thread.Sleep(100);
                    }

                    // create console context
                    var context = new ConsoleContext();

                    // assign channel
                    context.Channel.Assign(_listenClient);

                    // Invoke pipeline
                    _options.Application.Invoke(context);

                //}
                //catch (Exception ex)
                //{
                //    _logger.UnhandledException(ex);
                //}

                Thread.Sleep(1);
            //}
        }

        /// <summary>
        /// A client's state has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="state">The device state for the connected end point.</param>
        private void DeviceStateChanged(RawDevice sender, DeviceState state)
        {
            Debug.WriteLine($"Connection changed:{state}");
        }

        /// <summary>
        /// A client has provided data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="count"></param>
        private void DataReceived(RawDevice sender, uint count)
        {
            Debug.WriteLine("Data received:" + count);
        }

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The socket channel for the connected end point.</param>
        protected  void ClientConnected(object sender, Cdc channel)
        {
        }

        /// <summary>
        /// A client has disconnected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The socket <see cref="Exception"/> for the disconnected endpoint.</param>
        protected void ClientDisconnected(object sender, Exception execption)
        {
            //_logger.RemoteDisconnect(execption);
        }
    }
}





///// <summary>
///// A client has connected.
///// </summary>
///// <param name="sender">The source of the event.</param>
///// <param name="channel">The socket channel for the connected end point.</param>
//protected override void ClientConnected(object sender, SocketChannel channel)
//{
//    //_logger.RemoteConnected(channel);

//    // Set channel error handler
//    channel.ChannelError += ChannelError;

//    try
//    {
//        // get context from context pool
//        var context = _contextPool.GetContext(typeof(TelnetContext)) as TelnetContext;

//        // assign channel
//        context.Channel = channel;
//        //context.Channel.Assign(
//        //    channel.Client,
//        //    new TerminalStream(channel.InputStream),
//        //    new TerminalStream(channel.OutputStream)
//        //);

//        try
//        {
//            // Invoke pipeline
//            _options.Application.Invoke(context);
//        }
//        catch (Exception ex)
//        {
//            _logger.UnhandledException(ex);
//        }

//        _logger.RemoteClosed(channel);

//        // release context back to pool and close connection once pipeline is complete
//        _contextPool.Release(context);
//    }
//    catch (Exception ex)
//    {
//        _logger.UnhandledException(ex);
//        return;
//    }
//}

///// <summary>
///// A client has disconnected.
///// </summary>
///// <param name="sender">The source of the event.</param>
///// <param name="execption">The socket <see cref="Exception"/> for the disconnected endpoint.</param>
//protected override void ClientDisconnected(object sender, Exception execption)
//{
//    _logger.RemoteDisconnect(execption);
//}