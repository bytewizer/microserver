using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// An Answer resource record (RFC 1035 4.1)
    /// </summary>
    [Serializable]
    public class Answer : ResourceRecord
    {
        public Answer() : base() { }
        public Answer(DnsReader br) : base(br) { }
        public Answer(byte[] bytes) : base(new DnsReader(bytes)) { }
    }
}