using System;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR
{
    public static class Hardware
    {
        /// <summary>
        /// Initialize SC20260D development board onboard ethernet.
        /// </summary>
        internal static void InitializeEthernet()
        {
            var networkController = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings
            {
                MacAddress = new byte[] { 0x00, 0x8D, 0xB4, 0x49, 0xAD, 0xBD }
            };

            networkController.SetInterfaceSettings(networkInterfaceSetting);
            networkController.NetworkAddressChanged += NetworkAddressChanged;
            networkController.SetAsDefaultController();

            networkController.Enable();
        }

        /// <summary>
        /// Initialize Wifi click installed in slot 1 on SC20260D development board.
        /// </summary>
        internal static void InitializeWiFiClick(string ssid, string password)
        {
            var enablePin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PI0);
            enablePin.SetDriveMode(GpioPinDriveMode.Output);
            enablePin.Write(GpioPinValue.High);

            var Controller = NetworkController.FromName(SC20260.NetworkController.ATWinc15x0);

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
            Controller.NetworkAddressChanged += NetworkAddressChanged;

            Controller.Enable();
        }

        /// <summary>
        /// Initialize onboard Wifi installed on FEZ Portal single board computers.
        /// </summary>
        internal static void InitializeWiFi(string ssid, string password)
        {
            var enablePin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PA8);
            enablePin.SetDriveMode(GpioPinDriveMode.Output);
            enablePin.Write(GpioPinValue.High);

            var Controller = NetworkController.FromName(SC20260.NetworkController.ATWinc15x0);

            Controller.SetCommunicationInterfaceSettings(new SpiNetworkCommunicationInterfaceSettings()
            {
                SpiApiName = SC20260.SpiBus.Spi3,
                GpioApiName = SC20260.GpioPin.Id,
                InterruptPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PF10),
                InterruptEdge = GpioPinEdge.FallingEdge,
                InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                ResetPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PC3),
                ResetActiveState = GpioPinValue.Low,
                SpiSettings = new SpiConnectionSettings()
                {
                    ChipSelectLine = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PA6),
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
            Controller.NetworkAddressChanged += NetworkAddressChanged;

            Controller.Enable();
        }

        /// <summary>
        /// Triggers on network address change.
        /// </summary>
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