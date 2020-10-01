using System;
using System.Threading;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR
{
    public static class NetworkProvider
    {
        private static bool _initialized;
        
        private static readonly object _lock = new object();
       
        public static NetworkController Controller { get; private set; }      
        
        public static bool IsLinkReady { get; private set; }
        
        public static bool IsConnected { get; private set; }

        public static void Initialize()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                var _dhcpEvent = new AutoResetEvent(false);

                // Create GPIO reset pin
                var resetPin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PB13);
                resetPin.SetDriveMode(GpioPinDriveMode.Output);

                // Toggle reset pin and wait for reset completion
                resetPin.Write(GpioPinValue.Low);
                Thread.Sleep(100);
                resetPin.Write(GpioPinValue.High);
                Thread.Sleep(100);

                // Create spi settings
                var spiSettings = new SpiConnectionSettings()
                {
                    ChipSelectLine = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PD15),
                    ClockFrequency = 4000000,
                    Mode = SpiMode.Mode0,
                    ChipSelectType = SpiChipSelectType.Gpio,
                    ChipSelectHoldTime = TimeSpan.FromTicks(10),
                    ChipSelectSetupTime = TimeSpan.FromTicks(10)
                };

                // Create network settngs
                var spiNetworkSettings = new SpiNetworkCommunicationInterfaceSettings()
                {
                    SpiApiName = SC20100.SpiBus.Spi3,
                    GpioApiName = SC20100.GpioPin.Id,
                    InterruptPin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PB12),
                    InterruptEdge = GpioPinEdge.FallingEdge,
                    InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                    ResetPin = resetPin,
                    ResetActiveState = GpioPinValue.Low,
                    SpiSettings = spiSettings
                };

                // Create WiFi settings
                var wifiNetworkSettings = new WiFiNetworkInterfaceSettings()
                {
                    Ssid = "crytek",
                    Password = "!therices!",
                };

                // Create network controller
                Controller = NetworkController.FromName(SC20100.NetworkController.ATWinc15x0);
                Controller.SetCommunicationInterfaceSettings(spiNetworkSettings);
                Controller.SetInterfaceSettings(wifiNetworkSettings);
                Controller.SetAsDefaultController();
                Controller.NetworkLinkConnectedChanged += NetworkLinkConnectedChanged;
                Controller.NetworkAddressChanged += NetworkAddressChanged;

                // Create GPIO enable pin
                var enablePin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PA8);
                enablePin.SetDriveMode(GpioPinDriveMode.Output);
                enablePin.Write(GpioPinValue.High);
                
                Controller.Enable();

                _initialized = true;
            }
        }

        private static void NetworkLinkConnectedChanged
            (NetworkController sender, NetworkLinkConnectedChangedEventArgs e)
        {
            IsLinkReady = e.Connected;
        }

        private static void NetworkAddressChanged
            (NetworkController sender, NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();
            IsConnected = address[0] != 0;

            if (IsConnected)
            {
                ListNetworkInfo(sender);
            }
        }

        private static void ListNetworkInfo(NetworkController networkInterface)
        {
            try
            {
                switch (networkInterface.InterfaceType)
                {
                    case (NetworkInterfaceType.Ethernet):
                        Debug.WriteLine(string.Empty);
                        Debug.WriteLine("Found Ethernet Interface");
                        break;
                    case (NetworkInterfaceType.WiFi):
                        Debug.WriteLine(string.Empty);
                        Debug.WriteLine("Found 802.11 WiFi Interface");
                        break;
                    case (NetworkInterfaceType.Ppp):
                        Debug.WriteLine(string.Empty);
                        Debug.WriteLine("Found Point to Point Interface");
                        break;
                }

                var network = networkInterface.GetInterfaceProperties();
                var ip = networkInterface.GetIPProperties();
                var activeSettings = networkInterface.ActiveInterfaceSettings;

                Debug.WriteLine(string.Empty);
                Debug.WriteLine("   Physical Address. . . . . . . . . : " + GetPhysicalAddress(network.MacAddress));
                Debug.WriteLine("   DHCP Enabled. . . . . . . . . . . : " + BoolToYesNo(activeSettings.IsDhcpEnabled));
                Debug.WriteLine("   IPv4 Address. . . . . . . . . . . : " + ip.Address.ToString());
                Debug.WriteLine("   Subnet Mask . . . . . . . . . . . : " + ip.SubnetMask.ToString());
                Debug.WriteLine("   Default Gateway . . . . . . . . . : " + ip.GatewayAddress.ToString());
                Debug.WriteLine(string.Empty);
                for (int i = 0; i < ip.DnsAddresses.Length; i++)
                {
                    Debug.WriteLine("   DNS Servers . . . . . . . . . . . : " + ip.DnsAddresses[i].ToString());
                }
                Debug.WriteLine("   Dynamic DNS Enabled . . . . . . . : " + BoolToYesNo(activeSettings.IsDynamicDnsEnabled));
                Debug.WriteLine(string.Empty);

                if (networkInterface.InterfaceType == NetworkInterfaceType.WiFi)
                {
                    var wifi = (WiFiNetworkInterfaceSettings)networkInterface.ActiveInterfaceSettings;
                    Debug.WriteLine("   SSID  . . . . . . . . . . . . . . : " + wifi.Ssid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ListNetworkInfo exception:  " + e.Message);
            }
        }

        public static string GetPhysicalAddress(byte[] byteArray, char seperator = '-')
        {
            if (byteArray == null)
                return null;

            string physicalAddress = string.Empty;

            for (int i = 0; i < byteArray.Length; i++)
            {
                physicalAddress += byteArray[i].ToString("X2");

                if (i != byteArray.Length - 1)
                    physicalAddress += seperator;
            }

            return physicalAddress;
        }

        public static string BoolToYesNo(bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}