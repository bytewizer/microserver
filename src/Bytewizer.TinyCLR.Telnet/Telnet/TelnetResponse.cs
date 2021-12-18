using System;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Represents the outgoing side of an individual Telnet request.
    /// </summary>
    public class TelnetResponse 
    {
        /// <summary>
        /// Gets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the response code.
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
        /// Clears the <see cref="TelnetResponse"/> messgae.
        /// </summary>
        public void Clear()
        {
            Message = default;
        }
    } 
}