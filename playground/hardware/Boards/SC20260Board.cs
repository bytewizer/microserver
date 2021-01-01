using System;

using Bytewizer.TinyCLR.Hardware.Components;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.TinyCLR.Hardware.Boards
{
    /// <summary>
    /// SC20260D development board.
    /// </summary>
    public sealed class SC20260Board : DisposableObject, IHardware
    {
     
        #region Lifetime

        /// <summary>
        /// Creates an instance.
        /// </summary>
        public SC20260Board()
        {
            try
            {
                // Initialize hardware
                HardwareProvider.Initialize();

                // Initialize components
                Network = EthernetDevice.Initialize();
                Storage = CardDevice.Connect(SC20260.StorageController.SdCard);
                Led = new LedDevice(SC20260.GpioPin.PH11);
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

        #endregion

        #region Public Properties

        /// <summary>
        /// Hardware chipset model.
        /// </summary>
        public ChipsetModel Chipset => ChipsetModel.Sc20260;

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