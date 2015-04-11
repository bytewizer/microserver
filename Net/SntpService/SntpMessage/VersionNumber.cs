namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// Indicator of the NTP/SNTP version number.
    /// </summary>
    public enum VersionNumber : byte
    {
        /// <summary>
        /// Version 3 (IPv4 only).
        /// </summary>
        Version3 = 0x003,

        /// <summary>
        /// Version 4 (IPv4, IPv6 and OSI).
        /// </summary>
        Version4 = 0x004,
    }

    /// <summary>
    /// A class that gets the name of the version number.
    /// </summary>
    public static class VersionNumberString
    {
        public static string GetName(VersionNumber VersionNumberEnum)
        {
            switch (VersionNumberEnum)
            {
                case VersionNumber.Version3:
                    return "Version 3";
                case VersionNumber.Version4:
                    return "Version 4";
                default:
                    return "Unknown";
            }
        }
    }
}
