using System;
using System.Net;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    public class EthClickDevice : NetworkDevice, IEthernetDevice
    {
        EthernetNetworkInterfaceSettings IEthernetDevice.Settings 
            => Settings as EthernetNetworkInterfaceSettings;

        public static EthClickDevice Initialize(ChipsetModel model, int slot)
        {
            EthClickDevice device = null;
            if (model == ChipsetModel.Sc20100)
            {
                switch(slot)
                {
                    case 1:
                        device = SC20100Slot1Device();
                        break;
                    case 2:
                        device = SC20100Slot2Device();
                        break;
                }
            }
            if (model == ChipsetModel.Sc20260)
            {
                switch (slot)
                {
                    case 1:
                        device = SC20260Slot1Device();
                        break;
                    case 2:
                        device = SC20260Slot1Device();
                        break;
                }
            }

            if (device == null)
                throw new ArgumentNullException(nameof(slot));

            return device;
        }

        public EthClickDevice(NetworkController networkController, GpioPin resetPin)
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

        private static EthClickDevice SC20100Slot1Device()
        {
            // Create GPIO reset pin
            var resetPin = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PD4);
            resetPin.SetDriveMode(GpioPinDriveMode.Output);

            // Create spi settings
            var spiSettings = new SpiConnectionSettings()
            {
                ChipSelectLine = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PD3),
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
                InterruptPin = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PC5),
                InterruptEdge = GpioPinEdge.FallingEdge,
                InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                ResetPin = resetPin,
                ResetActiveState = GpioPinValue.Low,
                SpiSettings = spiSettings
            };

            // Create network controller
            var controller = NetworkController.FromName(SC20100.NetworkController.Enc28j60);
            controller.SetCommunicationInterfaceSettings(spiNetworkSettings);

            return new EthClickDevice(controller, resetPin); ;
        }
        
        private static EthClickDevice SC20100Slot2Device()
        {
            throw new NotImplementedException();
        }
        
        private static EthClickDevice SC20260Slot1Device()
        {
            throw new NotImplementedException();
        }
        
        private static EthClickDevice SC20260Slot2Device()
        {
            throw new NotImplementedException();
        }
    }
}