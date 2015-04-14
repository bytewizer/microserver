using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// PTR Resource Record (RFC 1035 3.3.12)
    /// </summary>
    [Serializable]
    public class PTRERecord : RecordBase
    {
        private readonly string _domain;

        #region Public Properties

        /// <summary>
        /// A &lt;domain-name&gt; which specifies the canonical or primary
        /// name for the owner. The owner name is an alias.
        /// </summary>
        public string Domain
        {
            get { return _domain; }
        }

        #endregion

        public PTRERecord(DnsReader br)
        {
            _domain = br.ReadDomain();
        }

        public override string ToString()
        {
            return "    name = " + _domain;
        }
    }
}