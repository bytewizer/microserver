using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    public class LedDevice : ILedDevice
    {
        private readonly GpioPin _led;

        public LedDevice(int pinNumber)
        {
            _led = HardwareProvider.Gpio.OpenPin(pinNumber);
            _led.SetDriveMode(GpioPinDriveMode.Output);
        }

        /// <summary>
        /// Toggles the state of the onboard LED.
        /// </summary>
        public void Toggle()
        {
            _led.Write(_led.Read() == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High);
        }

        /// <summary>
        /// Sets the state of the onboard LED on.
        /// </summary>
        public void On()
        {
            _led.Write(GpioPinValue.Low);
        }

        /// <summary>
        /// Sets the state of the onboard LED off.
        /// </summary>
        public void Off()
        {
            _led.Write(GpioPinValue.High);
        }
    }
}