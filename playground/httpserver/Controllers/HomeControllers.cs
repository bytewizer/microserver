using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;

using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.WebServer.Models;

namespace Bytewizer.TinyCLR.WebServer
{
    public class HomeController: Controller
    {
        private static GpioPin led;

        public HomeController()
        {
            if (led == null)
            {
                var gpioController = GpioController.GetDefault();
                led = gpioController.OpenPin(SC20100.GpioPin.PE11);
                led.SetDriveMode(GpioPinDriveMode.Output);
            }
        }

        public IActionResult Index()
        {
            ViewModel["title"] = "Microserver";
            ViewModel["image"] = "<img src='/assets/img/ocean.jpg' class='rounded'>";      
            ViewModel.Show("has-image");
            ViewModel.Child("header")["content"] = "<h3>Shared Header Content</h3>";
            ViewModel.Bind( 
                new PlayerModel ()
                    {
                        TeamId = 288272,
                        PlayerName = "Nazem Kadri",
                        Points = 18,
                        Goals = 10,
                        Assists = 16
                    });

            return View(@"views\Home\index.html");
        }

        public IActionResult Toggle()
        {
            led.Write(led.Read() == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High);

            return Ok();
        }
    }
}
