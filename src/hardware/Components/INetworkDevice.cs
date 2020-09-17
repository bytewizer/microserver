using System.Net;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    /// <summary>
    /// Ethernet device interface.
    /// </summary>
    public interface INetworkDevice
    {
        bool IsConnected { get; }
        bool IsLinkReady { get; }
        int Timeout { get; set; }
        void Disable();
        void Enabled();
        void Reset();
        void UseDHCP();
        EthernetNetworkInterfaceSettings NetworkSettings { get; }
        void UseStaticIP(IPAddress ip, IPAddress subnet, IPAddress gateway, IPAddress[] dns);
        void UseStaticIP(string ip, string subnet, string gateway, string[] dns);
        void Dispose();
    }
}