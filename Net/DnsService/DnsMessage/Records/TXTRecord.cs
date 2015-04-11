using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// TXT Resource Record (RFC 1035 3.3.14)
    /// </summary>
    public class TXTRecord : RecordBase
    {
        internal string _txtData;

        #region Public Properties

        /// <summary>
        /// One or more &lt;character-string&gt;s.
        /// </summary>
        public string TxtData
        {
            get { return _txtData; }
        }

        #endregion

        internal TXTRecord(DnsReader br)
        {
            _txtData = br.ReadString();
        }

        public override string ToString()
        {
            return "    text = \"" + _txtData + "\"";
        }
    }
}