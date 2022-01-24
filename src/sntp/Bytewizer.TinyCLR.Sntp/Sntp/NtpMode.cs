namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Describes SNTP packet mode, i.e. client or server.
    /// </summary>
    public enum NtpMode
    {
        /// <summary>
        /// Identifies client-to-server SNTP packet.
        /// </summary>
        Client = 3,

        /// <summary>
        /// Identifies server-to-client SNTP packet.
        /// </summary>
        Server = 4,
    }
}
