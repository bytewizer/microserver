using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="SocketException"/>.
    /// </summary>
    public static class SocketExceptionExtensions
    {
        internal static bool IsIgnorableSocketException(this SocketException se)
        {
            // TODO: What final list of error codes should be ingorable on TinyCLR?

            if (se.ErrorCode == (int)SocketError.ConnectionAborted)
                return true;

            if (se.ErrorCode == (int)SocketError.ConnectionRefused)
                return true;

            if (se.ErrorCode == (int)SocketError.Shutdown)
                return true;

            if (se.ErrorCode == (int)SocketError.ConnectionReset)
                return true;

            if (se.ErrorCode == (int)SocketError.Interrupted)
                return true;

            if (se.ErrorCode == (int)SocketError.NotSocket)
                return true;

            // Not sure about these
            if (se.ErrorCode == 89)
                return true;

            if (se.ErrorCode == 125)
                return true;

            if (se.ErrorCode == 104)
                return true;

            if (se.ErrorCode == 54)
                return true;

            if (se.ErrorCode == 995)
                return true;

            return false;
        }
    }
}
