using System;
using System.Net;
using Bytewizer.TinyCLR.Sockets;

using GHIElectronics.TinyCLR.Devices.Rtc;
using GHIElectronics.TinyCLR.Native;

namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Represents configuration options of server specific features.
    /// </summary>
    public class SntpServerOptions : ServerOptions
    {
        /// <summary>
        /// Specifies the IP address or a DNS resolvable name of a server running an NTP service of a higher stratum.   
        /// A remote reference server or local reference clock from which packets are to be received. Local device time
        /// is used if a server is not specified.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Specifies local coordinated universal time ‎(UTC)‎ time to use for timestamps.
        /// </summary>
        public DateTime TimeSource
        {
            get
            {
                return DateTime.UtcNow;
            }
            set
            {
                if (RealtimeClock != null)
                {
                    RealtimeClock.Now = value;
                }

                SystemTime.SetTime(value);
                ReferenceTimestamp = value;
            }
        }

        /// <summary>
        /// Specifies real time clock (RTC) to use for timestamps.
        /// </summary>
        public RtcController RealtimeClock { get; set; } = null;

        /// <summary>
        /// Gets the time when the system clock was last set or corrected.
        /// </summary>
        public DateTime ReferenceTimestamp { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Specifies protocol version number.
        /// </summary>
        public VersionNumber VersionNumber { get; set; } = VersionNumber.Version3;

        /// <summary>
        /// Specifies server's distance from the reference clock.
        /// </summary>
        public Stratum Stratum { get; set; } = Stratum.Unspecified;

        /// <summary>
        /// Specifies the ID of the time source used by the server or Kiss-o'-Death code sent by the server.
        /// </summary>
        /// <value>
        /// <para>
        /// ID of server's time source or Kiss-o'-Death code.
        /// Purpose of this property depends on value of <see cref="Stratum" /> property.
        /// </para>
        /// <para>
        /// Stratum 1 servers write here one of several special values that describe the kind of hardware clock they use.
        /// </para>
        /// <para>
        /// Stratum 2 and lower servers set this property to IPv4 address of their upstream server.
        /// If upstream server has IPv6 address, the address is hashed, because it doesn't fit in this property.
        /// </para>
        /// <para>
        /// When server sets <see cref="Stratum" /> to special value 0,
        /// this property contains so called kiss code that instructs the client to stop querying the server.
        /// </para>
        /// </value>
        public ReferenceId ReferenceId { get; set; } = ReferenceId.LOCL;

        /// <summary>
        /// Specifies the ID of the time source used by the server or Kiss-o'-Death code sent by the server.
        /// Stratum 2 and lower servers set this property to IPv4 address of their upstream server.
        /// </summary>
        public IPAddress ReferenceIPAddress { get; set; } = IPAddress.Any;

        /// <summary>
        /// Specifies server's preferred polling interval in log₂ seconds.
        /// </summary>
        /// <value>
        /// Polling interval in log₂ seconds, e.g. 4 stands for 16s and 17 means 131,072s.
        /// </value>
        public int PollInterval { get; set; } = 13;

        /// <summary>
        /// Specifies the precision of the clock in log₂ seconds.
        /// </summary>
        /// <value>
        /// Clock precision in log₂ seconds, e.g. -20 for microsecond precision.
        /// </value>
        public int Precision { get; set; } = -20;

        /// <summary>
        /// Specifies .....
        /// </summary>
        public TimeSpan SyncInterval { get; set; } = TimeSpan.FromSeconds(8192);

        /// <summary>
        /// Specifies .....
        /// </summary>
        public TimeSpan SyncTimeout { get; set; } = TimeSpan.FromSeconds(5);
    }
}