using System;
using System.Net;

using MicroServer.IO;
using MicroServer.Utilities;

namespace MicroServer.Net.Dns
{
    [Serializable]
    public class NBRecord : RecordBase
    {
        private readonly ushort _flags;
        private readonly IPAddress _ipAddress;

        #region Public Properties

        public bool G
        {
            get { return ByteUtility.GetBit(_flags, 15); }
        }

        public ushort ONT
        {
            get { return ByteUtility.GetBits(_flags, 13, 2); }
        }

        /// <summary>
        /// A 32 bit Internet address.
        /// </summary>
        public IPAddress IPAddress
        {
            get { return _ipAddress; }
        }

        #endregion

        public NBRecord(IPAddress address)
        {
            _ipAddress = address;
        }

        public NBRecord(DnsReader br)
        {
            _flags = br.ReadUInt16();
            _ipAddress = new IPAddress(br.ReadBytes(4));
        }

        public override string ToString()
        {
            return "    " + _ipAddress.ToString();
        }

        public override byte[] GetBytes()
        {
            DnsWriter bw = new DnsWriter();

            bw.Write(_flags);
            bw.Write(_ipAddress.GetAddressBytes());

            return bw.GetBytes();
        }
    }
}