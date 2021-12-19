﻿using System.Text;

using Bytewizer.TinyCLR.Telnet;

namespace Bytewizer.Playground.Telnet.Commands
{
    /// <summary>
    /// Implements the <c>exit</c> telnet command.
    /// </summary>
    public class ExitCommand : Command
    {
        /// <summary>
        /// Disconnects the client by closing the connection.  This is the default action.
        /// </summary>
        public IActionResult Default()
        {
            CommandContext.TelnetContext.Active = false;
            
            return new EmptyResult();
        }

        public IActionResult Clear()
        {
            return new ClearResult();
        }

        /// <summary>
        /// Provides interactive help for the <c>exit</c> telnet command.
        /// </summary>
        public IActionResult Help()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Available Commands:");
            sb.AppendLine();
            sb.AppendLine(" exit");
            //sb.AppendLine("Disconnects the client by closing the connection.");
            sb.AppendLine();

            return new ResponseResult(sb.ToString()) { NewLine = false };
        }
    }
}