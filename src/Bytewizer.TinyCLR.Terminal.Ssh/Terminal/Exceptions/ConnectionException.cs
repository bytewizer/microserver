using System;

namespace Bytewizer.TinyCLR.SecureShell
{
    public class ConnectionException : Exception
    {
        public ConnectionException()
        {
        }

        public ConnectionException(string message, DisconnectReason disconnectReason = DisconnectReason.None)
            : base(message)
        {
            DisconnectReason = disconnectReason;
        }

        public DisconnectReason DisconnectReason { get; private set; }

        public override string ToString()
        {
            return string.Format("Secure shell connection disconnected because {0}", DisconnectReason);
        }
    }
}
