using System;

using Bytewizer.TinyCLR.Hardware.Components;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Pins;

namespace Bytewizer.TinyCLR.Hardware.Boards
{
    /// <summary>
    /// FEZ Feather board.
    /// </summary>
    public sealed class FeatherBoard : DisposableObject, IMainboard
    {
        #region Lifetime

        /// <summary>
        /// Creates an instance.
        /// </summary>
        public FeatherBoard()
        {
            try
            {
                // Initialize hardware
                HardwareProvider.Initialize();

                // Initialize components
                Network = WifiDevice.Initialize();
                Led = new LedDevice(SC20100.GpioPin.PE11);

            }
            catch
            {
                // Close devices in case partially initialized
                Network?.Dispose();

                // Continue error
                throw;
            }
        }

        #region IDisposable

        /// <summary>
        /// Frees resources owned by this instance.
        /// </summary>
        /// <param name="disposing">
        /// True when called via <see cref="IDisposable.Dispose()"/>, false when called from the finalizer.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // Only managed resources to dispose
            if (!disposing)
                return;

            // Dispose owned objects
            Network?.Dispose();
        }

        #endregion IDisposable

        #endregion Lifetime

        #region Public Properties

        /// <summary>
        /// Hardware chipset model.
        /// </summary>
        public ChipsetModel Chipset => ChipsetModel.Sc20100;

        /// <summary>
        /// Wifi device.
        /// </summary>
        public INetworkDevice Network { get; }

        /// <summary>
        /// Onboard led device.
        /// </summary>
        public ILedDevice Led { get; }

        #endregion Public Properties

    }
}