using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Security.Authentication;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Ftp.Features;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Listener;


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

            // Write ready message to channel output stream
            _context.Channel.Write(220, _ftpOptions.BannerMessage);

            // Process command loop
            while (_context.Active || _context.Channel.Connected)
            {
                byte[] buffer = new byte[128]; //128

                var bytes = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (bytes > 0)
                {
                    // parse input commands
                    _context.Request.Command = FtpCommand.Parse(buffer, 0, bytes);

                    // log request command
                    _logger.CommandRequest(_context);

                    // process the command received
                    CommandReceived();

                    if (_context.Response.Message != null)
                    {
                        // log response command
                        _logger.CommandResponse(_context);

                        // Write the response to the client
                        _context.Channel.Write(
                            _context.Response.Message
                            );
                    }

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

            var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));
            if (feature.TlsProt == null || feature.TlsProt == "C")
            {
                _listenerStream = channel.InputStream as NetworkStream;
            }
            else
            {
                var sslChannel = new SocketChannel();
                sslChannel.Assign(channel.Client, _ftpOptions.Certificate, SslProtocols.Tls12);
                _context.Channel = sslChannel;
                _listenerStream = _context.Channel.InputStream as NetworkStream;
            }
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
                case "AUTH":
                    Auth();
                    return;

                case "FEAT":
                    Feat();
                    return;

                case "HELP":
                    Help();
                    return;

                case "PASS":
                    Pass();
                    return;

                case "PBSZ":
                    Pbsz();
                    return;

                case "PROT":
                    Prot();
                    return;

                case "QUIT":
                    Quit();
                    return;

                case "SYST":
                    Syst();
                    return;

                case "USER":
                    User();
                    return;
            }

            if (_context.Request.Authenticated)
            {
                switch (_context.Request.Command.Name)
                {
                    case "APPE":
                        Appe();
                        return;

                    case "CDUP":
                        Cdup();
                        return;

                    case "CWD":
                    case "XCWD":
                        Cwd();
                        return;

                    case "DELE":
                        Dele();
                        return;

                    case "EPRT":
                        Eprt();
                        return;

                    case "EPSV":
                        Epsv();
                        return;

                    case "LIST":
                        List();
                        return;

                    case "MDTM":
                        Mdtm();
                        return;

                    case "MKD":
                    case "XMKD":
                        Mkd();
                        return;

                    case "MLSD":
                        Mlsd();
                        return;

                    case "MLST":
                        Mlst();
                        return;

                    case "MODE":
                        Mode();
                        return;

                    case "NLST":
                        Nlst();
                        return;

                    case "NOOP":
                        Noop();
                        return;

                    case "OPTS":
                        Opts();
                        return;

                    case "PASV":
                        Pasv();
                        return;

                    case "PORT":
                        Port();
                        return;

                    case "PWD":
                    case "XPWD":
                        Pwd();
                        return;

                    case "REST":
                        Rest();
                        return;

                    case "RETR":
                        Retr();
                        return;

                    case "RMD":
                    case "XRMD":
                        Rmd();
                        return;

                    case "RNFR":
                        Rnfr();
                        return;

                    case "RNTO":
                        Rnto();
                        return;

                    case "SIZE":
                        Size();
                        return;

                    case "STOR":
                        Stor();
                        return;

                    case "STRU":
                        Stru();
                        return;

                    case "TYPE":
                        Type();
                        return;
                }
            }
            else
            {
                _context.Response.Write(530, "Please login with USER and PASS.");
                return;
            }

            CommandNotImplemented();
        }

        private Stream GetNetworkStream()
        {
            if (_context.Request.DataMode == DataMode.Active
                || _context.Request.DataMode == DataMode.ExtendedActive)
            {
                var client = new TcpClient
                {
                    NoDelay = true
                };
                client.Connect(_endpoint);

                Stream stream;
                var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));
                if (feature.TlsProt == null || feature.TlsProt == "C")
                {
                    stream = new LoggerStream(client.GetStream());
                }
                else
                {
                    var streamBuilder = new SslStreamBuilder(_ftpOptions.Certificate, SslProtocols.Tls12);
                    stream = streamBuilder.Build(client.Client);
                }

                return stream;
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

        private void BadSequenceOfCommands()
        {
            _context.Response.Write(503, "Bad sequence of commands.");
        }
    }
}
