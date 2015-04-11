namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// Warns of an impending leap second to be inserted/deleted in the last minute of the current day.
    /// </summary>
    public enum LeapIndicator
    {
        /// <summary>
        /// No warning.
        /// </summary>
        NoWarning = 0,

        /// <summary>
        /// Last minute has 61 seconds.
        /// </summary>
        LastMinute61Seconds = 1,

        /// <summary>
        /// Last minute has 59 seconds.
        /// </summary>
        LastMinute59Seconds = 2,

        /// <summary>
        /// Alarm condition (clock not synchronized).
        /// </summary>
        Alarm = 3
    }

    /// <summary>
    /// A class that gets the name of the leap Indicator.
    /// </summary>
    public static class LeapIndicatorString
    {
        public static string GetName(LeapIndicator LeapIndicatorEnum)
        {
            switch (LeapIndicatorEnum)
            {
                case LeapIndicator.NoWarning:
                    return "No Warning";
                case LeapIndicator.LastMinute61Seconds:
                    return "Last Minute 61 Seconds";
                case LeapIndicator.LastMinute59Seconds:
                    return "Last Minute 59 Seconds";
                case LeapIndicator.Alarm:
                    return "Alarm - Clock Not Synchronized";
                default:
                    return "Unknown";
            }
        }
    }
}
