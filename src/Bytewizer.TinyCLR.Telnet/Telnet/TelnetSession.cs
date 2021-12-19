using System;
using System.Threading;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Telnet.Features;

namespace Bytewizer.TinyCLR.Telnet
{
    public class TelnetSession
    {
        private readonly ILogger _logger;
        private readonly TelnetContext _context;
        private readonly TelnetServerOptions _telnetOptions;

        public TelnetSession(
                ILogger logger,
                TelnetContext context,
                TelnetServerOptions telnetOptions)
        {
            _logger = logger;
            _context = context;
            _telnetOptions = telnetOptions;

            // Send client protocol initialization
            SendProtocolInit();

            // Write welcome message
            SendWelcomeMessage();

            // Process command loop
            CommandLoop();
        }

        private void SendProtocolInit()
        {
            //var initBytes = new byte[] {
            //                0xff, 0xfd, 0x01,   // Do Echo
            //                0xff, 0xfd, 0x21,   // Do Remote Flow Control
            //                0xff, 0xfb, 0x01,   // Will Echo
            //                0xff, 0xfb, 0x03    // Will Supress Go Ahead
            //            };

            //_context.Channel.OutputStream.Write(initBytes, 0, initBytes.Length);
        }

        private void SendWelcomeMessage()
        {
            _context.Channel.WriteLine(_telnetOptions.WelcomeMessage);
            _context.Channel.WriteLine(_telnetOptions.HelpMessage);
            _context.Channel.WriteLine(string.Empty);
        }

        private void CommandLoop()
        {
            // Initialize command buffer
            byte[] buffer = new byte[_telnetOptions.BufferSize];

            // Process command loop
            while (_context.Active && _context.Channel.Connected)
            {
                _context.Response.Clear();

                if (_context.Request.Authenticated == false)
                {
                    PromptLogin();
                    continue;
                }

                // Write command prompt
                _context.Channel.Write($"{_telnetOptions.CommandPrompt} ");

                // Clear command buffer 
                Array.Clear(buffer, 0, _telnetOptions.BufferSize);

                var byteCount = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (byteCount > 0)
                {
                    // log request command
                    _logger.CommandRequest(_context, buffer, byteCount);

                    // parse input commands
                    _context.Request.Command = TelnetCommand.Parse(buffer, byteCount);
                    if (_context.Request.Command != null)
                    {
                        CommandReceived();
                    }

                    if (_context.Response.Message != null)
                    {
                        // log response command
                        _logger.CommandResponse(_context);

                        // Write the response to the client
                        _context.Channel.Write(
                            _context.Response.Message
                            );
                    }
                }

                Thread.Sleep(1);
            }
        }

        private void CommandReceived()
        {
            if (_context.TryGetEndpoint(_context.Request.Command.ToString(), out RouteEndpoint endpoint))
            {
                if (endpoint?.CommandDelegate != null)
                {
                    try
                    {
                        _logger.CommandExecuted(_context, endpoint);
                        endpoint.CommandDelegate(_context);
                    }
                    catch (Exception ex)
                    {
                        _logger.UnhandledException(ex);
                    }
                }
            }
            else
            {
                if(_context.Request.Command.Action == "default")
                {
                    _context.Channel.WriteLine($"The specified '{_context.Request.Command.Name}' command is invalid.");
                }
                else 
                {
                    _context.Channel.WriteLine($"The specified '{_context.Request.Command.Name} {_context.Request.Command.Action}' command is invalid.");
                }
                _context.Channel.WriteLine(string.Empty);
            }
        }

        private void PromptLogin()
        {
            var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));

            if (feature.IdentityProvider != null)
            {
                byte[] buffer = new byte[_telnetOptions.BufferSize];

                _context.Channel.WriteLine("User Access Verification");
                _context.Channel.Write("username: ");

                var byteCount = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (byteCount > 0)
                {
                    feature.UserName = buffer.ToEncodedString(byteCount); 
                }

                _context.Channel.Write("password: ");

                byteCount = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (byteCount > 0)
                {
                    var password = buffer.ToEncodedString(byteCount);

                    if (feature.UserName != null)
                    {
                        var user = feature.IdentityProvider.FindByName(feature.UserName);
                        if (user != null)
                        {
                            var results = feature.IdentityProvider.VerifyPassword(user, password);
                            if (results.Succeeded)
                            {
                                _context.Request.Authenticated = true;
                                _logger.LoginSucceeded(_context);
                                _context.Channel.WriteLine(string.Empty);
                                return;
                            }
                            else
                            {
                                foreach (Exception error in results.Errors)
                                {
                                    _logger.LoginFailed(_context, error.Message);
                                }
                            }
                        }
                    }
                }

                _logger.LoginFailed(_context, $"Unknown user name or invalid identity.");

                _context.Channel.WriteLine("Logon failure: Unknown user name or bad password.");
                _context.Channel.WriteLine(string.Empty);
            }
            else
            {
                // if authentication is not added to the pipeline allow unscure access.
                _context.Request.Authenticated = true;
            }
        }
    }
}