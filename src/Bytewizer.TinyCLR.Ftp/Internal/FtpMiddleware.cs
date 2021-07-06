using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Ftp.Internal
{
    internal class FtpMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly FtpServerOptions _ftpOptions;

        private FtpContext _context;

        public FtpMiddleware(ILogger logger, FtpServerOptions options)
        {
            _logger = logger;
            _ftpOptions = options;
        }

        protected override void Invoke(FtpContext context, RequestDelegate next)
        {
            _context = context;

            var command = context.Request.Command;
            Debug.WriteLine(string.Format($"{command.Name} {command.Argument}"));

            //if (context.Request.Command.Name != "AUTH" 
            //    && context.Request.Command.Name != "USER" 
            //    && context.Request.Command.Name != "PASS")
            //{
            //    context.Channel.Write(530, "Please login with USER and PASS.");
            //    return;
            //}

            switch (context.Request.Command.Name)
            {
                case "USER":
                    CommandUser();
                    break;

                case "PASS":
                    CommandPass();
                    break;

                case "SYST":
                    CommandSyst();
                    break;

                case "FEAT":
                    CommandFeat();
                    break;

                case "OPTS":
                    CommandOpts();
                    break;

                case "PWD":
                    CommandPwd();
                    break;

                case "TYPE":
                    CommandType();
                    break;

                case "QUIT":
                    CommandQuit();
                    break;

                case "PASV":
                    CommandPasv();
                    break;

                case "PORT":
                    CommandPort();
                    break;
            }
        }

        private void CommandUser()
        {
            _context.User = _context.Request.Command.Argument;
            if (_context.User == "anonymous" && _ftpOptions.AllowAnonymous)
            {
                _context.Channel.Write(331, "Anonymous access allowed, send identity (e-mail name) as password.");
            }
            else
            {
                _context.Channel.Write(331, $"Password required for {_context.User}.");
            }
        }

        public void CommandPass()
        {
            _context.Channel.Write(230, "User logged in.");
            _context.Request.IsAuthenticated = true;

            //AuthEventArgs args = new AuthEventArgs(Session.User, Request.Argument);
            //if (Service.FTP.Configuration.AnonymousAuthentication)
            //{
            //    args.IsAuthenticated = true;
            //}
            //else
            //{
            //    Service.FTP.Configuration.AuthMethod(this, args);
            //}
            //if (args.IsAuthenticated)
            //{
            //    com.SendControl(Session, "230 User logged in.");
            //    Session.WorkingDirectory = Service.FTP.Configuration.DocumentRoot;
            //    Session.IsAuthenticated = true;
            //}
            //else
            //{
            //    Session.User = string.Empty;
            //    com.SendControl(Session, "530-User cannot log in.\r\n Logon failure: Unknown user name or bad password.\r\n530 End");
            //}
        }

        public void CommandSyst()
        {
            _context.Channel.Write(215, "UNIX simulated by TinyCLR OS.");
        }

        public void CommandFeat()
        {
            _context.Channel.Write(211, "Extensions supported:", new string[] { "UTF8" }, "End");
        }

        public void CommandOpts()
        {
            if (_context.Request.Command.Argument == "UTF8 ON")
            {
                _context.Channel.Write(200, "UTF-8 is on");
            }
            else if (_context.Request.Command.Argument == "UTF8 OFF" )
            {
                _context.Channel.Write(502, "Command not implemented.");
            }
        }

        public void CommandPwd()
        {
            _context.Channel.Write(257, $"{ConvertPathToRemote("A:\\")} is current directory.");
        }
        
        private string ConvertPathToRemote(string Directory)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('/');

            char[] chars = Directory.ToCharArray(3, Directory.Length - 3);  // without leading A|B:\
            char c;
            for (int i = 0; i < chars.Length; i++)
            {
                c = chars[i];

                if (c == '\\')
                {
                    sb.Append('/');
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public void CommandType()
        {
            if (_context.Request.Command.Argument == "A" 
                || _context.Request.Command.Argument == "I")
            {
                _context.Channel.Write(215, $"TYPE set to {_context.Request.Command.Argument}.");
            }
            else
            {
                _context.Channel.Write(501, "Parameter not understood.");
            }
        }

        public void CommandQuit()
        {
            _context.Channel.Write(221, "Goodbye.");
            _context.Abort();
        }

        private void CommandPasv()
        {
            var localEP = _context.Connection.LocalIpAddress;
            var ipBytes = localEP.GetAddressBytes();
            if (ipBytes.Length != 4)
            {
                throw new Exception("IPv6 is not supported");
            }

            var passiveEPString =
                string.Format(
                    "{0},{1},{2},{3},{4},{5}",
                    ipBytes[0],
                    ipBytes[1],
                    ipBytes[2],
                    ipBytes[3],
                    (byte)(_context.Connection.LocalPort / 256),
                    (byte)(_context.Connection.LocalPort % 256));
            
            _context.Channel.Write(227, "Entering Passive Mode (" + passiveEPString + ")");
        }

        public void CommandPort()
        {
            string[] addr;
            string IP;
            int Port;

            try
            {
                addr = _context.Request.Command.Argument.Split(new char[] { ',' });
                IP = addr[0] + "." + addr[1] + "." + addr[2] + "." + addr[3];
                Port = (int.Parse(addr[4]) * 0x100) + int.Parse(addr[5]);

                _context.Channel.Client.Connect(new IPEndPoint(IPAddress.Parse(IP), Port));

                _context.Channel.Write(200, "PORT command successful.");
            }
            catch
            {
                _context.Channel.Write(500, "PORT command failed.");
                _context.Abort();
            }
        }
    }
}