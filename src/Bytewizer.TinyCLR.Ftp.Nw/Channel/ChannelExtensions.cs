using System.Text;

using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Convenience methods for <see cref="SocketChannel"/>.
    /// </summary>
    public static class SocketChannelExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public static void Write(this SocketChannel channel, int code, string message)
        {
            StringBuilder sb = new StringBuilder(6 + message.Length);
            
            sb.Append($"{code:D3}");
            sb.Append(" ");
            sb.AppendLine(message);

            channel.Write(sb.ToString());
        }

        public static void Write(this SocketChannel channel, int code, string start, string[] lines, string end)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{code}-{start}");
            foreach(string line in lines)
            {
                sb.AppendLine($" {line}");
            }
            sb.AppendLine($"{code} {end}");

            channel.Write(sb.ToString());
        }
    }
}