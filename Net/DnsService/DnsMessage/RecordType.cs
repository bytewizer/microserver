using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// The DNS TYPE fields are used in resource records. (RFC 1035 3.2.2)
    /// </summary>
    public enum RecordType : ushort
    {
        /// <summary>
        /// A host address
        /// </summary>
        A = 1,

        /// <summary>
        /// An authoritative name server
        /// </summary>
        NS = 2,

        /// <summary>
        /// A mail destination (Obsolete - use MX)
        /// </summary>
        [Obsolete("Use DnsType.MX instead.", true)]
        MD = 3,

        /// <summary>
        /// A mail forwarder (Obsolete - use MX)
        /// </summary>
        [Obsolete("Use DnsType.MX instead.", true)]
        MF = 4,

        /// <summary>
        /// The canonical name for an alias
        /// </summary>
        CNAME = 5,

        /// <summary>
        /// Marks the start of a zone of authority
        /// </summary>
        SOA = 6,

        /// <summary>
        /// A mailbox domain name (EXPERIMENTAL)
        /// </summary>
        [Obsolete("DnsType used only experimental.", true)]
        MB = 7,

        /// <summary>
        /// A mail group member (EXPERIMENTAL)
        /// </summary>
        [Obsolete("DnsType used only experimental.", true)]
        MG = 8,

        /// <summary>
        /// a mail rename domain name (EXPERIMENTAL)
        /// </summary>
        [Obsolete("DnsType used only experimental.", true)]
        MR = 9,

        /// <summary>
        /// A null RR (EXPERIMENTAL)
        /// </summary>
        [Obsolete("DnsType used only experimental.", true)]
        NULL = 10,

        /// <summary>
        /// A well known service description
        /// </summary>
        WKS = 11,

        /// <summary>
        /// A domain name pointer
        /// </summary>
        PTR = 12,

        /// <summary>
        /// Host information
        /// </summary>
        HINFO = 13,

        /// <summary>
        /// Mailbox or mail list information
        /// </summary>
        MINFO = 14,

        /// <summary>
        /// Mail exchange
        /// </summary>
        MX = 15,

        /// <summary>
        /// Text strings
        /// </summary>
        TXT = 16,

        /// <summary>
        /// NetBIOS general Name Service Resource Record
        /// </summary>
        NB = 32,

        /// <summary>
        /// NetBIOS NODE STATUS Resource Record
        /// </summary>
        NBSTAT = 33
    }
}
