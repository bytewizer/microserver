using System;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;
using System.Net;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    public class WifiDevice : NetworkDevice, IWifiDevice
    {
        WiFiNetworkInterfaceSettings IWifiDevice.Settings => Settings as WiFiNetworkInterfaceSettings;

        public static WifiDevice Initialize()
        {
            // Create GPIO reset pin
            var resetPin = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PB13);
            resetPin.SetDriveMode(GpioPinDriveMode.Output);

            // Create spi settings
            var spiSettings = new SpiConnectionSettings()
            {
                ChipSelectLine = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PD15),
                ClockFrequency = 4000000,
                Mode = SpiMode.Mode0,
                ChipSelectType = SpiChipSelectType.Gpio,
                ChipSelectHoldTime = TimeSpan.FromTicks(10),
                ChipSelectSetupTime = TimeSpan.FromTicks(10)
            };

            var spiNetworkSettings = new SpiNetworkCommunicationInterfaceSettings()
            {
                SpiApiName = SC20100.SpiBus.Spi3,
                GpioApiName = SC20100.GpioPin.Id,
                InterruptPin = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PB12),
                InterruptEdge = GpioPinEdge.FallingEdge,
                InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                ResetPin = resetPin,
                ResetActiveState = GpioPinValue.Low,
                SpiSettings = spiSettings
            };

            // Create network controller
            var controller = NetworkController.FromName(SC20100.NetworkController.ATWinc15x0);
            controller.SetCommunicationInterfaceSettings(spiNetworkSettings);

            return new WifiDevice(controller, resetPin);
        }

        public WifiDevice(NetworkController networkController, GpioPin resetPin)
            : base(networkController, resetPin)
        {
            Settings = new WiFiNetworkInterfaceSettings
            {
                Address = new IPAddress(new byte[] { 192, 168, 1, 100 }),
                SubnetMask = new IPAddress(new byte[] { 255, 255, 255, 0 }),
                GatewayAddress = new IPAddress(new byte[] { 192, 168, 1, 1 }),
            };
            networkController.SetInterfaceSettings(Settings);

            var enablePin = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PA8);

            enablePin.SetDriveMode(GpioPinDriveMode.Output);
            enablePin.Write(GpioPinValue.High);

            Reset();
        }
    }
}