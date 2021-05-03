using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Extensions
{
    /// <summary>
    /// Contains extension methods for working with <see cref="Stream"/> instances.
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// Reads a line of text from a <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns></returns>
        public static string ReadLine(this Stream stream)
        {

            //int next_char;
            //StringBuilder data = new StringBuilder();
            //while (true)
            //{
            //    next_char = stream.ReadByte();
            //    if (next_char == '\n')
            //    {
            //        break;
            //    }
            //    if (next_char == '\r')
            //    {
            //        continue;
            //    }
            //    if (next_char == -1)
            //    {
            //        continue;
            //    };
            //    data.Append(Convert.ToChar((ushort)next_char));
            //}
            //return data.ToString();


            var line = new StringBuilder();

            // Read until newline.
            int buffer;
            while ((buffer = stream.ReadByte()) != '\n')
            {
                switch (buffer)
                {
                    case '\r':
                        break;
                    case -1:
                        break;
                    default:
                        line.Append(Convert.ToChar((ushort)buffer));
                        break;
                }
            }

            return line.ToString();
        }

        /// <summary>
        /// Will fill the entire buffer from the stream. Will throw an exception when the underlying stream is closed.
        /// </summary>
        public static void ReadBuffer(this Stream stream, byte[] buffer)
        {
            int count = 0;

            do
            {
                int read = stream.Read(buffer, count, buffer.Length - count);

                if (read <= 0)
                    throw new Exception("Stream closed unexpectedly by the remote server");

                count += read;
            } while (count < buffer.Length);
        }
    }
}
