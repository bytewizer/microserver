using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;

using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Stubble;
using Bytewizer.TinyCLR.WebServer.Models;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.TinyCLR.WebServer
{
    public class HomeController : Controller
    {
        //private static GpioPin _led;
        private static ILogger _logger;

        private static LoggerFactory loggingFactory;

        public HomeController()
        {
            //if (_led == null)
            //{
            //    _led = HardwareProvider.Gpio.OpenPin(SC20100.GpioPin.PE11);
            //    _led.SetDriveMode(GpioPinDriveMode.Output);
            //}
            if (loggingFactory == null)
            {
                loggingFactory = new LoggerFactory();

                loggingFactory.AddDebug();
                _logger = loggingFactory.CreateLogger(typeof(HomeController));
            }
        }

        public IActionResult Index()
        {
            _logger.LogInformation(100, "/index action");

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