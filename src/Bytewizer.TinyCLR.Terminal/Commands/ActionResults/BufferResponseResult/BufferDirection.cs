namespace Bytewizer.TinyCLR.Terminal.Channel
{
    /// <summary>
    /// Consumable types.
    /// </summary>
    public enum BufferDirection
    {
        /// <summary>
        /// Get the next set of data.
        /// </summary>
        Forward = 0,

        /// <summary>
        /// Get the prior set of data.
        /// </summary>
        Backward = 1,

        /// <summary>
        /// Repeat the last set of data.
        /// </summary>
        Repeat = 2,

        /// <summary>
        /// Moves forward sending all data.
        /// </summary>
        ForwardAllData = 3
    }
}