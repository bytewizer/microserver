using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.HttpServer
{
    class Program
    {
        static void Main()
        {
            // Initialize SC2026D development board ethernet
            InitializeEthernet();

            var led = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PH11);
            led.SetDriveMode(GpioPinDriveMode.Output);

            var server = new HttpServer();
            server.Start();

            while (true)
            {
                led.Write(GpioPinValue.High);
                Thread.Sleep(100);

                led.Write(GpioPinValue.Low);
                Thread.Sleep(100);
            }
        }

        private static void InitializeEthernet()
        {
            var networkController = NetworkController.FromName(
                SC20260.NetworkController.EthernetEmac);

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings
            {
                MacAddress = new byte[] { 0x00, 0x8D, 0xA4, 0x49, 0xCD, 0xBD },
                DhcpEnable = true,
                DynamicDnsEnable = true
            };

            networkController.SetInterfaceSettings(networkInterfaceSetting);
            networkController.NetworkAddressChanged += NetworkAddressChanged;
            networkController.SetAsDefaultController();

            networkController.Enable();
        }

        private static void NetworkAddressChanged(
            NetworkController sender,
            NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                Debug.WriteLine($"Lauch web brower on: http://{ipProperties.Address}");
            }
        }
    }
}