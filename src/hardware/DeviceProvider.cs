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
        public static ChipsetModel Detect()
        {
            // Return existing board model when present
            if (_board != null)
                return _board.Chipset;

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
                            return ChipsetModel.Sc20260;
                        }
                        if (deviceName == "SC20100")
                        {
                            return ChipsetModel.Sc20100;
                        }
                    }

                    // Unsupported device
                    return ChipsetModel.Unidentified;
                }
                catch
                {
                    // No hardware found
                    return ChipsetModel.Unidentified;
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
        public static IMainboard Connect(BoardModel model)
        {
            // Thread-safe lock
            lock (_lock)
            {
                // Check existing board when present
                if (_board != null)
                {
                    // Detect chipset
                    var chipset = Detect();
                    if (chipset == ChipsetModel.Unidentified)
                        return null;

                    // Return existing board when chipset mataches
                    if (_board.Chipset == chipset)
                        return _board;

                    // Dispose existing board when not null and different model (just in case)
                    _board.Dispose();
                    _board = null;
                }

                // Create new board
                switch (model)
                {
                    case BoardModel.Sc20260D:
                        return _board = new SC20260Board();

                    case BoardModel.Sc20100S:
                        return _board = new SC20100Board();

                    case BoardModel.Bit:
                        throw new NotImplementedException();

                    case BoardModel.Duino:
                        return _board = new DuinoBoard();

                    case BoardModel.Feather:
                        return _board = new FeatherBoard();

                    case BoardModel.Portal:
                        throw new NotImplementedException();
                    
                    case BoardModel.Stick:
                        throw new NotImplementedException();

                    default:
                        throw new ArgumentOutOfRangeException(nameof(model));
                }
            }
        }
        #endregion
    }
}
