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
        /// Warns the client that the last minute of the current day has 61 seconds.
        /// </summary>
        LastMinuteHas61Seconds = 1,

        /// <summary>
        /// Warns the client that the last minute of the current day has 59 seconds.
        /// </summary>
        LastMinuteHas59Seconds = 2,

        /// <summary>
        /// Special value indicating that the server clock is unsynchronized and the returned time is unreliable.
        /// </summary>
        AlarmCondition = 3
    }
}
