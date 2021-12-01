using System;
using System.IO;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Contains extension methods for <see cref="StreamReader"/>.
    /// </summary>
    static class StreamReaderExtensions
    {
        /// <summary>
        /// Skips white space in the current stream.
        /// </summary>
        /// <param name="reader">The source <see cref="StreamReader"/>.</param>
        public static void SkipWhiteSpace(this StreamReader reader)
        {
            while (true)
            {
                var raw = reader.Peek();
                if (raw == -1)
                {
                    break;
                }
                var ch = (char)raw;
                if (!ch.IsWhiteSpace())
                {
                    break;
                }
                reader.Read();
            }
        }
        public static void ReadBuffer(this StreamReader reader, char[] buffer)
        {
            int count = 0;

            do
            {
                int read = reader.Read(buffer, count, buffer.Length - count);

                //if (read <= 0)
                //    throw new Exception("Stream closed unexpectedly by the remote server");

                count += read;
            } while (count < buffer.Length);
        }
    }
}