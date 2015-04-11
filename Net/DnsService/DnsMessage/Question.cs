using System;
using System.Text;
using System.Net;
using MicroServer.IO;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// A Question (RFC 1035 4.1.2)
    /// </summary>
    public class Question
    {
        private string _domain;     // QNAME
        private RecordType _qtype;
        private RecordClass _qclass;

        #region Public Properties

        /// <summary>
        /// The domain name to ask for.
        /// </summary>
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        /// <summary>
        /// The type of the query.
        /// </summary>
        public RecordType Type
        {
            get { return _qtype; }
            set { _qtype = value; }
        }

        /// <summary>
        /// The class of the query.
        /// </summary>
        public RecordClass Class
        {
            get { return _qclass; }
            set { _qclass = value; }
        }

        #endregion

        internal Question(DnsReader br)
        {
            _domain = br.ReadDomain();
            _qtype = (RecordType)br.ReadInt16();
            _qclass = (RecordClass)br.ReadInt16();
        }

        public Question()
        {
        }

        public Question(string domain, RecordType qtype, RecordClass qclass)
        {
			if (qtype == RecordType.PTR)
			{
				IPAddress addr = IPAddress.Parse (domain);

				StringBuilder sb = new StringBuilder();

				byte[] addrBytes = addr.GetAddressBytes();
				for(int i=addrBytes.Length -1; i>=0; i--)
					sb.Append((int)addrBytes[i] + ".");

				sb.Append ("in-addr.arpa");

				_domain = sb.ToString ();
			}
            else
                _domain = domain;

            _qtype = qtype;
            _qclass = qclass;
        }

        internal void Write(DnsWriter bw)
        {
            bw.WriteDomain(Domain);
            bw.Write((short)Type);
            bw.Write((short)Class);
        }
    }
}
