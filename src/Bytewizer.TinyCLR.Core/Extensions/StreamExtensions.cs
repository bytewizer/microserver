using System;
using System.IO;

namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// Contains extension methods for <see cref="Stream"/> and <see cref="MemoryStream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to read from.</param>
        /// <param name="buffer">An array of bytes.</param>
        public static void Write(this Stream source, byte[] buffer)
        {
            source.Write(buffer, 0, buffer.Length);
        }
        
        /// <summary>
        /// Reads the bytes from the current stream and writes them to another stream.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to read from.</param>
        /// <param name="destination">The <see cref="Stream"/> to which the contents of the current stream will be copied.</param>
        public static void CopyTo(this Stream source, Stream destination)
        {
            int maxChunkSize = 0x2000;
            
            int size = (source.CanSeek) ? Math.Min((int)(source.Length - source.Position), maxChunkSize) : maxChunkSize;
            byte[] buffer = new byte[size];
            int n;
            do
            {
                n = source.Read(buffer, 0, buffer.Length);
                if (n == -1) n=0;
                destination.Write(buffer, 0, n);
            } while (n != 0);
        }

        //public static void CopyTo(this MemoryStream src, Stream dest)
        //{
        //    dest.Write(src.GetBuffer(), (int)src.Position, (int)(src.Length - src.Position));
        //}

        //public static void CopyTo(this Stream src, MemoryStream dest)
        //{
        //    if (src.CanSeek)
        //    {
        //        int pos = (int)dest.Position;
        //        int length = (int)(src.Length - src.Position) + pos;
        //        dest.SetLength(length);

        //        while (pos < length)
        //            pos += src.Read(dest.GetBuffer(), pos, length - pos);
        //    }
        //    else
        //        src.CopyTo((Stream)dest);
        //}
    }
}
