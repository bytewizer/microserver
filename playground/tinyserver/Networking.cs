using System;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;
using System.Net;

namespace Bytewizer.TinyCLR
{
    public class Networking
    {
        private static bool linkReady = false;

        public static void SetupEthernet()
        {
            // SC20100 development board using ETH Click in slot 1
            
            var networkController = NetworkController.FromName
                   ("GHIElectronics.TinyCLR.NativeApis.ENC28J60.NetworkController");

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings();
            var networkCommunicationInterfaceSettings = new SpiNetworkCommunicationInterfaceSettings();

            var settings = new SpiConnectionSettings()
            {
                ChipSelectLine = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PD3),
                ClockFrequency = 4000000,
                Mode = SpiMode.Mode0,
                ChipSelectType = SpiChipSelectType.Gpio,
                ChipSelectHoldTime = TimeSpan.FromTicks(10),
                ChipSelectSetupTime = TimeSpan.FromTicks(10)
            };

            networkCommunicationInterfaceSettings.SpiApiName = SC20100.SpiBus.Spi3;
            networkCommunicationInterfaceSettings.GpioApiName = SC20100.GpioPin.Id;
            networkCommunicationInterfaceSettings.SpiSettings = settings;
            networkCommunicationInterfaceSettings.InterruptPin = 
                GpioController.GetDefault().OpenPin(SC20100.GpioPin.PC5);

            networkCommunicationInterfaceSettings.InterruptEdge = GpioPinEdge.FallingEdge;
            networkCommunicationInterfaceSettings.InterruptDriveMode = GpioPinDriveMode.InputPullUp;
            networkCommunicationInterfaceSettings.ResetPin =
                GpioController.GetDefault().OpenPin(SC20100.GpioPin.PD4);

            networkCommunicationInterfaceSettings.ResetActiveState = GpioPinValue.Low;

            //networkInterfaceSetting.IsDhcpEnabled = false;
            //networkInterfaceSetting.IsDynamicDnsEnabled = false;
            //networkInterfaceSetting.Address = new IPAddress(new byte[] { 172, 20, 10, 52 });
            //networkInterfaceSetting.SubnetMask = new IPAddress(new byte[] { 255, 255, 255, 0 });
            //networkInterfaceSetting.GatewayAddress = new IPAddress(new byte[] { 172, 20, 10, 1 });
            //networkInterfaceSetting.DnsAddresses = new IPAddress[] { 
            //    new IPAddress(new byte[] { 8, 8, 8, 8 }),
            //    new IPAddress(new byte[] { 4, 4, 4, 4 }) 
            //};
            
            networkInterfaceSetting.MacAddress = new byte[] { 0x3E, 0x4B, 0x27, 0x21, 0x61, 0x57 };

            networkController.SetInterfaceSettings(networkInterfaceSetting);
            networkController.SetCommunicationInterfaceSettings(networkCommunicationInterfaceSettings);
            networkController.SetAsDefaultController();
            networkController.NetworkAddressChanged += NetworkController_NetworkAddressChanged;
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
    }
}