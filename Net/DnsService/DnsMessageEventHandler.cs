using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// Subscribe to this event to receive request messages.
    /// </summary>
    public delegate void DnsMessageEventHandler(object sender, DnsMessageEventArgs args);
}
