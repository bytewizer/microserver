using System;

/// <summary>
/// Subscribe to this event to receive request messages.
/// </summary>
namespace MicroServer.Net.Dhcp
{
    public delegate void DhcpAddressLeaseEventHandler(object sender, DhcpLeaseEventArgs args); 
}
