using System;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    public class EthernetDevice : NetworkDevice, INetworkDevice
    {
        public static EthernetDevice Initialize()
        {
            var device = SC20260EthernetDevice();

            if (device == null)
                throw new ArgumentNullException(nameof(device));

            return device;
        }

        public EthernetDevice(NetworkController networkController, GpioPin resetPin)
            : base(networkController, resetPin)
        {
            NetworkSettings.MacAddress = new byte[] { 0x3E, 0x4B, 0x27, 0x21, 0x61, 0x57 };
            Reset();
        }

        private static EthernetDevice SC20260EthernetDevice()
        {
            // Create GPIO reset pin
            var resetPin = HardwareProvider.Gpio.OpenPin(SC20260.GpioPin.PG3);
            resetPin.SetDriveMode(GpioPinDriveMode.Output);

            // Create network controller
            var controller = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);
            var networkSettings = new BuiltInNetworkCommunicationInterfaceSettings();
            controller.SetCommunicationInterfaceSettings(networkSettings);

            return new EthernetDevice(controller, resetPin); ;
        }
    }
}