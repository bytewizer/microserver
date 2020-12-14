using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;

using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Toggler
{    
    public class ApiController : Controller
    {
        private static GpioPin led;

        public ApiController()
        {
            if (led == null)
            {
                led = HardwareProvider.Gpio.OpenPin(SC20260.GpioPin.PH11);
                led.SetDriveMode(GpioPinDriveMode.Output);
            }
        }

        public IActionResult GetOK()
        {
            return Ok();
        }

        public IActionResult Toggle(bool status)
        {
            if (status == true)
            {
                led.Write(GpioPinValue.High);

                return Content("Turn Off");
            }

            led.Write(GpioPinValue.Low);
            return Content("Turn On");
        }
    }
}