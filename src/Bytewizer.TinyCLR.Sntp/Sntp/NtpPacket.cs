using System;
using System.Text;

namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Represents RFC4330 SNTP packet used for communication to and from a network time server.
    /// </summary>
    public class NtpPacket
    {
        static readonly DateTime epoch = new DateTime(1900, 1, 1);

        /// <summary>
        /// Gets RFC4330-encoded SNTP packet.
        /// </summary>
        /// <value>
        /// Byte array containing RFC4330-encoded SNTP packet. It is at least 48 bytes long.
        /// </value>
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// Gets or sets the leap second indicator.
        /// </summary>
        /// <value>
        /// Leap second warning, if any. Special value
        /// <see cref="LeapIndicator.AlarmCondition" /> indicates unsynchronized server clock.
        /// Default is <see cref="LeapIndicator.NoWarning" />.
        /// </value>
        /// <remarks>
        /// Only servers fill in this property. Clients can consult this property for possible leap second warning.
        /// </remarks>
        public LeapIndicator LeapIndicator
        {
            get { return (LeapIndicator)((Bytes[0] & 0xC0) >> 6); }
            set { Bytes[0] = (byte)((Bytes[0] & 0xC0) | (int)value << 6); }
        }

        /// <summary>
        /// Gets or sets protocol version number.
        /// </summary>
        /// <value>
        /// SNTP protocol version. Default is 4, which is the latest version at the time of this writing.
        /// </value>
        /// <remarks>
        /// In request packets, clients should leave this property at default value 4.
        /// Servers usually reply with the same protocol version.
        /// </remarks>
        public VersionNumber VersionNumber
        {
            get { return (VersionNumber)((Bytes[0] & 0x38) >> 3); }
            set { Bytes[0] = (byte)((Bytes[0] & ~0x38) | (int)value << 3); } //TODO: Check
        }

        /// <summary>
        /// Gets or sets SNTP packet mode, i.e. whether this is client or server packet.
        /// </summary>
        /// <value>
        /// SNTP packet mode. Default is <see cref="NtpMode.Client" /> in newly created packets.
        /// Server reply should have this property set to <see cref="NtpMode.Server" />.
        /// </value>
        public NtpMode Mode
        {
            get { return (NtpMode)(Bytes[0] & 0x07); }
            set { Bytes[0] = (byte)((Bytes[0] & ~0x07) | (int)value); }
        }

        /// <summary>
        /// Gets or sets server's distance from the reference clock.
        /// </summary>
        /// <value>
        /// <para>
        /// Distance from the reference clock. This property is set only in server reply packets.
        /// Servers connected directly to reference clock hardware set this property to 1.
        /// Statum number is incremented by 1 on every hop down the NTP server hierarchy.
        /// </para>
        /// <para>
        /// Special value 0 indicates that this packet is a Kiss-o'-Death message
        /// with kiss code stored in <see cref="ReferenceId" />.
        /// </para>
        /// </value>
        public Stratum Stratum
        {
            get { return (Stratum)(Bytes[1]); }
            set { Bytes[1] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets server's preferred polling interval.
        /// </summary>
        /// <value>
        /// Polling interval in log₂ seconds, e.g. 4 stands for 16s and 17 means 131,072s.
        /// </value>
        public int Poll
        {
            get { return Bytes[2]; }
            set { Bytes[2] = (byte)value; } 
        }

        /// <summary>
        /// Gets or sets the precision of server clock.
        /// </summary>
        /// <value>
        /// Clock precision in log₂ seconds, e.g. -20 for microsecond precision.
        /// </value>
        public int Precision
        {
            get { return Bytes[3]; }
            set { Bytes[3] = (byte)value; } 
        }

        /// <summary>
        /// Gets the total round-trip delay from the server to the reference clock.
        /// </summary>
        /// <value>
        /// Round-trip delay to the reference clock. Normally a positive value smaller than one second.
        /// </value>
        public TimeSpan RootDelay
        {
            get { return GetTimeSpan32(4); }
            //set { SetTimeSpan32(4, value); }
        }

        /// <summary>
        /// Gets the estimated error in time reported by the server.
        /// </summary>
        /// <value>
        /// Estimated error in time reported by the server. Normally a positive value smaller than one second.
        /// </value>
        public TimeSpan RootDispersion
        {
            get { return GetTimeSpan32(8); }
            //set { SetTimeSpan32(8, value); }
        }

        /// <summary>
        /// Gets the ID of the time source used by the server or Kiss-o'-Death code sent by the server.
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
        /// </para>
        /// <para>
        /// When server sets <see cref="Stratum" /> to special value 0,
        /// this property contains so called kiss code that instructs the client to stop querying the server.
        /// </para>
        /// </value>
        public ReferenceId ReferenceId
        {
            get { return (ReferenceId)GetUInt32BE(12); }
            set { SetUInt32BE(12, (uint)value); }
        }

        /// <summary>
        /// Gets the ID of the time source used by the server or Kiss-o'-Death code sent by the server.
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
        /// </para>
        /// <para>
        /// When server sets <see cref="Stratum" /> to special value 0,
        /// this property contains so called kiss code that instructs the client to stop querying the server.
        /// </para>
        /// </value>
        public uint ReferenceIPAddress
        {
            get { return GetUInt32BE(12); }
            set { SetUInt32BE(12, value); }
        }

        /// <summary>
        /// Gets or sets the time when the server clock was last set or corrected.
        /// </summary>
        /// <value>
        /// Time when the server clock was last set or corrected or <c>null</c> when not specified.
        /// </value>
        /// <remarks>
        /// This Property is usually set only by servers. It usually lags server's current time by several minutes,
        /// so don't use this property for time synchronization.
        /// </remarks>
        public DateTime ReferenceTimestamp
        {
            get { return GetDateTime64(16); }
            set { SetDateTime64(16, value); }
        }

        /// <summary>
        /// Gets or sets the time when the client sent its request.
        /// </summary>
        /// <value>
        /// This property is <c>null</c> in request packets.
        /// In reply packets, it is the time when the client sent its request.
        /// Servers copy this value from <see cref="TransmitTimestamp" />
        /// that they find in received request packet.
        /// </value>
        public DateTime OriginTimestamp
        {
            get { return GetDateTime64(24); }
            set { SetDateTime64(24, value); }
        }

        /// <summary>
        /// Gets or sets the time when the request was received by the server.
        /// </summary>
        /// <value>
        /// This property is <c>null</c> in request packets.
        /// In reply packets, it is the time when the server received client request.
        /// </value>
        public DateTime ReceiveTimestamp
        {
            get { return GetDateTime64(32); }
            set { SetDateTime64(32, value); }
        }

        /// <summary>
        /// Gets or sets the time when the packet was sent.
        /// </summary>
        /// <value>
        /// Time when the packet was sent. It should never be <c>null</c>.
        /// Default value is <see cref="DateTime.UtcNow" />.
        /// </value>
        /// <remarks>
        /// This property must be set by both clients and servers.
        /// </remarks>
        public DateTime TransmitTimestamp
        {
            get { return GetDateTime64(40); }
            set { SetDateTime64(40, value); }
        }

        /// <summary>
        /// Gets or sets the time of reception of response SNTP packet on the client.
        /// </summary>
        /// <value>
        /// Time of reception of response SNTP packet on the client. It is <c>null</c> in request packets.
        /// </value>
        /// <remarks>
        /// This property is not part of the protocol.
        /// It is set by <see cref="SntpClient" /> when reply packet is received.
        /// </remarks>
        public DateTime DestinationTimestamp { get; set; }

        /// <summary>
        /// Gets the round-trip time to the server.
        /// </summary>
        /// <value>
        /// Time the request spent travelling to the server plus the time the reply spent travelling back.
        /// This is calculated from timestamps in the packet as <c>(t1 - t0) + (t3 - t2)</c>
        /// where t0 is <see cref="OriginTimestamp" />,
        /// t1 is <see cref="ReceiveTimestamp" />,
        /// t2 is <see cref="TransmitTimestamp" />,
        /// and t3 is <see cref="DestinationTimestamp" />.
        /// This property throws an exception in request packets.
        /// </value>
        public TimeSpan RoundTripTime
        {
            get
            {
                CheckTimestamps();
                return (ReceiveTimestamp - OriginTimestamp) + (DestinationTimestamp - TransmitTimestamp);
            }
        }

        /// <summary>
        /// Gets the offset that should be added to local time to synchronize it with server time.
        /// </summary>
        /// <value>
        /// Time difference between server and client. It should be added to local time to get server time.
        /// It is calculated from timestamps in the packet as <c>0.5 * ((t1 - t0) - (t3 - t2))</c>
        /// where t0 is <see cref="OriginTimestamp" />,
        /// t1 is <see cref="ReceiveTimestamp" />,
        /// t2 is <see cref="TransmitTimestamp" />,
        /// and t3 is <see cref="DestinationTimestamp" />.
        /// This property throws an exception in request packets.
        /// </value>
        public TimeSpan CorrectionOffset
        {
            get
            {
                CheckTimestamps();
                return TimeSpan.FromTicks(((ReceiveTimestamp - OriginTimestamp) - (DestinationTimestamp - TransmitTimestamp)).Ticks / 2);
            }
        }

        /// <summary>
        /// Initializes default request packet.
        /// </summary>
        /// <remarks>
        /// Created request packet can be passed to <see cref="SntpClient.Query(NtpPacket)" />.
        /// Properties <see cref="Mode" /> and <see cref="VersionNumber" />
        /// are set appropriately for request packet. Property <see cref="TransmitTimestamp" />
        /// is set to <see cref="DateTime.UtcNow" />.
        /// </remarks>
        public NtpPacket()
            : this(new byte[48])
        {
            Mode = NtpMode.Client;
            VersionNumber = VersionNumber.Version3;
            TransmitTimestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Converts SNTP packet into string object.
        /// </summary>
        /// <param name="packet">Packet to convert into string.</param>
        public static string Print(NtpPacket packet)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Received {packet.Bytes.Length}B packet");
            sb.AppendLine("-------------------------------------");
            //sb.AppendLine($"Correction offset: {packet.CorrectionOffset}");
            //sb.AppendLine($"Round-trip time:   {packet.RoundTripTime}");
            sb.AppendLine($"Origin time:       {packet.OriginTimestamp:dd/MM/yyyy hh:mm:ss.fff}");
            sb.AppendLine($"Receive time:      {packet.ReceiveTimestamp:dd/MM/yyyy hh:mm:ss.fff}");
            sb.AppendLine($"Transmit time:     {packet.TransmitTimestamp:dd/MM/yyyy hh:mm:ss.fff}");
            sb.AppendLine($"Destination time:  {packet.DestinationTimestamp:dd/MM/yyyy hh:mm:ss.fff}");
            sb.AppendLine($"Protocol version:  {packet.VersionNumber}");
            sb.AppendLine($"Protocol mode:     {packet.Mode}");
            sb.AppendLine($"Leap second:       {packet.LeapIndicator}");
            sb.AppendLine($"Stratum:           {packet.Stratum}");
            sb.AppendLine($"Reference ID:      0x{packet.ReferenceId:x}");
            sb.AppendLine($"Reference time:    {packet.ReferenceTimestamp:dd/MM/yyyy hh:mm:ss.fff}");
            sb.AppendLine($"Root delay:        {packet.RootDelay.TotalMilliseconds}ms");
            sb.AppendLine($"Root dispersion:   {packet.RootDispersion.TotalMilliseconds}ms");
            sb.AppendLine($"Poll interval:     2^{packet.Poll}s");
            sb.AppendLine($"Precision:         2^{packet.Precision}s");

            return sb.ToString();
        }

        internal NtpPacket(byte[] bytes)
        {
            if (bytes.Length < 48)
            {
                throw new Exception("SNTP reply packet must be at least 48 bytes long.");
            }
            Bytes = bytes;
        }

        internal void ValidateRequest()
        {
            if (Mode != NtpMode.Client)
            {
                throw new Exception("This is not a request or response SNTP packet.");
            }

            if (VersionNumber == 0)
            {
                throw new Exception("Protocol version of the request is not specified.");
            }

            if (TransmitTimestamp == DateTime.MinValue)
            {
                throw new Exception("TransmitTimestamp must be set in request packet.");
            }
        }

        internal void ValidateReply(NtpPacket request)
        {
            if (Mode != NtpMode.Server)
            {
                throw new Exception("This is not a reply SNTP packet.");
            }

            if (VersionNumber == 0)
            {
                throw new Exception("Protocol version of the reply is not specified.");
            }
            if (Stratum == 0)
            {
                throw new Exception($"Received Kiss-o'-Death SNTP packet with code 0x{ReferenceId:x}.");
            }
            if (LeapIndicator == LeapIndicator.AlarmCondition)
            {
                throw new Exception("SNTP server has unsynchronized clock.");
            }

            CheckTimestamps();

            if (OriginTimestamp != request.TransmitTimestamp)
            {
                throw new Exception("Origin timestamp in reply doesn't match transmit timestamp in request.");
            }
        }

        private void CheckTimestamps()
        {
            if (OriginTimestamp == DateTime.MinValue)
            {
                throw new Exception("Origin timestamp is missing.");
            }

            if (ReceiveTimestamp == DateTime.MinValue)
            {
                throw new Exception("Receive timestamp is missing.");
            }

            if (TransmitTimestamp == DateTime.MinValue)
            {
                throw new Exception("Transmit timestamp is missing.");
            }

            if (DestinationTimestamp == DateTime.MinValue)
            {
                throw new Exception("Destination timestamp is missing.");
            }
        }

        private DateTime GetDateTime64(int offset)
        {
            var field = GetUInt64BE(offset);
            if (field == 0)
            {
                return DateTime.MaxValue;
            }

            return new DateTime(epoch.Ticks + (long)(field * (1.0 / (1L << 32) * 10000000.0)));
        }

        private void SetDateTime64(int offset, DateTime value)
        {
            SetUInt64BE(offset, value == DateTime.MinValue ? 0 : (ulong)((value.Ticks - epoch.Ticks) * (0.0000001 * (1L << 32))));
        }

        private TimeSpan GetTimeSpan32(int offset)
        {
            return TimeSpan.FromSeconds(GetInt32BE(offset) / (double)(1 << 16));
        }

        //private void SetTimeSpan32(int offset, TimeSpan value)
        //{
        //    SetInt32BE(offset, value == TimeSpan.MinValue ? 0 : (int)((value.Ticks - epoch.Ticks) * (0.0000001 * (1L << 16))));
        //}

        private ulong GetUInt64BE(int offset)
        {
            return SwapEndianness(BitConverter.ToUInt64(Bytes, offset));
        }

        private void SetUInt64BE(int offset, ulong value)
        {
            Array.Copy(BitConverter.GetBytes(SwapEndianness(value)), 0, Bytes, offset, 8);
        }

        private int GetInt32BE(int offset)
        {
            return (int)GetUInt32BE(offset);
        }

        //private void SetInt32BE(int offset, int value)
        //{
        //    Array.Copy(BitConverter.GetBytes((int)SwapEndianness((uint)value)), 0, Bytes, offset, 4);
        //}

        private uint GetUInt32BE(int offset)
        {
            return SwapEndianness(BitConverter.ToUInt32(Bytes, offset));
        }

        private void SetUInt32BE(int offset, uint value)
        {
            Array.Copy(BitConverter.GetBytes(SwapEndianness(value)), 0, Bytes, offset, 4);
        }

        private static uint SwapEndianness(uint x)
        {
            return ((x & 0xff) << 24) | ((x & 0xff00) << 8) | ((x & 0xff0000) >> 8) | ((x & 0xff000000) >> 24);
        }

        private static ulong SwapEndianness(ulong x)
        {
            return ((ulong)SwapEndianness((uint)x) << 32) | SwapEndianness((uint)(x >> 32));
        }
    }
}