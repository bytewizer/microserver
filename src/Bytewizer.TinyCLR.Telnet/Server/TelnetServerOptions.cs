using System;
using System.Reflection;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Represents an implementation of the <see cref="ServerOptions"/> for creating telnet servers.
    /// </summary>
    public class TelnetServerOptions : ServerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetServerOptions"/> class.
        /// </summary>
        public TelnetServerOptions()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            WelcomeMessage = $"Welcome to TinyCLR Telnet Server [{version}]";
            HelpMessage = "Enter 'help' for a list of built-in commands";
            Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Specifies the command buffer size.
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        /// <summary>
        /// Specifies the assemblies used to search for command providers.
        /// </summary>
        public Assembly[] Assemblies { get; set; }

        /// <summary>
        /// Specifies the session command prompt.
        /// </summary>
        public string CommandPrompt { get; set; } = "$";

        /// <summary>
        /// Specifies the session welecom message.
        /// </summary>
        public string WelcomeMessage { get; set; }

        /// <summary>
        /// Specifies the session help message.
        /// </summary>
        public string HelpMessage { get; set; }
    }
}