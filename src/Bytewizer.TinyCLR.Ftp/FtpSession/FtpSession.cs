using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Listener;
using Bytewizer.TinyCLR.Ftp.Features;
using System.Text;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private IPEndPoint _endpoint;

        private TcpListener _listener;
        private NetworkStream _listenerStream;
      
        private readonly ILogger _logger;
        private readonly FtpContext _context;
        private readonly FileProvider _fileProvider;
        private readonly FtpServerOptions _ftpOptions;

        public FtpSession(
                ILogger logger,
                FtpContext context,
                FileProvider fileProvider,
                FtpServerOptions ftpOptions)
        {
            _logger = logger;
            _context = context;
            _fileProvider = fileProvider;
            _ftpOptions = ftpOptions;

            // Setup passive data channel listener
            SetupListener();

            // Set root and user path option
            _fileProvider.SetWorkingDirectory(_ftpOptions.RootPath);

            // Set list format option
            _context.Request.ListFormat = _ftpOptions.ListFormat;

            // Write ready message to channel output stream
            _context.Channel.Write(220, _ftpOptions.BannerMessage);

            // Process command loop
            byte[] buffer = new byte[1024];
            while (_context.Channel.Connected)
            {
                var bytes = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (bytes > 0)
                {
                    _context.Request.Command = FtpCommand.Parse(buffer, 0, bytes);

                    // log request command
                    _logger.CommandRequest(_context);

                    // process the command received
                    CommandReceived();

                    // log response command
                    _logger.CommandResponse(_context);

                    // Write the response to the client
                    _context.Channel.Write(
                        _context.Response.Message
                        );

                    // Clear request commands
                    _context.Request.Command.Clear();
                }

                Thread.Sleep(1);
            }
        }

        private void SetupListener()
        {
            var options = new SocketListenerOptions();

            _listener = new TcpListener(options);
            _listener.Connected += ClientConnected;
            _listener.Start();
        }

        private void ClientConnected(object sender, SocketChannel channel)
        {
            channel.ChannelError += ChannelError;
            _listenerStream = new NetworkStream(channel.Client, true);
        }

        protected void ClientDisconnected(object sender, Exception execption)
        {
            _listenerStream.Close();
            _logger.RemoteDisconnect(execption);
        }

        private void ChannelError(object sender, Exception execption)
        {
            _logger.ChannelExecption(execption);
        }

        private void CommandReceived()
        {
            switch (_context.Request.Command.Name)
            {
                
                case "USER":
                    User();
                    break;

                case "PASS":
                    Pass();
                    break;

                case "QUIT":
                    Quit();
                    break;

                case "HELP":
                    Help();
                    break;

                case "NOOP":
                    Noop();
                    break;
                
                case "RNFR":
                    Authorized(() =>
                    {
                        Rnfr();
                    });
                    break;

                case "RNTO":
                    Authorized(() =>
                    {
                        Rnto();
                    });
                    break;

                case "DELE":
                    Authorized(() =>
                    {
                        Dele();
                    });
                    break;

                case "XRMD":
                case "RMD":
                    Authorized(() =>
                    {
                        Rmd();
                    });
                    break;

                case "XMKD":
                case "MKD":
                    Authorized(() =>
                    {
                        Mkd();
                    });
                    break;

                case "XPWD":
                case "PWD":
                    Authorized(() =>
                    {
                        Pwd();
                    });
                    break;

                case "SYST":
                    Authorized(() =>
                    {
                        Syst();
                    });
                    break;

                case "PORT":
                    Authorized(() =>
                    {
                        Port();
                    });
                    break;

                case "PASV":
                    Authorized(() =>
                    {
                        Pasv();
                    });
                    break;

                case "TYPE":
                    Authorized(() =>
                    {
                        Type();
                    });
                    break;

                case "MODE":
                    Authorized(() =>
                    {
                        Mode();
                    });
                    break;

                case "RETR":
                    Authorized(() =>
                    {
                        Retr();
                    });
                    break;

                case "STOR":
                    Authorized(() =>
                    {
                        Stor();
                    });
                    break;

                case "XCWD":
                case "CWD":
                    Authorized(() =>
                    {
                        Cwd();
                    });
                    break;

                case "CDUP":
                    Authorized(() =>
                    {
                        Cdup();
                    });
                    break;

                case "NLST":
                    Authorized(() =>
                    {
                        CommandNotImplemented();
                    });
                    break;

                case "LIST":
                    Authorized(() =>
                    {
                        List();
                    });
                    break;

                case "STRU":
                    Authorized(() =>
                    {
                        Stru();
                    });
                    break;

                case "PROT":
                    Authorized(() =>
                    {
                        CommandNotImplemented();
                    });
                    break;

                case "AUTH": // Extensions defined by rfc 2228
                    CommandNotImplemented();
                    break;

                case "MDMT": // Extensions defined by rfc 3659
                    Authorized(() =>
                    {
                        Mdmt();
                    });
                    break;

                case "SIZE": // Extensions defined by rfc 3659
                    Authorized(() =>
                    {
                        Size();
                    });
                    break;

                case "FEAT": // Extensions defined by rfc 2389
                    Authorized(() =>
                    {
                        Feat();
                    });
                    break;

                case "OPTS": // Extensions defined by rfc 2640
                    Authorized(() =>
                    {
                        Opts();
                    });
                    break;

                case "EPRT": // Extensions defined by rfc 2428
                    Authorized(() =>
                    {
                        Eprt();
                    });
                    break;

                case "EPSV": // Extensions defined by rfc 2428
                    Authorized(() =>
                    {
                        Epsv();
                    });
                    break;

                //TODO: Should they be added?

                case "ALLO":
                    CommandNotImplemented();
                    break;

                case "ACCT":
                    CommandNotImplemented();
                    break;

                case "REIN":
                    CommandNotImplemented();
                    break;

                case "APPE":
                    CommandNotImplemented();
                    break;

                case "REST":
                    CommandNotImplemented();
                    break;

                case "ABOR":
                    CommandNotImplemented();
                    break;

                default:
                    CommandNotImplemented();
                    break;
            }
        }

        private void Authorized(Action action)
        {
            if (_context.Request.IsAuthenticated)
            {
                action();
            }
            else
            {
                _context.Response.Write(530, "Please login with USER and PASS.");
            }
        }

        private NetworkStream GetNetworkStream()
        {
            if (_context.Request.DataMode == DataMode.Active
                || _context.Request.DataMode == DataMode.ExtendedActive)
            {
                var client = new TcpClient();
                client.NoDelay = true;
                client.Connect(_endpoint);

                return client.GetStream();
            }
            else
            {
                var timeout = 5000;
                var startTicks = DateTime.UtcNow.Ticks;

                do
                {
                    if ((DateTime.UtcNow.Ticks - startTicks) / TimeSpan.TicksPerMillisecond > timeout)
                    {
                        return null;
                    }

                    Thread.Sleep(100);

                } while (_listenerStream == null);

                return _listenerStream;
            }
        }

        private void CommandNotImplemented()
        {
            _context.Response.Write(502, "Command not implemented.");
        }

        private void ParameterNotImplemented()
        {
            _context.Response.Write(504, "Parameter not implemented.");
        }

        private void ParameterNotRecognized()
        {
            _context.Response.Write(501, "Parameter not recognized.");
        }
    }
}
