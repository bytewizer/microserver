using System;
using System.Text;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Contains extension methods for byte array.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Convert byte array to UTF8 encoded string with newlines removed.
        /// </summary>
        /// <param name="source">The source byte array.</param>
        /// <param name="count">The number of bytes to decode.</param>
        public static string ToEncodedString(this byte[] source, int count)
        {
            return Encoding.UTF8.GetString(source, 0, count).Replace(Environment.NewLine, string.Empty);
        }
    }
}