using System;

namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// Subscribe to this event to receive the time acquired by the sntp requests.
    /// </summary>
    public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs e);
}
