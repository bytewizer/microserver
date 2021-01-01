using System;

using GHIElectronics.TinyCLR.Pins;
using Bytewizer.TinyCLR.Hardware.Components;
using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.TinyCLR.Hardware.Boards
{
    /// <summary>
    /// SC20260D development board.
    /// </summary>
    public sealed class SC20100Board : DisposableObject, IHardware
    {

        #region Lifetime

        /// <summary>
        /// Creates an instance.
        /// </summary>
        public SC20100Board()
        {
            try
            {
                // Initialize hardware
                HardwareProvider.Initialize();

                // Initialize components
                Network = EthClickDevice.Initialize(ChipsetModel.Sc20100, 1);
                Storage = CardDevice.Connect(SC20100.StorageController.SdCard);
                Led = new LedDevice(SC20100.GpioPin.PE11);

            }
            catch
            {
                // Close devices in case partially initialized
                Network?.Dispose();
                Storage?.Dispose();

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
            Storage?.Dispose();
        }

        #endregion IDisposable

        #endregion Lifetime

        #region Public Properties

        /// <summary>
        /// Hardware chipset model.
        /// </summary>
        public ChipsetModel Chipset => ChipsetModel.Sc20100;

        /// <summary>
        /// Ethernet device.
        /// </summary>
        public INetworkDevice Network { get; }

        /// <summary>
        /// Storage device.
        /// </summary>
        public IStorageDevice Storage { get; }

        /// <summary>
        /// Onboard led device.
        /// </summary>
        public ILedDevice Led { get; }

        #endregion Public Properties

    }
}