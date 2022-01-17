namespace Bytewizer.Playground.Terminal
{
    public static class StatusProvider
    {
        private static readonly object _lock = new object();

        private static bool _initialized;

        public static StatusLed Led { get; private set; }

        public static void Initialize(int pinNumber)
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                Led = new StatusLed(pinNumber);
            }

            _initialized = true;
        }
    }
}