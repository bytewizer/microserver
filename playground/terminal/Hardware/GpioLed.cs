using System;
using System.Threading;

using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.Playground.Terminal
{
    public abstract class GpioLed : IDisposable
    {
        private int _timeOn;
        private int _timeOff;

        private Thread _thread;
        private readonly GpioPin _ledPin;

        protected GpioLed(int pinNumber)
        {
            var gpio = GpioController.GetDefault();

            _ledPin = gpio.OpenPin(pinNumber);
            _ledPin.SetDriveMode(GpioPinDriveMode.Output);
            _ledPin.Write(GpioPinValue.Low);
        }

        public void Dispose()
        {
            _ledPin.Dispose();
        }

        public bool Active { get; private set; } = false;

        public GpioPinValue State
        {
            get { return _ledPin.Read(); }
            set { _ledPin.Write(value); }
        }

        public void On()
        {
            _ledPin.Write(GpioPinValue.High);
        }

        public void Off()
        {
            StopBlinking();

            _ledPin.Write(GpioPinValue.Low);
        }

        public void Blink(int timeOn, int timeOff, int interval)
        {
            if (interval > 0)
            {
                Timer timer = new Timer(sender =>
                {
                    StopBlinking();
                }, null, interval, 0);
            }

            Blink(timeOn, timeOff);
        }

        public void Blink(int timeOn, int timeOff)
        {
            _timeOff = timeOff;
            _timeOn = timeOn;

            if (Active)
            {
                return;
            }

            Active = true;

            _thread = new Thread(() =>
            {
                while (Active)
                {
                    var ret = _ledPin.Read();
                    if (ret == GpioPinValue.High)
                    {
                        _ledPin.Write(GpioPinValue.Low);
                        Thread.Sleep(_timeOff);
                    }
                    else
                    {
                        _ledPin.Write(GpioPinValue.High);
                        Thread.Sleep(_timeOn);
                    }
                }
            });

            _thread.Start();
        }

        private void StopBlinking()
        {
            if (!Active)
            {
                return;
            }

            Active = false;

            if (_thread != null)
            {
                _thread = null;
            }

            _ledPin.Write(GpioPinValue.Low);
        }
    }
}