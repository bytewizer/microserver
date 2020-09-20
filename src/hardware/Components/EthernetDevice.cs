using System;
using System.Net;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    public class EthernetDevice : NetworkDevice, IEthernetDevice
    {
        EthernetNetworkInterfaceSettings IEthernetDevice.Settings 
            => Settings as EthernetNetworkInterfaceSettings;

        public static EthernetDevice Initialize()
        {
            // Create GPIO reset pin
            var resetPin = HardwareProvider.Gpio.OpenPin(SC20260.GpioPin.PG3);
            resetPin.SetDriveMode(GpioPinDriveMode.Output);

            // Create network controller
            var controller = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);
            var networkSettings = new BuiltInNetworkCommunicationInterfaceSettings();
            controller.SetCommunicationInterfaceSettings(networkSettings);

            return new EthernetDevice(controller, resetPin);
        }

        public EthernetDevice(NetworkController networkController, GpioPin resetPin)
            : base(networkController, resetPin)
        {
            Settings = new EthernetNetworkInterfaceSettings
            {
                Address = new IPAddress(new byte[] { 192, 168, 1, 100 }),
                SubnetMask = new IPAddress(new byte[] { 255, 255, 255, 0 }),
                GatewayAddress = new IPAddress(new byte[] { 192, 168, 1, 1 }),
                MacAddress = new byte[] { 0x3E, 0x4B, 0x27, 0x21, 0x61, 0x57 }
            };

            networkController.SetInterfaceSettings(Settings);
            
            Reset();
        }
    }
}