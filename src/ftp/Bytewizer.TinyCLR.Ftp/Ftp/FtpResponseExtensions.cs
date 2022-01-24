using System;
using System.Collections;
using System.Text;

using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Convenience methods for <see cref="FtpResponse"/>.
    /// </summary>
    public static class FtpResponseExtensions
    {
        /// <summary>
        /// Writes the given control message to the response.
        /// </summary>
        /// <param name="response">The <see cref="FtpResponse"/>.</param>
        /// <param name="message">The control message to write to the response.</param>
        public static void Write(this FtpResponse response, string message)
        {
            response.Message = message;
        }

        /// <summary>
        /// Writes the given control message to the response.
        /// </summary>
        /// <param name="response">The <see cref="FtpResponse"/>.</param>
        /// <param name="code">The control message response code to write to the response.</param>
        /// <param name="message">The control message to write to the response.</param>
        public static void Write(this FtpResponse response, int code, string message)
        {
            response.Message = WriteResponse(code, message);
        }

        /// <summary>
        /// Writes the given multi-line control message to the response.
        /// </summary>
        /// <param name="response">The <see cref="FtpResponse"/>.</param>
        /// <param name="code">The control message response code to write to the response.</param>
        /// <param name="start">The start control message to write to the response.</param>
        /// <param name="lines">The control message lines to write to the response.</param>
        /// <param name="end">The end control message to write to the response.</param>
        public static void Write(this FtpResponse response, int code, string start, ArrayList lines, string end)
        {
            response.Message = WriteResponse(code, start, lines, end);
        }

        /// <summary>
        /// Writes the given control message directly to the channel.
        /// </summary>
        /// <param name="channel">The <see cref="SocketChannel"/>.</param>
        /// <param name="code">The control message response code to write to the response.</param>
        /// <param name="message">The control message to write to the response.</param>
        public static void Write(this SocketChannel channel, int code, string message)
        {
            channel.Write(WriteResponse(code, message));
        }

        private static string WriteResponse(int code, string message)
        {
            VerifyCode(code);

            StringBuilder sb = new StringBuilder(6 + message.Length);

            sb.Append($"{code:D3}");
            sb.Append(" ");
            sb.AppendLine(message);

            return sb.ToString();
        }

        private static string WriteResponse(int code, string start, ArrayList lines, string end)
        {
            VerifyCode(code);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{code}-{start}");
            foreach (string line in lines)
            {
                sb.AppendLine($" {line}");
            }
            sb.AppendLine($"{code} {end}");

            return sb.ToString();
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