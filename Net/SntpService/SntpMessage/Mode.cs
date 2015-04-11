namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// Indicator of the mode of operation.
    /// In unicast and anycast modes, the client sets this field to 3 (client) in the request
    /// and the server sets it to 4 (server) in the reply.
    /// In multicast mode, the server sets this field to 5 (broadcast).
    /// </summary>
    public enum Mode : byte
    {
        /// <summary>
        /// Reserved.
        /// </summary>
        Reserved = 0x000,

        /// <summary>
        /// Symmetric active.
        /// </summary>
        SymmetricActive = 0x001,

        /// <summary>
        /// Symmetric passive.
        /// </summary>
        SymmetricPassive = 0x002,

        /// <summary>
        /// Client.
        /// </summary>
        Client = 0x003,

        /// <summary>
        /// Server.
        /// </summary>
        Server = 0x004,

        /// <summary>
        /// Broadcast.
        /// </summary>
        Broadcast = 0x005,

        /// <summary>
        /// Reserved for NTP control message.
        /// </summary>
        ReservedNTPControl = 0x006,

        /// <summary>
        /// Reserved for private use.
        /// </summary>
        ReservedPrivate = 0x007
    }

    /// <summary>
    /// A class that gets the name of the mode.
    /// </summary>
    public static class ModeString
    {
        public static string GetName(Mode ModeEnum)
        {
            switch (ModeEnum)
            {
                case Mode.Reserved:
                    return "Reserved";
                case Mode.SymmetricActive:
                    return "Symmetric Active";
                case Mode.SymmetricPassive:
                    return "Symmetric Passive";
                case Mode.Client:
                    return "Client";
                case Mode.Server:
                    return "Server";
                case Mode.Broadcast:
                    return "Broadcast";
                case Mode.ReservedNTPControl:
                    return "Reserved NTP Control";
                case Mode.ReservedPrivate:
                    return "Reserved Private";
                default:
                    return "Unknown";
            }
        }
    }
 
}
