using System;
using System.Diagnostics;
using System.Net;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR
{
    public class NetworkingDuino
    {
        private static bool linkReady = false;

        public void SetupEthernet()
        {
            // SC20100 development board using ETH Click in slot 1

            var enablePin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PA8);
            enablePin.SetDriveMode(GpioPinDriveMode.Output);
            enablePin.Write(GpioPinValue.High);

            SpiNetworkCommunicationInterfaceSettings netInterfaceSettings =
                new SpiNetworkCommunicationInterfaceSettings();

            var cs = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PD15);

            var settings = new SpiConnectionSettings()
            {
                ChipSelectLine = cs,
                ClockFrequency = 4000000,
                Mode = SpiMode.Mode0,
                ChipSelectType = SpiChipSelectType.Gpio,
                ChipSelectHoldTime = TimeSpan.FromTicks(10),
                ChipSelectSetupTime = TimeSpan.FromTicks(10)
            };

            netInterfaceSettings.SpiApiName = SC20100.SpiBus.Spi3;

            netInterfaceSettings.GpioApiName = SC20100.GpioPin.Id;

            netInterfaceSettings.SpiSettings = settings;
            netInterfaceSettings.InterruptPin = GpioController.GetDefault().
                OpenPin(SC20100.GpioPin.PB12);

            netInterfaceSettings.InterruptEdge = GpioPinEdge.FallingEdge;
            netInterfaceSettings.InterruptDriveMode = GpioPinDriveMode.InputPullUp;
            netInterfaceSettings.ResetPin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PB13);
            netInterfaceSettings.ResetActiveState = GpioPinValue.Low;

            var networkController = NetworkController.FromName
                ("GHIElectronics.TinyCLR.NativeApis.ATWINC15xx.NetworkController");

            WiFiNetworkInterfaceSettings wifiSettings = new WiFiNetworkInterfaceSettings()
            {
                Ssid = "ncc1701",
                Password = "Fun-With-Wireless",
            };

            wifiSettings.Address = new IPAddress(new byte[] { 192, 168, 91, 220 });
            wifiSettings.SubnetMask = new IPAddress(new byte[] { 255, 255, 254, 0 });
            wifiSettings.GatewayAddress = new IPAddress(new byte[] { 192, 168, 90, 1 });
            wifiSettings.DnsAddresses = new IPAddress[] { new IPAddress(new byte[]
        { 192, 168, 90, 14 }) };

            //wifiSettings.MacAddress = new byte[] { 0xFB, 0xF0, 0x05, 0x75, 0x75, 0xFD };
            wifiSettings.IsDhcpEnabled = false;
            wifiSettings.IsDynamicDnsEnabled = false;
            wifiSettings.TlsEntropy = new byte[] { 0, 1, 2, 3 };

            networkController.SetInterfaceSettings(wifiSettings);
            networkController.SetCommunicationInterfaceSettings(netInterfaceSettings);
            networkController.SetAsDefaultController();

            networkController.NetworkAddressChanged += NetworkController_NetworkAddressChanged;

            networkController.NetworkLinkConnectedChanged += NetworkController_NetworkLinkConnectedChanged;

            networkController.Enable();

            while (linkReady == false);

            Debug.WriteLine("Network is Ready");
        }

        private static void NetworkController_NetworkAddressChanged(NetworkController sender, NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                Debug.WriteLine($"Network interface address assinged: {ipProperties.Address}");
                linkReady = address[0] != 0;
            }
        }
        private static void NetworkController_NetworkLinkConnectedChanged
            (NetworkController sender, NetworkLinkConnectedChangedEventArgs e)
        {
            // Raise event connect/disconnect
        }
    }
}