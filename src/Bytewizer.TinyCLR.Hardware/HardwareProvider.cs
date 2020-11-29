using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Uart;

namespace Bytewizer.TinyCLR.Hardware
{
    /// <summary>
    /// Hardware configuration and device factory.
    /// </summary>
    public static class HardwareProvider
    {
        #region Private Fields

        /// <summary>
        /// Thread synchronization object.
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Configuration flag.
        /// </summary>
        private static bool _initialized;

        #endregion

        #region Public Properties

        /// <summary>
        /// SPI controller.
        /// </summary>
        public static SpiController Spi { get; private set; }

        /// <summary>
        /// GPIO controller.
        /// </summary>
        public static GpioController Gpio { get; private set; }

        /// <summary>
        /// UART controller.
        /// </summary>
        public static UartController Uart { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Configures the <see cref="HardwareProvider"/>.
        /// </summary>
        /// <remarks>
        /// Not thread safe, must be called in a thread safe context.
        /// </remarks>
        public static void Initialize()
        {
            // Do nothing when already configured
            if (_initialized)
                return;
            
            lock (_lock)
            {
                // Thread-safe double-check lock
                if (_initialized)
                    return;

                //Spi = SpiController.GetDefault();
                Gpio = GpioController.GetDefault();
                //Uart = UartController.GetDefault();

                // Flag initialized
                _initialized = true;
            }
        }

        #endregion
    }
}