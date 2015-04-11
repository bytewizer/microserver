using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// An Answer resource record (RFC 1035 4.1)
    /// </summary>
    [Serializable]
    public class Answer : ResourceRecord
    {
        internal Answer() : base() { }
        internal Answer(DnsReader br) : base(br) { }
    }
}