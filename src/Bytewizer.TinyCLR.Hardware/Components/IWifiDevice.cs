using System.Net;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    /// <summary>
    /// Wifi device interface.
    /// </summary>
    public interface IWifiDevice : INetworkDevice
    {
        WiFiNetworkInterfaceSettings Settings { get; }
    }
}