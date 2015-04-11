namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// Indicator of the stratum level of a server clock.
    /// </summary>
    public enum Stratum : byte
    {
        /// <summary>
        /// Unspecified or unavailable.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Primary reference (e.g. radio clock).
        /// </summary>
        Primary = 1,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary = 2,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary3 = 3,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary4 = 4,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary5 = 5,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary6 = 6,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary7 = 7,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary8 = 8,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary9 = 9,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary10 = 10,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary11 = 11,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary12 = 12,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary13 = 13,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary14 = 14,

        /// <summary>
        /// Secondary reference (via NTP or SNTP).
        /// </summary>
        Secondary15 = 15,
    }

    /// <summary>
    /// A class that gets the name of the stratum.
    /// </summary>
    public static class StratumString
    {
        public static string GetName(Stratum StratumEnum)
        {
            switch (StratumEnum)
            {
                case Stratum.Unspecified:
                    return "Unspecified";
                case Stratum.Primary:
                    return "Primary";
                case Stratum.Secondary:
                    return "Secondary";
                case Stratum.Secondary3:
                    return "Secondary 3";
                case Stratum.Secondary4:
                    return "Secondary 4";
                case Stratum.Secondary5:
                    return "Secondary 5";
                case Stratum.Secondary6:
                    return "Secondary 6";
                case Stratum.Secondary7:
                    return "Secondary 7";
                case Stratum.Secondary8:
                    return "Secondary 8";
                case Stratum.Secondary9:
                    return "Secondary 9";
                case Stratum.Secondary10:
                    return "Secondary 10";
                case Stratum.Secondary11:
                    return "Secondary 11";
                case Stratum.Secondary12:
                    return "Secondary 12";
                case Stratum.Secondary13:
                    return "Secondary 13";
                case Stratum.Secondary14:
                    return "Secondary 14";
                case Stratum.Secondary15:
                    return "Secondary 15";
                default:
                    return "Unknown";
            }
        }
    }
}
