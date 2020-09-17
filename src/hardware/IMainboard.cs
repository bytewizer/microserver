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
        HardwareModel Model { get; }
        
        /// <summary>
        /// Hardware model.
        /// </summary>
        INetworkDevice Network { get; }

        #endregion
    }
}
