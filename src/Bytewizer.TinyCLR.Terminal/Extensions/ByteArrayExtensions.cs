using System;
using System.Text;

namespace Bytewizer.TinyCLR.Terminal
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
            return Encoding.UTF8.GetString(source, 0, count).Replace(AnsiSequences.NewLine, string.Empty);
        }

        /// <summary>
        /// Returns a specified number of contiguous bytes.
        /// </summary>
        /// <param name="value">The array to return a number of bytes from.</param>
        /// /// <param name="count">The number of bytes to take from <paramref name="value"/>.</param>
        public static byte[] Take(this byte[] value, int count)
        {
            return Take(value, 0, value.Length);
        }

        /// <summary>
        /// Returns a specified number of contiguous bytes from a given offset.
        /// </summary>
        /// <param name="value">The array to return a number of bytes from.</param>
        /// <param name="offset">The zero-based offset in <paramref name="value"/> at which to begin taking bytes.</param>
        /// <param name="count">The number of bytes to take from <paramref name="value"/>.</param>
        /// <returns>
        /// A <see cref="byte"/> array that contains the specified number of bytes at the specified offset
        /// of the input array.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <remarks>
        /// When <paramref name="offset"/> is zero and <paramref name="count"/> equals the length of <paramref name="value"/>,
        /// then <paramref name="value"/> is returned.
        /// </remarks>
        public static byte[] Take(this byte[] value, int offset, int count)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (count == 0)
            {
                return new byte[0];
            }

            if (offset == 0 && value.Length == count)
            {
                return value;
            }

            var taken = new byte[count];
            Array.Copy(value, offset, taken, 0, count);

            return taken;
        }

    }
}