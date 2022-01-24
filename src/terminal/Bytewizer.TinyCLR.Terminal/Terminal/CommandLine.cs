using System;
using System.Collections;
using System.Text;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Terminal command with argument.
    /// </summary>
    public sealed class CommandLine
    {
        private static readonly char[] _whiteSpaces = { ' ', '\t' };

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine"/> class.
        /// </summary>
        public CommandLine() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine"/> class.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <param name="action">The command action.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <param name="arguments">The command argument flags.</param>
        public CommandLine(string name, string action, string[] parameters, Hashtable arguments)
        {
            Name = name.Replace(AnsiSequences.NewLine, string.Empty);
            Action = action.Replace(AnsiSequences.NewLine, string.Empty);
            Arguments = arguments;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the command action.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Gets the command parameters.
        /// </summary>
        public string[] Parameters { get; private set; }

        /// <summary>
        /// Gets the command arguments object.
        /// </summary>
        public Hashtable Arguments { get; private set; }

        /// <summary>
        /// Splits the <paramref name="buffer"/> into the name and its arguments.
        /// </summary>
        /// <param name="buffer">The command to split into name and arguments.</param>
        /// <param name="count">The number of bytes to parse.</param>
        public static CommandLine Parse(byte[] buffer, int count)
        {
            var command = buffer.ToEncodedString(count);

            if (string.IsNullOrEmpty(command.Trim()))
            {
                return null;    
            }

            var arguments = ParseCommandLine(command); // command.Split(new char[] { ' ' });

            var flags = new ArrayList();
            var parts = new ArrayList();
            foreach (var argument in arguments)
            {
                if (argument.StartsWith("--"))
                {
                    flags.Add(argument);
                }
                else
                {
                    parts.Add(argument);
                }
            }

            if (parts.Count == 0)
            {
                return null;
            }

            var commandName = ((string)parts[0]).Trim().ToLower();
            var commandAction = parts.Count > 1 ? ((string)parts[1]).Trim().ToLower() : "default";
            var commandParms = new string[parts.Count > 2 ? parts.Count - 2 : 0];
            
            for (int i = 2; i < parts.Count; i++)
            {
                commandParms[i - 2] = ((string)parts[i]).Trim().ToLower();
            }

            var commandArguments = ArgumentParser.ParseArguments(flags);

            return new CommandLine(
                commandName,
                commandAction,
                commandParms,
                commandArguments);
        }

        public override string ToString()
        {
            return $"{Name}/{Action}";
        }

        /// <summary>
        /// Clears the <see cref="CommandLine"/> name and arguments.
        /// </summary>
        public void Clear()
        {
            Name = default;
            Action = default;
            Parameters = default;
            Arguments = default;
        }
        
        private static string[] ParseCommandLine(string cmdLine)
        {
            var args = new ArrayList();
            if (string.IsNullOrEmpty(cmdLine))
            {
                return new string[0];
            }

            var currentArg = new StringBuilder();
            bool inQuotedArg = false;

            for (int i = 0; i < cmdLine.Length; i++)
            {
                if (cmdLine[i] == '"')
                {
                    if (inQuotedArg)
                    {
                        args.Add(currentArg.ToString());
                        currentArg = new StringBuilder();
                        inQuotedArg = false;
                    }
                    else
                    {
                        inQuotedArg = true;
                    }
                }
                else if (cmdLine[i] == ' ')
                {
                    if (inQuotedArg)
                    {
                        currentArg.Append(cmdLine[i]);
                    }
                    else if (currentArg.Length > 0)
                    {
                        args.Add(currentArg.ToString());
                        currentArg = new StringBuilder();
                    }
                }
                else
                {
                    currentArg.Append(cmdLine[i]);
                }
            }

            if (currentArg.Length > 0) args.Add(currentArg.ToString());

            return (string[])args.ToArray(typeof(string));
        }
    }
}