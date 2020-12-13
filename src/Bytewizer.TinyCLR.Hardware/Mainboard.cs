using System;

namespace Bytewizer.TinyCLR.Hardware
{
    public class Mainboard
    {
        private readonly HardwareOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mainboard"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="Mainboard"/> specific features.</param>
        public Mainboard(HardwareOptionsDelegate configure)
        {
            var options = new HardwareOptions();

            configure(options);
            
            _options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mainboard"/> class.
        /// </summary>
        /// <param name="options">The configuration options of <see cref="Mainboard"/> specific features.</param>
        public Mainboard(HardwareOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        public IHardware Connect()
        {
            if (_options.BoardModel == BoardModel.Unidentified)
            {
                throw new ArgumentException("Missing options.BoardModel implementation.");
            }

            var device = DeviceProvider.Connect(_options.BoardModel);

            if (device.Chipset == ChipsetModel.Unidentified)
            {
                throw new ArgumentException("Hardware failed to connect.");
            }

            return device;
        }
    }
}