using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Internal
{
    internal class ChannelReader : IDisposable
    {
        internal static int DefaultBufferSize = 1024;
        private const int MinBufferSize = 128;

        private Stream _stream;
        private Encoding _encoding;
        private Decoder _decoder;
        private byte[] _byteBuffer;
        private char[] _charBuffer;
        private int _charPos;
        private int _charLen;

        private int _arrayPos;
        private int _arrayLen;

        private int _byteLen;
        private bool _isBlocked;
        private bool _closable;

        internal ChannelReader() { }

        public ChannelReader(Stream stream)
            : this(stream, DefaultBufferSize, false)
        {
        }

        public ChannelReader(Stream stream,  int bufferSize, bool leaveOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream Not Readable");
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            Init(stream, bufferSize, leaveOpen);
        }

        private void Init(Stream stream, int bufferSize, bool leaveOpen)
        {
            _stream = stream;
            _encoding = Encoding.UTF8;
            _decoder = Encoding.UTF8.GetDecoder();
            
            if (bufferSize < MinBufferSize)
            {
                bufferSize = MinBufferSize;
            }

            _byteBuffer = new byte[bufferSize];
            _charBuffer = new char[1024];
            _byteLen = 0;
            _isBlocked = false;
            _closable = !leaveOpen;
        }

        internal void Init(Stream Stream)
        {
            _stream = Stream;
            _closable = true;
        }

        public void Close()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            try
            {
                if (!LeaveOpen && disposing && (_stream != null))
                    _stream.Close();
            }
            finally
            {
                if (!LeaveOpen && _stream != null)
                {
                    _stream = null;
                    _encoding = null;
                    _decoder = null;
                    _byteBuffer = null;
                    _charBuffer = null;
                    _charPos = 0;
                    _charLen = 0;
                }
            }
        }

        public virtual Encoding CurrentEncoding
        {
            get { return _encoding; }
        }

        public virtual Stream BaseStream
        {
            get { return _stream; }
        }

        internal bool LeaveOpen
        {
            get { return !_closable; }
        }

        public void DiscardBufferedData()
        {
            _byteLen = 0;
            _charLen = 0;
            _charPos = 0;

            if (_encoding != null)
            {
                _decoder = _encoding.GetDecoder();
            }
            _isBlocked = false;
        }

        public bool EndOfStream
        {
            get
            {
                if (_stream == null)
                    throw new ObjectDisposedException();

                if (_charPos < _charLen)
                    return false;

                var numRead = ReadBuffer();

                return numRead == 0;
            }
        }

        public int Peek()
        {
            if (_stream == null)
            {
                throw new ObjectDisposedException();
            }

            if (_charPos == _charLen)
            {
                if (_isBlocked || ReadBuffer() == 0) return -1;
            }
            
            return _charBuffer[_charPos];
        }

        public int Read()
        {
            if (_stream == null)
            {
                throw new ObjectDisposedException();
            }
            if (_charPos == _charLen)
            {
                if (ReadBuffer() == 0) return -1;
            }
            
            int result = _charBuffer[_charPos];
            _charPos++;
            
            return result;
        }

        internal virtual int ReadBuffer()
        {
            _charLen = 0;
            _charPos = 0;
            
            do
            {
                _byteLen = _stream.Read(_byteBuffer, 0, _byteBuffer.Length);

                if (_byteLen == 0)
                    return _charLen;

                _isBlocked = (_byteLen < _byteBuffer.Length);

                _decoder.Convert(_byteBuffer, 0, _byteLen, _charBuffer, _charLen, _charBuffer.Length, false, out int byteUsed, out int charUsed, out bool completed);
                _charLen += byteUsed; 

            } while (_charLen == 0);

            return _charLen;
        }

        public string ReadLine()
        {
            if (_stream == null)
                throw new ObjectDisposedException();

            if (_charPos == _charLen)
            {
                if (ReadBuffer() == 0)
                    return null;
            }

            StringBuilder sb = null;
            do
            {
                var i = _charPos;
                do
                {
                    var ch = _charBuffer[i];

                    if (ch == '\r' || ch == '\n')
                    {
                        string s;
                        if (sb != null)
                        {
                            sb.Append(_charBuffer, _charPos, i - _charPos);
                            s = sb.ToString();
                        }
                        else
                        {
                            s = new string(_charBuffer, _charPos, i - _charPos);
                        }
                        _charPos = i + 1;
                        if (ch == '\r' && (_charPos < _charLen || ReadBuffer() > 0))
                        {
                            if (_charBuffer[_charPos] == '\n') _charPos++;
                        }
                        return s;
                    }
                    i++;
                } while (i < _charLen);

                i = _charLen - _charPos;
                if (sb == null)
                    sb = new StringBuilder(i + 80);

                sb.Append(_charBuffer, _charPos, i);

            } while (ReadBuffer() > 0);

            return sb.ToString();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
