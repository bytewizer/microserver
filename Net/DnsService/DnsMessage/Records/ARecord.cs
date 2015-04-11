using System;
using System.Net;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// A Resource Record (RFC 1035 3.4.1)
    /// 
    /// A records cause no additional section processing. The RDATA section of
    /// an A line in a master file is an Internet address expressed as four
    /// decimal numbers separated by dots without any imbedded spaces (e.g.,
    /// "10.2.0.52" or "192.0.5.6").
    /// </summary>
    [Serializable]
    public class ARecord : RecordBase
    {
        private readonly IPAddress _ipAddress;

        #region Public Properties

        /// <summary>
        /// A 32 bit Internet address.
        /// </summary>
        public IPAddress IPAddress
        {
            get { return _ipAddress; }
        }

        #endregion

        internal ARecord(DnsReader br)
        {
            _ipAddress = new IPAddress(br.ReadBytes(4));
        }

        public override string ToString()
        {
            return _ipAddress.ToString();
        }

        internal override byte[] GetBytes()
        {
            DnsWriter bw = new DnsWriter();
            bw.Write(_ipAddress.GetAddressBytes());

            return bw.GetBytes();
        }
    }
}