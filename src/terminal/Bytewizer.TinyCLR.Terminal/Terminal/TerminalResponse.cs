using System;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents the outgoing side of an individual terminal request.
    /// </summary>
    public class TerminalResponse 
    {
        /// <summary>
        /// Gets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the response message.
        /// </summary>
        public override string ToString()
        {
            if (Message != null)
            {
                return "[null]";
            }
           
            return Message;
        }

        /// <summary>
        /// Clears the <see cref="TerminalResponse"/> messgae.
        /// </summary>
        public void Clear()
        {
            Message = default;
        }
    } 
}