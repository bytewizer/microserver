using System;
using System.Text;

using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Convenience methods for <see cref="FtpResponse"/>.
    /// </summary>
    public static class ChannelExtensions
    {
        public static void Write(this SocketChannel channel, int code, string message)
        {
            VerifyCode(code);

            StringBuilder sb = new StringBuilder(6 + message.Length);

            sb.Append($"{code:D3}");
            sb.Append(" ");
            sb.AppendLine(message);

            channel.Write(sb.ToString());
        }

        private static void VerifyCode(int code)
        {
            if (code < 100 || code > 999)
            {
                throw new ArgumentException("Protocol violation no such status code.");
            }
        }
    }
}