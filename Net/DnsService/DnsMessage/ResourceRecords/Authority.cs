using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// An Authority resource record (RFC 1035 4.1)
    /// </summary>
    [Serializable]
    public class Authority : ResourceRecord
    {
        internal Authority() : base() { }
        internal Authority(DnsReader br) : base(br) { }
    }
}