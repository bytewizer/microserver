using System;
using System.Text;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Convenience methods for <see cref="FtpResponse"/>.
    /// </summary>
    public static class FtpResponseExtensions
    {
        public static void Write(this FtpResponse response, string message)
        {
            response.Message = message;
        }

        public static void Write(this FtpResponse response, int code, string message)
        {
            VerifyCode(code);

            StringBuilder sb = new StringBuilder(6 + message.Length);

            sb.Append($"{code:D3}");
            sb.Append(" ");
            sb.AppendLine(message);

            response.Message = sb.ToString();
        }

        public static void Write(this FtpResponse response, int code, string start, string[] lines, string end)
        {
            VerifyCode(code);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{code}-{start}");
            foreach (string line in lines)
            {
                sb.AppendLine($" {line}");
            }
            sb.AppendLine($"{code} {end}");

            response.Message = sb.ToString();
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