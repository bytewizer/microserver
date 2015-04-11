using System;

/// <summary>
/// Subscribe to this event to receive request messages.
/// </summary>
namespace MicroServer.Net.Dhcp
{
    public delegate void DhcpMessageEventHandler(object sender, DhcpMessageEventArgs args);
}
