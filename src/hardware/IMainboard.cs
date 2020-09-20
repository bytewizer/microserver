using Bytewizer.TinyCLR.Hardware.Components;
using System;

namespace Bytewizer.TinyCLR.Hardware
{
    /// <summary>
    /// Common hardware interface to all TinyCLR devices.
    /// </summary>
    public interface IMainboard : IDisposable
    {
        #region General

        /// <summary>
        /// Hardware model.
        /// </summary>
        ChipsetModel Chipset { get; }
        
        /// <summary>
        /// Hardware model.
        /// </summary>
        INetworkDevice Network { get; }

        /// <summary>
        /// Onboard LED.
        /// </summary>
        ILedDevice Led { get; }

        #endregion
    }
}
