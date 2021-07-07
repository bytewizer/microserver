using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Devices.Rtc;

namespace Bytewizer.Playground
{
    public static class ClockProvider
    {
        private static readonly object _lock = new object();

        private static bool _initialized;

        public static RtcController Controller { get; private set; }
      
        public static void Initialize()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                Controller = RtcController.GetDefault();
                Controller.SetChargeMode(BatteryChargeMode.Fast);

                if (Controller.IsValid)
                {
                    SystemTime.SetTime(Controller.Now);
                }

                _initialized = true;
            }
        }
    }
}