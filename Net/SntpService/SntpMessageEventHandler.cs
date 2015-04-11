using System;

namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// Subscribe to this event to receive request messages.
    /// </summary>
    public delegate void SntpMessageEventHandler(object sender, SntpMessageEventArgs args);
}
