namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Represents leap second warning from the server that instructs the client to add or remove leap second.
    /// </summary>
    public enum LeapIndicator
    {
        /// <summary>
        /// No leap second warning. No action required.
        /// </summary>
        NoWarning = 0,

        /// <summary>
        /// Last minute has 61 seconds
        /// </summary>
        LastMinute61 = 1,

        /// <summary>
        /// Last minute has 59 seconds
        /// </summary>
        LastMinute59 = 2,

        /// <summary>
        /// Special value indicating that the server clock is unsynchronized and the returned time is unreliable.
        /// </summary>
        AlarmCondition = 3
    }
}
