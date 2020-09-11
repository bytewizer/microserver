//using GHIElectronics.TinyCLR.Pins;
//using GHIElectronics.TinyCLR.Devices.Gpio;

//using Bytewizer.TinyCLR.Http.Mvc;

//namespace Bytewizer.TinyCLR.WebServer.Controllers
//{    
//    public class ApiController : Controller
//    {
//        private static GpioPin led;

//        public ApiController()
//        {
//            if (led == null)
//            {           
//                var gpioController = GpioController.GetDefault();
//                led = gpioController.OpenPin(SC20100.GpioPin.PE11);
//                led.SetDriveMode(GpioPinDriveMode.Output);
//            }
//        }

//        public IActionResult Toggle()
//        {   
//            led.Write(led.Read() == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High);

//            return Ok();
//        }

//        //public IActionResult Toggle(bool status)
//        //{
//        //    if (status == true)
//        //    {    
//        //        led.Write(GpioPinValue.High);
//        //        return Content("Turn Off", "text/html");
//        //    }

//        //    led.Write(GpioPinValue.Low);
//        //    return Content("Turn On", "text/html");
//        //}
//    }
//}