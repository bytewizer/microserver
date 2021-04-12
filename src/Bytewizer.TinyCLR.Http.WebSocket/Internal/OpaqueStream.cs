using System;
using System.IO;

namespace Bytewizer.TinyCLR.Http.WebSockets.Internal
{
    internal class OpaqueStream : Stream
    {
        private readonly Stream _requestStream;
        private readonly Stream _responseStream;

        internal OpaqueStream(Stream requestStream, Stream responseStream)
        {
            _requestStream = requestStream;
            _responseStream = responseStream;
        }

        #region Properties

        public override bool CanRead
        {
            get { return _requestStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanTimeout
        {
            get { return _requestStream.CanTimeout || _responseStream.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return _responseStream.CanWrite; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int ReadTimeout
        {
            get { return _requestStream.ReadTimeout; }
            set { _requestStream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return _responseStream.WriteTimeout; }
            set { _responseStream.WriteTimeout = value; }
        }

        #endregion Properties

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        #region Read

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _requestStream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return _requestStream.ReadByte();
        }

        #endregion Read

        #region Write

        public override void Write(byte[] buffer, int offset, int count)
        {
            _responseStream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            _responseStream.WriteByte(value);
        }

        public override void Flush()
        {
            _responseStream.Flush();
        }

        #endregion Write

        protected override void Dispose(bool disposing)
        {
            // TODO: Suppress dispose?
            if (disposing)
            {
                _requestStream.Dispose();
                _responseStream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
