using System;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR
{
    public static class NetworkProvider
    {
        private static readonly object _lock = new object();

        private static bool _initialized;

        public static NetworkController Controller { get; private set; }
        
        public static bool IsLinkReady { get; private set; }
        
        public static bool IsConnected { get; private set; }

        public static void InitializeEthernet()
        {

            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                var Controller = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

                Controller.SetInterfaceSettings(new EthernetNetworkInterfaceSettings()
                {
                    MacAddress = new byte[] { 0x00, 0x2D, 0xB4, 0x49, 0xCD, 0xBD }
                });

                Controller.SetAsDefaultController();
                Controller.NetworkLinkConnectedChanged += NetworkLinkConnectedChanged;
                Controller.NetworkAddressChanged += NetworkAddressChanged;
                
                Controller.Enable();

                _initialized = true;

            }
        }

        public static void InitializeWiFi(string ssid, string password)
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                var enablePin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PI0);
                enablePin.SetDriveMode(GpioPinDriveMode.Output);
                enablePin.Write(GpioPinValue.High);

                Controller = NetworkController.FromName(SC20260.NetworkController.ATWinc15x0);

                Controller.SetCommunicationInterfaceSettings(new SpiNetworkCommunicationInterfaceSettings()
                {
                    SpiApiName = SC20260.SpiBus.Spi3,
                    GpioApiName = SC20260.GpioPin.Id,
                    InterruptPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PG6),
                    InterruptEdge = GpioPinEdge.FallingEdge,
                    InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                    ResetPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PI8),
                    ResetActiveState = GpioPinValue.Low,
                    SpiSettings = new SpiConnectionSettings()
                    {
                        ChipSelectLine = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PG12),
                        ClockFrequency = 4000000,
                        Mode = SpiMode.Mode0,
                        ChipSelectType = SpiChipSelectType.Gpio,
                        ChipSelectHoldTime = TimeSpan.FromTicks(10),
                        ChipSelectSetupTime = TimeSpan.FromTicks(10)
                    }
                });

                Controller.SetInterfaceSettings(new WiFiNetworkInterfaceSettings()
                {
                    Ssid = ssid,
                    Password = password,
                });

                Controller.SetAsDefaultController();
                Controller.NetworkLinkConnectedChanged += NetworkLinkConnectedChanged;
                Controller.NetworkAddressChanged += NetworkAddressChanged;

                Controller.Enable();

                _initialized = true;
            }
        }

        private static void NetworkLinkConnectedChanged(NetworkController sender, NetworkLinkConnectedChangedEventArgs e)
        {
            IsLinkReady = e.Connected;
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