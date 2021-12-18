using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Telnet.Features;
using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{
    public class TelnetSession
    {
        private readonly ILogger _logger;
        private readonly TelnetContext _context;
        private readonly TelnetServerOptions _telnetOptions;

        private readonly CommandEndpointProvider _endpointProvider;

        public TelnetSession(
                ILogger logger,
                TelnetContext context,
                TelnetServerOptions telnetOptions)
        {
            _logger = logger;
            _context = context;
            _telnetOptions = telnetOptions;

           _endpointProvider = new CommandEndpointProvider();

            //var initBytes = new byte[] {
            //                0xff, 0xfd, 0x01,   // Do Echo
            //                0xff, 0xfd, 0x21,   // Do Remote Flow Control
            //                0xff, 0xfb, 0x01,   // Will Echo
            //                0xff, 0xfb, 0x03    // Will Supress Go Ahead
            //            };

            //_context.Channel.OutputStream.Write(initBytes, 0, initBytes.Length);

            // Write welcome message
            _context.Channel.WriteLine(_telnetOptions.WelcomeMessage);
            _context.Channel.WriteLine(string.Empty);

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
                
                var bytes = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (bytes > 0)
                {
                    // parse input commands
                    _context.Request.Command = TelnetCommand.Parse(buffer, 0, bytes);
                    if (_context.Request.Command != null)
                    { 
                        CommandReceived();
                    }

                    if (_context.Response.Message != null)
                    {
                        // Write the response to the client
                        _context.Channel.Write(
                            _context.Response.Message
                            );
                    }

                    Debug.WriteLine(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                }

                Thread.Sleep(1);
            }
        }

        private void CommandReceived()
        {
            if (_endpointProvider.TryGetEndpoint(_context.Request.Command.ToString(), out RouteEndpoint endpoint))
            {
                if (endpoint?.CommandDelegate != null)
                {
                    try
                    {
                        //_logger.LogDebug($"Executing command '{endpoint.DisplayName}'");
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
                _context.Channel.WriteLine("The specified value is invalid.");
                _context.Channel.WriteLine(string.Empty);
                return;
            }
        }

        private void PromptLogin()
        {
            var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));

            if (feature.IdentityProvider != null)
            {
                byte[] buffer = new byte[_telnetOptions.BufferSize];

                _context.Channel.Write("username:");

                var bytes = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (bytes > 0)
                {
                    feature.UserName = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Replace(Environment.NewLine, string.Empty);
                }

                _context.Channel.Write("password:");

                bytes = _context.Channel.InputStream.Read(buffer, 0, buffer.Length);
                if (bytes > 0)
                {
                    var password = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Replace(Environment.NewLine, string.Empty);

                    if (feature.UserName != null)
                    {
                        var user = feature.IdentityProvider.FindByName(feature.UserName);
                        if (user != null)
                        {
                            var results = feature.IdentityProvider.VerifyPassword(user, password);
                            if (results.Succeeded)
                            {
                                _context.Request.Authenticated = true;
                                _context.Channel.WriteLine(string.Empty);
                                return;
                            }
                        }
                    }
                }

                _context.Channel.WriteLine("Logon failure: Unknown user name or bad password.");
                _context.Channel.WriteLine(string.Empty);
            }
            else
            {
                _context.Request.Authenticated = true;
            }
        }
    }
}