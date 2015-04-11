using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// The DNS CLASS fields appear in resource records. (RFC 1035 3.2.4)
    /// </summary>
    public enum RecordClass : ushort
    {
        /// <summary>
        ///  The Internet
        /// </summary>
        IN = 1,

        /// <summary>
        /// The CSNET class (Obsolete - used only for examples in 
        /// some obsolete RFCs)
        /// </summary>
        [Obsolete("Used only for examples in some obsolete RFCs.", true)]
        CS = 2,

        /// <summary>
        /// The CHAOS class
        /// </summary>
        CH = 3,

        /// <summary>
        /// Hesiod [Dyer 87]
        /// </summary>
        HS = 4
    }
}
