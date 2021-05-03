using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Internal
{
    internal class ReadOnlyBufferedStream : Stream
    {
        private const int _defaultBufferSize = 4096;

        private readonly Stream _stream;
        private byte[] _buffer;
        
        private int _available = 0;
        private int _pos = 0;

        public ReadOnlyBufferedStream(Stream stream)
        {
            _stream = stream;
            _buffer = new byte[_defaultBufferSize];
        }

        public string ReadLine()
        {
            var line = new StringBuilder();

            // Read until newline.
            int buffer;
            while ((buffer = ReadByte()) != '\n')
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

        public override int Read(byte[] buffer, int offset, int size)
        {
            if (size <= _available)
            {
                Array.Copy(buffer, _pos, buffer, offset, size);
                _available -= size;
                _pos += size;
                return size;
            }
            else
            {
                int readcount = 0;
                if (_available > 0)
                {
                    Array.Copy(buffer, _pos, buffer, offset, _available);
                    offset += _available;
                    readcount += _available;
                    _available = 0;
                    _pos = 0;
                }

                while (true)
                {
                    try
                    {
                        _available = _stream.Read(buffer, 0, buffer.Length);
                        _pos = 0;
                    }
                    catch (Exception ex)
                    {
                        if (readcount > 0)
                        {
                            return readcount;
                        }

                        throw (ex);
                    }
                    if (_available < 1)
                    {
                        if (readcount > 0)
                        {
                            return readcount;
                        }

                        return 0;
                    }
                    else
                    {
                        int toread = size - readcount;
                        if (toread <= _available)
                        {
                            Array.Copy(buffer, _pos, buffer, offset, toread);
                            _available -= toread;
                            _pos += toread;
                            readcount += toread;
                            return readcount;
                        }
                        else
                        {
                            Array.Copy(buffer, _pos, buffer, offset, _available);
                            offset += _available;
                            readcount += _available;
                            _pos = 0;
                            _available = 0;
                        }
                    }
                }
            }
        }

        public override int ReadByte()
        {
            if (_available > 0)
            {
                _available -= 1;
                _pos += 1;
                return _buffer[_pos - 1];
            }
            else
            {
                try
                {
                    _available = _stream.Read(_buffer, 0, _buffer.Length);
                    _pos = 0;
                }
                catch
                {
                    return -1;
                }
                if (_available < 1)
                {
                    return -1;
                }
                else
                {
                    _available -= 1;
                    _pos += 1;
                    return _buffer[_pos - 1];
                }
            }
        }


        public bool EndOfStream
        {
            get
            {
                if (_stream == null)
                {
                    throw new ObjectDisposedException();
                }

                if (_pos < _available)
                {
                    return false;
                }

                return true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_buffer != null)
            {
                _buffer = null;
            }
            _buffer = null;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}