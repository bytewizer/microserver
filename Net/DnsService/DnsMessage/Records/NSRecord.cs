using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// NS (Name Server) Resource Record (RFC 1035 3.3.11)
    /// </summary>
    [Serializable]
    public class NSRecord : RecordBase
    {
        private readonly string _domainName;

        #region Public Properties

        /// <summary>
        /// A &lt;domain-name&gt; which specifies a host which should be 
        /// authoritative for the specified class and domain.
        /// </summary>
        public string DomainName
        {
            get { return _domainName; }
        }

        #endregion

        public NSRecord(DnsReader br)
        {
            _domainName = br.ReadDomain();
        }

        public override string ToString()
        {
            return "    nameserver = " + _domainName;
        }
    }
}