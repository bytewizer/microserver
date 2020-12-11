using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Stubble;
using Bytewizer.Playground.Mvc.Models;

namespace Bytewizer.Playground.Mvc
{
    public class HomeController : Controller
    {
        //private static GpioPin _led;

        public HomeController()
        {
            //if (_led == null)
            //{
            //    _led = HardwareProvider.Gpio.OpenPin(SC20260.GpioPin.PE11);
            //    _led.SetDriveMode(GpioPinDriveMode.Output);
            //}
        }
        public IActionResult Index()
        {
            ViewData["title"] = "Microserver";
            ViewData["image"] = "<img src='/assets/img/ocean.jpg' class='rounded'>";
            ViewData.Show("has-image");
            ViewData.Child("header")["content"] = "<h3>Shared Header Content</h3>";
            ViewData.Bind(
                new PlayerModel()
                {
                    TeamId = 288272,
                    PlayerName = "Nazem Kadri",
                    Points = 18,
                    Goals = 10,
                    Assists = 16
                });

            return View(@"views\home\index.html");
        }

        public IActionResult Toggle()
        {
            Program.MainBoard.Led.Toggle();
            
            return Ok();
        }
    }
}