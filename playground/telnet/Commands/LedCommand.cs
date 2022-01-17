using System.Collections;

using Bytewizer.TinyCLR.Terminal;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.Playground.Terminal.Commands
{
    /// <summary>
    /// Implements the <c>led</c> terminal command.
    /// </summary>
    public class LedCommand : ServerCommand
    {
        private readonly StatusLed _led;

        private int _timeOn;
        private int _timeOff;
        private int _interval;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommand"/> class.
        /// </summary>
        public LedCommand()
        {
            StatusProvider.Initialize(SC20260.GpioPin.PH6);

            Description = "Blink an led providing a visual status";
            HelpCommands = new ArrayList()
            {
                { "led" },
                { "led off" },
                { "led on [--interval=0]"},
                { "led blink [--timeon=100] [--timeoff=100] [--interval=0]" }
            };

            _led = StatusProvider.Led;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // called before action method

            _timeOn = filterContext.GetArgumentOrDefault("timeon", 100);
            _timeOff = filterContext.GetArgumentOrDefault("timeoff", 100);
            _interval = filterContext.GetArgumentOrDefault("interval", 0);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // called after action method    
        }

        public override void OnException(ExceptionContext filterContext)
        {
            // called on action method execption
            var actionName = CommandContext.ActionDescriptor.DisplayName;

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ResponseResult($"An error occurred in the {actionName} action.");
        }

        public IActionResult Default()
        {
            if (_led.Active == true)
            {
                return new ResponseResult("Led is blinking");
            }
            else
            {
                if (_led.State == GpioPinValue.High)
                {
                    return new ResponseResult("Led is on");
                }
                else
                {
                    return new ResponseResult("Led is off");
                }
            }
        }

        public IActionResult On()
        {
            if (_interval > 0)
            {
                _led.Blink(_interval, 0, _interval);
            }
            else
            {
                _led.On();
            }

            return new EmptyResult();
        }

        public IActionResult Off()
        {
            _led.Off();
            return new EmptyResult();
        }

        public IActionResult Blink()
        {
            _led.Blink(_timeOn, _timeOff, _interval);
            return new EmptyResult();
        }
    }
}


//sb.AppendLine(" led");
//sb.AppendLine(" led help");
//sb.AppendLine(" led off");
//sb.AppendLine(" led on [--interval=0]");
//sb.AppendLine(" led blink [--timeon=100] [--timeoff=100] [--interval=0]");