using System;
using System.Threading;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
{
    public class ConsoleSession
    {
        private readonly ILogger _logger;
        private readonly ConsoleContext _context;
        private readonly TerminalServerOptions _terminalOptions;

        public ConsoleSession(
                ILogger logger,
                TerminalContext context,
                ConsoleServerOptions terminalOptions)
        {
            _logger = logger;
            _context = context as ConsoleContext;
            _terminalOptions = terminalOptions;

            // Send client protocol initialization
            NegotiateProtocol();

            // Write welcome message
            SendBannerMessage();

            // Process command loop
            CommandLoop();
        }

        private void NegotiateProtocol()
        {
            // Tell the telnet client that we'd like to operate in line-at-a-time mode
            var mode = new byte[] {
                    0xff, 0xfc, 0x01   // IAC, DO, ECHO
                };

            //_context.Channel.Write(mode);

            Thread.Sleep(100);

            var buffer = new byte[1024];
            _context.Channel.Client.Stream.Read(buffer, 0, buffer.Length);
            //_context.OptionCommands = buffer;
        }

        private void SendBannerMessage()
        {
            _context.Channel.WriteLine(_terminalOptions.WelcomeMessage);
            _context.Channel.WriteLine(_terminalOptions.HelpMessage);
            _context.Channel.WriteLine(string.Empty);
        }

        private void CommandLoop()
        {
            var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));

            // Initialize command buffer
            byte[] buffer = new byte[_terminalOptions.BufferSize];

            // Process command loop
            while (_context.Active)
            {
                if (_context.Channel.Client.Stream.BytesToRead == 0)
                {
                    continue;
                }
                
                _context.Response.Clear();

                if (feature.Authenticated == false)
                {
                    PromptLogin();
                    continue;
                }

                // Write command prompt
                _context.Channel.Write($"{_terminalOptions.CommandPrompt} ");

                // Clear command buffer 
                Array.Clear(buffer, 0, _terminalOptions.BufferSize);

                var byteCount = _context.Channel.Read(buffer);
                if (byteCount > 0)
                {
                    // log request command
                    //_logger.CommandRequest(_context, buffer, byteCount);

                    // parse input commands
                    _context.Request.Command = CommandLine.Parse(buffer, byteCount);
                    if (_context.Request.Command != null)
                    {
                        CommandReceived();
                    }

                    if (_context.Response.Message != null)
                    {
                        // log response command
                        //_logger.CommandResponse(_context);

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
            if (_context.TryGetEndpoint(_context.Request.Command.ToString(), out Endpoint endpoint))
            {
                if (endpoint?.CommandDelegate != null)
                {
                    try
                    {
                        //_logger.CommandExecuted(_context, endpoint);
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
                if (_context.Request.Command.Action == "default")
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
            // Adds the timer to check for a login timeout
            if (_terminalOptions.TimeToLogin > 0)
            {
                _ = new Timer(LoginTimedOut, null, _terminalOptions.TimeToLogin * 1000, 0);
            }

            var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));

            if (feature.IdentityProvider != null)
            {
                byte[] buffer = new byte[_terminalOptions.BufferSize];

                _context.Channel.WriteLine("User Access Verification");

                for (int i = 0; i < _terminalOptions.Retries; ++i)
                {
                    _context.Channel.Write("username: ");

                    Array.Clear(buffer, 0, _terminalOptions.BufferSize);

                    var byteCount = _context.Channel.Read(buffer);
                    if (byteCount > 0)
                    {
                        feature.UserName = buffer.ToEncodedString(byteCount);
                    }
                    else
                    {
                        return;
                    }

                    _context.Channel.Write("password: ");

                    byteCount = _context.Channel.Read(buffer);
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
                                    feature.Authenticated = true;
                                    //_logger.LoginSucceeded(_context);
                                    _context.Channel.WriteLine(string.Empty);
                                    return;
                                }
                                else
                                {
                                    foreach (Exception error in results.Errors)
                                    {
                                        //_logger.LoginFailed(_context, error.Message);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }

                    //_logger.LoginFailed(_context, $"Unknown user name or invalid identity.");

                    _context.Channel.WriteLine("Logon failure: Unknown user name or bad password.");
                    _context.Channel.WriteLine(string.Empty);
                }

                _context.Channel.WriteLine("Closing the connection");
                Thread.Sleep(100);
                //_context.Channel.Close();
            }
            else
            {
                // if authentication is not added to the pipeline allow unscure access.
                feature.Authenticated = true;
                feature.UserName = "no authentication required";
            }
        }

        private void LoginTimedOut(object Param)
        {
            var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));

            if (feature.Authenticated == true)
            {
                return;
            }

            _context.Channel.WriteLine("\r\nLogin timeout");
            Thread.Sleep(100);
            //_context.Channel.Close();
        }
    }
}