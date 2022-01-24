using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.Terminal
{
    internal class SecureShellReader : IDisposable
    {
        protected Stream _stream;

        public SecureShellReader(Stream stream)
        {
            _stream = stream;
        }

        public Stream BaseStream
        {
            get { return _stream; }
        }

        public string[] ReadNameList()
        {
            return ReadString().Split(new char[] { ',' });
        }

        public byte[] ReadByteString()
        {
            var strLength = ReadUInt32();

            return ReadBytes(strLength);
        }

        //public int ReadMPInt32()
        //{
        //    return BitConverter.ToInt32(ReadMPInt(), 0);
        //}

        // Important: method assumes value is always positive integer, and therefore strips off a leading 
        // zero if one exists.
        public byte[] ReadMPInt()
        {
            // Read bytes from stream.
            var data = ReadBytes(ReadUInt32());

            // Check if value contains leading zero.
            if (data.Length > 0 && data[0] == 0)
            {
                // Stip off leading zero.
                var output = new byte[data.Length - 1];
                Array.Copy(data, 1, output, 0, output.Length);

                // Return value of integer.
                return output;
            }
            else
            {
                // Return value of integer.
                return data;
            }
        }

        public string ReadString()
        {
            var length = ReadUInt32();

            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        public char ReadChar()
        {
            var num = _stream.ReadByte();

            if (num == -1)
            { 
                throw new Exception("End of stream"); 
            
            }
            return Encoding.UTF8.GetChars(new byte[] { (byte)num })[0];
        }

        public short ReadInt16()
        {
            return unchecked((short)ReadUInt16());
        }

        public int ReadInt32()
        {
            return unchecked((int)ReadUInt32());
        }

        public long ReadInt64()
        {
            return unchecked((long)ReadUInt64());
        }

        public ushort ReadUInt16()
        {
            return (ushort)((ReadByte() << 8) | ReadByte());
        }

        public uint ReadUInt32()
        {
            return (uint)((ReadByte() << 24) | (ReadByte() << 16) | (ReadByte() << 8) | ReadByte());
        }

        public ulong ReadUInt64()
        {
            return (ulong)((ReadByte() << 56) | (ReadByte() << 48) | (ReadByte() << 40) | (ReadByte() << 32) |
                (ReadByte() << 24) | (ReadByte() << 16) | (ReadByte() << 8) | ReadByte());
        }

        public bool ReadBoolean()
        {
            int num = _stream.ReadByte();

            if (num == -1)
            {
                throw new Exception("End of stream");
            }
            return (num != 0);
        }

        public byte[] ReadBytes(uint count)
        {
            byte[] buffer = new byte[count];

            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0 && buffer.Length > 0) throw new Exception("End of stream");

            byte[] returnBuffer = new byte[bytesRead];
            Array.Copy(buffer, 0, returnBuffer, 0, returnBuffer.Length);

            return returnBuffer;
        }

        public byte ReadByte()
        {
            int num = _stream.ReadByte();

            if (num == -1)
            {
                throw new Exception("End of stream");
            }

            return (byte)num;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public virtual void Close()
        {
            this.Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stream stream = _stream;
                _stream = null;
                if (stream != null)
                {
                    stream.Close();
                };
            }

            _stream = null;
        }
    }
}
