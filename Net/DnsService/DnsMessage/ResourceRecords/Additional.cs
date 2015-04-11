using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// An Additional resource record (RFC 1035 4.1)
    /// </summary>
    [Serializable]
    public class Additional : ResourceRecord
    {
        internal Additional() : base() { }
        internal Additional(DnsReader br) : base(br) { }
    }
}