using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Ftp.Internal;
using Bytewizer.TinyCLR.Sockets.Channel;
using System.Diagnostics;


namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents an implementation of the <see cref="FtpServer"/> for creating web servers.
    /// </summary>
    public class FtpServer : SocketService, IServer
    {
        private readonly ILogger _logger;
        private readonly ContextPool _contextPool;
        private readonly FtpServerOptions _ftpOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, new FtpServerOptions())
        {
            _options.Pipeline(app =>
            {
                app.Use(new FtpMiddleware(_logger, _ftpOptions));
            });

            configure(_options as FtpServerOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : this(loggerFactory, new FtpServerOptions())
        {
            _options.Pipeline(app =>
            {
                app.Use(new FtpMiddleware(_logger, _ftpOptions));
            });

            configure(_options as FtpServerOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ILoggerFactory loggerFactory, ServerOptions options)
            : base(loggerFactory)
        {
            _options = options;
            _contextPool = new ContextPool();
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");
            _ftpOptions = _options as FtpServerOptions;

            _options.Listen(21);
        }

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The socket channel for the connected end point.</param>
        protected override void ClientConnected(object sender, SocketChannel channel)
        {
            // Set channel error handler
            channel.ChannelError += ChannelError;

            try
            {
                // get context from context pool
                var context = _contextPool.GetContext(typeof(FtpContext)) as FtpContext;

                // assign channel
                context.Channel = channel;

                // Write ready message to channel output stream
                context.Channel.Write(220, _ftpOptions.BannerMessage);

                byte[] buffer = new byte[256];

                while (true)
                {
                    //if (!context.Channel.Active)
                    //{
                    //    break;
                    //}

                    // Update command
                    var bytes = context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                    if (bytes > 0)
                    {
                        var commands = Encoding.UTF8.GetString(buffer, 0, bytes);

                        try
                        {
                            context.Request.Command = FtpCommand.Parse(commands);

                            // Invoke pipeline
                            _options.Application.Invoke(context);
                        }
                        catch (Exception ex)
                        {
                            var message = ex.Message.Replace("\r", " ").Replace("\n", "");
                            context.Channel.Write(451, $"Exception thrown, message: {message}.");

                            break;
                        }
                    }

                    Thread.Sleep(100);
                }

                // release context back to pool and close connection once pipeline is complete
                _contextPool.Release(context);
            }
            catch (Exception ex)
            {
                _logger.UnhandledException(ex);
                return;
            }
        }

        /// <summary>
        /// An internal channel error occured.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The <see cref="Exception"/> for the channel error.</param>
        private void ChannelError(object sender, Exception execption)
        {
            _logger.ChannelExecption(execption);
        }
    }
}





// check to make sure channel contains data
//if (context.Channel.InputStream.Length > 0)
//{
// invoke pipeline 
//_options.Application.Invoke(context);
//}

//byte[] buffer = new byte[1024];
//while (true)
//{
//    do
//    {
//        inputStream.ReadSafely(buffer, 0, buffer.Length);
//        var command = FtpCommand.Parse(Encoding.UTF8.GetString(buffer));
//    } while (inputStream.DataAvailable);
//}