using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    internal class PayloadData
    {
        private readonly long _length;
        private readonly byte[] _data;

        public static readonly ulong MaxLength;
        public static readonly PayloadData Empty;

        internal PayloadData(byte[] data)
            : this(data, data.Length)
        {
        }

        static PayloadData()
        {
            MaxLength = long.MaxValue;
            Empty = new PayloadData(new byte[0], 0);
        }

        internal PayloadData(byte[] data, long length)
        {
            _data = data;
            _length = length;
        }

        internal ushort Code
        {
            get
            {
                return _length >= 2
                       ? _data.SubArray(0, 2).ToUInt16(ByteOrder.Big)
                       : (ushort)1005;
            }
        }

        internal long ExtensionDataLength { get; private set; }

        internal bool HasReservedCode
        {
            get
            {
                return _length >= 2 && Code.IsReserved();
            }
        }

        public byte[] ApplicationData
        {
            get
            {
                return ExtensionDataLength > 0
                       ? _data.SubArray(ExtensionDataLength, _length - ExtensionDataLength)
                       : _data;
            }
        }

        public byte[] ExtensionData
        {
            get
            {
                return ExtensionDataLength > 0
                       ? _data.SubArray(0, ExtensionDataLength)
                       : new byte[0];
            }
        }

        public ulong Length
        {
            get
            {
                return (ulong)_length;
            }
        }

        internal void Mask(byte[] key)
        {
            for (long i = 0; i < _length; i++)
                _data[i] = (byte)(_data[i] ^ key[i % 4]);
        }

        public byte[] ToArray()
        {
            return _data;
        }

        public override string ToString()
        {
            return BitConverter.ToString(_data);
        }
    }
}
