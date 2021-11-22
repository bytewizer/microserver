using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.Ftp.Internal
{
    internal class ControlConnection
    {
        private readonly Stream _stream;
        private readonly Decoder _decoder;

        private readonly byte[] _readByteBuffer = new byte[12];
        private readonly char[] _readCharBuffer = new char[12];
        private int _readOffset = 0;
   
        public ControlConnection()
        {
            _decoder = Encoding.UTF8.GetDecoder();
        }

        /// <summary>
        /// Reads a line from network stream partitioned by CRLF.
        /// </summary>
        /// <returns>The line read with CRLF trimmed.</returns>
        private string ReadLine()
        {   
            StringBuilder messageBuilder = new StringBuilder();

            bool lastByteIsCr = false;
            while (true)
            {
                var byteCount = _stream.Read(_readByteBuffer, _readOffset, _readByteBuffer.Length - _readOffset);
                if (byteCount == 0)
                {
                    throw new Exception();
                }

                for (int i = _readOffset; i < _readOffset + byteCount; i++)
                {
                    // If meets CRLF, stop
                    if (lastByteIsCr && _readByteBuffer[i] == '\n')
                    {
                        var byteCountToRead = i + 1 - _readOffset;
                        while (byteCountToRead > 0)
                        {
                            _decoder.Convert(
                                _readByteBuffer,
                                _readOffset,
                                byteCountToRead,
                                _readCharBuffer,
                                0,
                                _readCharBuffer.Length,
                                true,
                                out int bytesUsed,
                                out int charsUsed,
                                out bool completed);
                            messageBuilder.Append(_readCharBuffer, 0, charsUsed);
                            byteCountToRead -= bytesUsed;
                        }

                        messageBuilder.Remove(messageBuilder.Length - 2, 2);
                        return messageBuilder.ToString();
                    }
                    else
                    {
                        lastByteIsCr = _readByteBuffer[i] == '\r';
                    }
                }

                while (byteCount > 0)
                {
                    _decoder.Convert(
                        _readByteBuffer,
                        _readOffset,
                        byteCount,
                        _readCharBuffer,
                        0,
                        _readCharBuffer.Length,
                        false,
                        out int bytesUsed,
                        out int charsUsed,
                        out bool completed);
                    byteCount -= bytesUsed;
                    messageBuilder.Append(_readCharBuffer, 0, charsUsed);
                }

                _readOffset = 0;
            }
        }
    }
}
