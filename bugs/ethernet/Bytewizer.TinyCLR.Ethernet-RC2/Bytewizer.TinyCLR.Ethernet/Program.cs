using System.Net;
using System.Threading;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;
using GHIElectronics.TinyCLR.Pins;

namespace Bytewizer.TinyCLR.Ethernet
{
    class Program
    {
        public static NetworkController Controller { get; private set; }

        static void Main()
        {
            var gpioController = GpioController.GetDefault();
            var resetPin = gpioController.OpenPin(SC20260.GpioPin.PG3);

            resetPin.SetDriveMode(GpioPinDriveMode.Output);

            resetPin.Write(GpioPinValue.Low);
            Thread.Sleep(100);

            resetPin.Write(GpioPinValue.High);
            Thread.Sleep(100);

            Controller = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings
            {
                MacAddress = new byte[] { 0x00, 0x8D, 0xB4, 0x49, 0xAD, 0xBD },

                DhcpEnable = true,
                DynamicDnsEnable = true,
                Address = new IPAddress(new byte[] { 192, 168, 1, 200 }),
                SubnetMask = new IPAddress(new byte[] { 255, 255, 255, 0 }),
                GatewayAddress = new IPAddress(new byte[] { 192, 168, 1, 1 }),
                DnsAddresses = new IPAddress[]{
                    new IPAddress(new byte[] { 8, 8, 8, 8 }),
                    new IPAddress(new byte[] { 8, 8, 4, 4 })
                    }
            };

            Controller.SetInterfaceSettings(networkInterfaceSetting);
            Controller.SetAsDefaultController();
            Controller.NetworkAddressChanged += NetworkAddressChanged;

            Controller.Enable();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void NetworkAddressChanged(
           NetworkController sender,
           NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                Debug.WriteLine(ipProperties.Address.ToString());
            }
        }
    }
}
