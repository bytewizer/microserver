using System;
using System.IO;

namespace Bytewizer.TinyCLR.Http.Extensions
{
    public static class StreamExtensions
    {
        public static void CopyTo(this Stream src, Stream dest)
        {
            int maxChunkSize = 0x100;
            int size = (src.CanSeek) ? Math.Min((int)(src.Length - src.Position), maxChunkSize) : maxChunkSize;
            byte[] buffer = new byte[size];
            int n;
            do
            {
                n = src.Read(buffer, 0, buffer.Length);
                if (n == -1) n=0;
                dest.Write(buffer, 0, n);
            } while (n != 0);
        }

        public static void CopyTo(this MemoryStream src, Stream dest)
        {
            dest.Write(src.GetBuffer(), (int)src.Position, (int)(src.Length - src.Position));
        }

        public static void CopyTo(this Stream src, MemoryStream dest)
        {
            if (src.CanSeek)
            {
                int pos = (int)dest.Position;
                int length = (int)(src.Length - src.Position) + pos;
                dest.SetLength(length);

                while (pos < length)
                    pos += src.Read(dest.GetBuffer(), pos, length - pos);
            }
            else
                src.CopyTo((Stream)dest);
        }
    }
}
