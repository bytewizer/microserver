using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Ftp.Features;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private Socket _listenSocket;
        private Socket _remoteSocket;

        private IPEndPoint _endpoint;
        private SocketChannel _socketChannel;
        private TcpListener _listener;

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

            // set root and user path
            _fileProvider.SetWorkingDirectory(_ftpOptions.RootPath);

            // Write ready message to channel output stream
            _context.Channel.Write(220, _ftpOptions.BannerMessage);

            // Process command loop
            byte[] buffer = new byte[4096];
            while (_context.Channel.Connected)
            {
                var bytes = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (bytes > 0)
                {
                    try
                    {
                        _context.Request.Command = FtpCommand.Parse(buffer, 0, bytes);
                    }
                    catch
                    {
                        _context.Response.Write(500, "Failed to read command closing connection.");
                        break;
                    }

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
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            _listenSocket.Listen(4);

            var thread = new Thread(() =>
            {
                if (_listenSocket != null)
                {
                    _remoteSocket = _listenSocket.Accept();
                    Debug.WriteLine(_remoteSocket.RemoteEndPoint.ToString());
                }
            });
            thread.Start();


            //var options = new SocketListenerOptions();

            //_listener = new TcpListener(options);
            //_listener.Connected += ClientConnected;
            //_listener.Start();

            //Debug.WriteLine(_listener.ActivePort.ToString());
        }

        private void ClientConnected(object sender, SocketChannel channel)
        {
            Debug.WriteLine("Assigned Channel");

            _socketChannel = channel;
        }

        private void CommandReceived()
        {
            switch (_context.Request.Command.Name)
            {
                case "RNFR":
                    Rnfr();
                    break;

                case "RNTO":
                    Rnto();
                    break;

                case "DELE":
                    Dele();
                    break;

                case "SIZE":
                    Size();
                    break;

                case "XRMD":
                case "RMD":
                    Rmd();
                    break;

                case "XMKD":
                case "MKD":
                    Mkd();
                    break;

                case "XPWD":
                case "PWD":
                    Pwd();
                    break;

                case "SYST":
                    if (Authorized())
                    {
                        Syst();
                    }
                    break;

                case "FEAT":
                    Feat();
                    break;

                case "HELP":
                    Help();
                    break;

                case "OPTS":
                    Opts();
                    break;

                case "USER":
                    User();
                    break;

                case "PASS":
                    Pass();
                    break;

                case "PORT":
                    Port();
                    break;

                case "EPRT": // Defined by Rfc3659
                    Eprt();
                    break;

                case "PASV":
                    Pasv();
                    break;

                case "EPSV": // Defined by Rfc3659
                    Epsv();
                    break;

                case "TYPE":
                    Type();
                    break;

                case "MODE":
                    CommandNotImplemented();
                    break;

                case "QUIT":
                    Quit();
                    break;

                case "RETR":
                    Retr();
                    break;

                case "STOR":
                    Stor();
                    break;

                case "XCWD":
                case "CWD":
                    Cwd();
                    break;

                case "CDUP":
                    Cdup();
                    break;

                case "NLST":
                    CommandNotImplemented();
                    break;

                case "LIST":
                    List();
                    break;

                case "NOOP":
                    Noop();
                    break;

                case "AUTH":
                    CommandNotImplemented();
                    break;

                case "PROT":
                    CommandNotImplemented();
                    break;

                default:
                    CommandNotImplemented();
                    break;
            }
        }

        private bool Authorized()
        {
            if (_context.Request.IsAuthenticated)
            {
                return true;
            }
            else
            {
                _context.Response.Write(530, "Please login with USER and PASS.");
            }

            return false;
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
                return new NetworkStream(_remoteSocket, true);
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
    }
}
