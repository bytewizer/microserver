using System.IO;

namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// Contains extension methods for <see cref="Stream"/> object.
    /// </summary>
    public static class StreamExtensions
    {
        private static readonly int _retry = 5;

        internal static byte[] ReadBytes(this Stream stream, int length)
        {
            var buff = new byte[length];
            var offset = 0;
            var retry = 0;

            while (length > 0)
            {
                int nread = stream.Read(buff, offset, length);
                if (nread <= 0)
                {
                    if (retry < _retry)
                    {
                        retry++;
                        continue;
                    }

                    return buff.SubArray(0, offset);
                }

                retry = 0;

                offset += nread;
                length -= nread;
            }

            return buff;
        }

        internal static byte[] ReadBytes(this Stream stream, long length, int bufferLength)
        {
            using (var dest = new MemoryStream())
            {
                var buff = new byte[bufferLength];
                var retry = 0;
                var nread = 0;

                while (length > 0)
                {
                    if (length < bufferLength)
                    {
                        bufferLength = (int)length;
                    }

                    nread = stream.Read(buff, 0, bufferLength);
                    if (nread <= 0)
                    {
                        if (retry < _retry)
                        {
                            retry++;
                            continue;
                        }

                        break;
                    }

                    retry = 0;

                    dest.Write(buff, 0, nread);
                    length -= nread;
                }

                dest.Close();
                return dest.ToArray();
            }
        }

        internal static void WriteBytes(this Stream stream, byte[] bytes, int bufferLength)
        {
            using (var src = new MemoryStream(bytes))
            {
                src.CopyTo(stream, bufferLength);
            }
        }

        //internal static void CopyTo(this Stream source, Stream destination, int bufferLength)
        //{
        //    var buffer = new byte[bufferLength];

        //    while (true)
        //    {
        //        int nread = source.Read(buffer, 0, bufferLength);
        //        if (nread <= 0)
        //        {
        //            break;
        //        }

        //        destination.Write(buffer, 0, nread);
        //    }
        //}

    }
}
