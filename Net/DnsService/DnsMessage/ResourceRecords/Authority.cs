using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// An Authority resource record (RFC 1035 4.1)
    /// </summary>
    [Serializable]
    public class Authority : ResourceRecord
    {
        public Authority() : base() { }
        public Authority(DnsReader br) : base(br) { }
    }
}