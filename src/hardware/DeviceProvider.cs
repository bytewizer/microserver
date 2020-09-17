using System;

using GHIElectronics.TinyCLR.Native;

using Bytewizer.TinyCLR.Hardware.Boards;

namespace Bytewizer.TinyCLR.Hardware
{
    /// <summary>
    /// Hardware device provider for <see cref="IMainboard"/> compatible boards.
    /// </summary>
    /// <remarks>
    /// Starting point from which consumers can gain access to all hardware devices
    /// without worrying about hardware detection or initialization.
    /// </remarks>
    public static class DeviceProvider
    {
        #region Singletons

        /// <summary>
        /// Thread synchronization.
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Currently active board.
        /// </summary>
        private static IMainboard _board;

        #endregion

        #region Hardware Detection

        /// <summary>
        /// Attempts auto-detection of the currently installed Development board.
        /// </summary>
        /// <returns>
        /// Development board hardware model when detected, or null when failed.
        /// </returns>
        public static HardwareModel Detect()
        {
            // Return existing board model when present
            if (_board != null)
                return _board.Model;

            // Thread-safe lock
            lock (_lock)
            {
                // Try to detect device information
                try
                {
                    // Read device name
                    var deviceName = DeviceInformation.DeviceName;
                    if (deviceName != null)
                    {
                        // Return  model for known device names
                        if (deviceName == "SC20260")
                        {
                            // Must be a SCM20260D development board
                            return HardwareModel.Sc20260;
                        }
                        if (deviceName == "SC20100")
                        {
                            // Must be a SC20260D development board
                            return HardwareModel.Sc20100;
                        }
                    }

                    // Unsupported device
                    return HardwareModel.Unidentified;
                }
                catch
                {
                    // No hardware found
                    return HardwareModel.Unidentified;
                }
            }
        }

        #endregion

        #region Factory

        /// <summary>
        /// Returns the current <see cref="IMainboard"/> or creates it the first time.
        /// </summary>
        /// <param name="model">Hardware model.</param>
        /// <returns>Hardware interface for the requested model when successful.</returns>
        /// <remarks>
        /// The requested hardware model must be the same, otherwise any existing board
        /// is disposed and an attempt made to create a board of the new model.
        /// </remarks>
        public static IMainboard Connect(HardwareModel model)
        {
            // Thread-safe lock
            lock (_lock)
            {
                // Check existing board when present
                if (_board != null)
                {
                    // Return existing board when present
                    if (_board.Model == model)
                        return _board;

                    // Dispose existing board when not null and different model (just in case)
                    _board.Dispose();
                    _board = null;
                }

                // Create new board
                switch (model)
                {
                    case HardwareModel.Sc20100:
                        return _board = new SC20100Board();

                    case HardwareModel.Sc20260:
                        return _board = new SC20260Board();

                    default:
                        // Invalid value or unsupported
                        throw new ArgumentOutOfRangeException(nameof(model));
                }
            }
        }

        /// <summary>
        /// Returns the current <see cref="IMainboard"/> or performs hardware detection then creates it the first time.
        /// </summary>
        /// <returns>Hardware interface for the detected model when successful or null when none found.</returns>
        public static IMainboard Connect()
        {
            // Thread-safe lock
            lock (_lock)
            {
                // Return existing board when present
                if (_board != null)
                    return _board;

                // Detect model
                var model = Detect();
                if (model == HardwareModel.Unidentified)
                    return null;

                // Create and return interface
                return Connect(model);
            }
        }

        #endregion
    }
}
