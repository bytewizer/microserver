using System;
using System.Net;
using System.Text;

using Microsoft.SPOT;

using MicroServer.IO;
using MicroServer.Utilities;
using MicroServer.Extensions;


namespace MicroServer.Net.Sntp
{
    #region RFC Specification
    /// Structure of the standard NTP header as described in RFC 2030
    ///                       1                   2                   3
    ///   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |LI | VN  |Mode |    Stratum    |     Poll      |   Precision   |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                          Root Delay                           |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                       Root Dispersion                         |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                     Reference Identifier                      |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                   Reference Timestamp (64)                    |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                   Originate Timestamp (64)                    |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                    Receive Timestamp (64)                     |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                    Transmit Timestamp (64)                    |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                 Key Identifier (optional) (32)                |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///  |                                                               |
    ///  |                                                               |
    ///  |                 Message Digest (optional) (128)               |
    ///  |                                                               |
    ///  |                                                               |
    ///  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// 
    /// NTP Timestamp Format (as described in RFC 2030)
    ///                         1                   2                   3
    ///     0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |                           Seconds                             |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |                  Seconds Fraction (0-padded)                  |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

    #endregion RFC Specification

    /// <summary>
    /// A class that represents a SNTP packet.
    /// See http://www.faqs.org/rfcs/rfc2030.html for full details of protocol.
    /// </summary>
    public class SntpMessage
    {
        #region Fields

        /// <summary>
        /// Represents the EPOCH date in DateTime format.
        /// </summary>
        private static readonly DateTime Epoch = new DateTime(1900, 1, 1);

        /// <summary>
        /// Represents the number of ticks in 1 second.
        /// </summary>
        private const long TicksPerSecond = TimeSpan.TicksPerSecond;

        #endregion Fields

        #region Private Properties

        private DateTime _timestamp;
        private DateTime _destinationDateTime;

        // Standard NTP header structure properties  
        private byte _flags = new byte();
        private byte _stratum = new byte();
        private byte _poll = new byte();
        private byte _precision = new byte();
        private byte[] _rootdelay = new byte[4];
        private byte[] _rootDispersion = new byte[4];
        private byte[] _referenceIdentifier = new byte[4];
        private byte[] _referenceTimestamp = new byte[8];
        private byte[] _originateTimestamp = new byte[8];
        private byte[] _receiveTimestamp = new byte[8];
        private byte[] _transmitTimestamp = new byte[8];
        private byte[] _keyIdentifier = new byte[4];  // optional  
        private byte[] _messageDigest = new byte[16];  //optional

        #endregion Private Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpMessage"/> class.
        /// /// <param name="data">Array containing the data to decode.</param>
        /// </summary>
        public SntpMessage(byte[] data)
        {
            if (data.Length < Constants.SNTP_MIN_MESSAGE_SIZE || data.Length > Constants.SNTP_MAX_MESSAGE_SIZE)
            {
                throw new ArgumentOutOfRangeException("Data", "Byte array must have a length between "
                    + Constants.SNTP_MIN_MESSAGE_SIZE + " and " + Constants.SNTP_MAX_MESSAGE_SIZE);
            }

            using (ByteReader byteReader = new ByteReader(data, ByteOrder.Network))
            {
                _timestamp = DateTime.Now;

                _flags = byteReader.ReadByte();
                _stratum = byteReader.ReadByte();
                _poll = byteReader.ReadByte();
                _precision = byteReader.ReadByte();
                _rootdelay = byteReader.ReadBytes(4);
                _rootDispersion = byteReader.ReadBytes(4);
                _referenceIdentifier = byteReader.ReadBytes(4);
                _referenceTimestamp = byteReader.ReadBytes(8);
                _originateTimestamp = byteReader.ReadBytes(8);
                _receiveTimestamp = byteReader.ReadBytes(8);
                _transmitTimestamp = byteReader.ReadBytes(8);
                if (byteReader.AvailableBytes > 0)
                {
                    _keyIdentifier = byteReader.ReadBytes(4);  // optional  
                }
                if (byteReader.AvailableBytes > 0)
                {
                    _messageDigest = byteReader.ReadBytes(16);  //optional  
                }
            }
            this.DestinationDateTime = DateTime.Now.ToUniversalTime();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpMessage"/> class.
        /// </summary>
        public SntpMessage()
        {
            this.Timestamp = DateTime.Now;
            this.LeapIndicator = LeapIndicator.NoWarning;
            this.VersionNumber = VersionNumber.Version4;
            this.Mode = Mode.Client;
            Array.Clear(_rootdelay, 0, _rootdelay.Length);
            Array.Clear(_rootDispersion, 0, _rootDispersion.Length);
            Array.Clear(_referenceIdentifier, 0, _referenceIdentifier.Length);
            Array.Clear(_referenceTimestamp, 0, _referenceTimestamp.Length);
            Array.Clear(_originateTimestamp, 0, _originateTimestamp.Length);
            Array.Clear(_receiveTimestamp, 0, _receiveTimestamp.Length);
            this.TransmitDateTime = DateTime.Now.ToUniversalTime();
            Array.Clear(_keyIdentifier, 0, _keyIdentifier.Length);
            Array.Clear(_messageDigest, 0, _messageDigest.Length);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets timestamp when cached
        /// </summary>
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

         ///<summary>
         ///Gets a warning of an impending leap second to be inserted/deleted in the last minute of the current day.
         ///</summary>
        public LeapIndicator LeapIndicator
        {
            get { return (LeapIndicator)ByteUtility.GetBits(_flags, 6, 2); }
            set { _flags = ByteUtility.SetBits(ref _flags, 6, 2, (byte)value) ; }
        }

        /// <summary>
        /// Gets the NTP/SNTP version number.
        /// </summary>
        public VersionNumber VersionNumber
        {
            get { return (VersionNumber)ByteUtility.GetBits(_flags, 3, 3); }
            set { _flags = ByteUtility.SetBits(ref _flags, 3, 3, (byte)value); }
        }

        /// <summary>
        /// Gets the operating mode of whatever last altered the packet.
        /// </summary>
        public Mode Mode
        {
            get { return (Mode)ByteUtility.GetBits(_flags, 0, 3); }
            set { _flags = ByteUtility.SetBits(ref _flags, 0, 3, (byte)value); }
        }

        /// <summary>
        /// Gets the stratum level of the clock.
        /// </summary>
        public Stratum Stratum
        {
            get { return (Stratum)ByteUtility.GetBits(_stratum, 0, 8); }
            set { _stratum = ByteUtility.SetBits(ref _stratum, 0, 8, (byte)value); }
        }

        /// <summary>
        /// Gets the maximum interval between successive messages in seconds.
        /// </summary>
        public double PollInterval
        {
            get { return System.Math.Pow(2, (sbyte)ByteUtility.GetBits(_poll, 0, 8)); }
            set { _poll = (byte)System.Math.Log(value); }
        }

        /// <summary>
        /// Gets the precision of the clock in seconds.
        /// </summary>
        public double Precision
        {
            get { return System.Math.Pow(2, (sbyte)ByteUtility.GetBits(_precision, 0, 8)); }
            set { _precision = (byte)System.Math.Log(value); }
        }

        /// <summary>
        /// Gets the total delay to the primary reference source in seconds.
        /// </summary>
        public double RootDelay
        {
            get { return SecondsStampToSeconds(_rootdelay); }
        }

        /// <summary>
        /// Gets the nominal error relative to the primary reference source in seconds.
        /// </summary>
        public double RootDispersion
        {
            get { return SecondsStampToSeconds(_rootDispersion); }
        }

        /// <summary>
        /// Gets the identifier of the reference source.
        /// </summary>
        public ReferenceIdentifier ReferenceIdentifier
        {
            get { return (ReferenceIdentifier)BitConverter.ToUInt32(_referenceIdentifier, 0); }
            set { _referenceIdentifier = BitConverter.GetBytes((uint)value); }
        }

        /// <summary>
        /// Gets the ip address of the reference source when present.
        /// </summary>
        public IPAddress ReferenceIPAddress
        {
            get 
            {
                switch ((ReferenceIdentifier)BitConverter.ToUInt32(_referenceIdentifier, 0))
                {
                    case ReferenceIdentifier.LOCL:
                    case ReferenceIdentifier.PPS:
                    case ReferenceIdentifier.ACTS:
                    case ReferenceIdentifier.USNO:
                    case ReferenceIdentifier.PTB:
                    case ReferenceIdentifier.TDF:
                    case ReferenceIdentifier.DCF:
                    case ReferenceIdentifier.MSF:
                    case ReferenceIdentifier.WWV:
                    case ReferenceIdentifier.WWVB:
                    case ReferenceIdentifier.CHU:
                    case ReferenceIdentifier.LORC:
                    case ReferenceIdentifier.OMEG:
                    case ReferenceIdentifier.GPS:
                    case ReferenceIdentifier.GOES:
                        return IPAddress.Any;

                    default:
                        return ReferenceToIPAddress(_referenceIdentifier);
                 }   
            }
            set { _referenceIdentifier = IPAddressToReference(value); }
        }

        /// <summary>
        /// Gets the DateTime (UTC) at which the clock was last set or corrected.
        /// </summary>
        public DateTime ReferenceDateTime
        {
            get { return TimestampToDateTime(_referenceTimestamp); }
            set { DateTimeToTimestamp(value, _referenceTimestamp); }
        }

        /// <summary>
        /// Gets the DateTime (UTC) at which the request departed the client for the server.
        /// </summary>
        public DateTime OriginateDateTime
        {
            get { return TimestampToDateTime(_originateTimestamp); }
            set { DateTimeToTimestamp(value, _originateTimestamp); }
        }

        /// <summary>
        /// Gets the DateTime (UTC) at which the request arrived at the server.
        /// </summary>
        public DateTime ReceiveDateTime
        {
            get { return TimestampToDateTime(_receiveTimestamp); }
            set { DateTimeToTimestamp(value, _receiveTimestamp); }
        }

        /// <summary>
        /// Gets the DateTime (UTC) at which the reply departed the server for the client.
        /// </summary>
        public DateTime TransmitDateTime
        {
            get { return TimestampToDateTime(_transmitTimestamp); }
            set { DateTimeToTimestamp(value, _transmitTimestamp); }
        }

        /// <summary>
        /// Gets the DateTime (UTC) when the data arrived from the server.
        /// </summary>
        public DateTime DestinationDateTime
        {
            get{ return _destinationDateTime; }
            internal set { _destinationDateTime = value; }
        }

        /// <summary>
        /// Gets the local DateTime (UTC) form the device.
        /// </summary>
        public DateTime LocalDateTime
        {
            get { return DateTime.Now.ToUniversalTime(); }
        }

        /// <summary>
        /// Gets the difference in seconds between the local time and the time retrieved from the server.
        /// </summary>
        public double LocalClockOffset
        {
            get
            {
                return ((double)((ReceiveDateTime.Ticks - OriginateDateTime.Ticks) +
                    (TransmitDateTime.Ticks - DestinationDateTime.Ticks)) / 2) / TicksPerSecond;
            }
        }

        /// <summary>
        /// Gets the total roundtrip delay in seconds.
        /// </summary>
        public double RoundTripDelay
        {
            get
            {
                return (double)((DestinationDateTime.Ticks - OriginateDateTime.Ticks)
                    - (ReceiveDateTime.Ticks - TransmitDateTime.Ticks)) / TicksPerSecond;
            }
        }

        #endregion Properties

        #region Methods

        // Private Methods 

        /// <summary>
        /// Converts a DateTime into a byte array and stores it message.
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <param name="data">The data at which to convert.</param>
        /// <returns>A double that represents the value in seconds</returns>
        private void DateTimeToTimestamp(DateTime dateTime, byte[] data)
        {
            ulong ticks = (ulong)(dateTime - Epoch).Ticks;
            ulong seconds = ticks / TicksPerSecond;
            ulong fractions = ((ticks % TicksPerSecond) * 0x100000000L) / TicksPerSecond;
            for (int i = 3; i >= 0; i--)
            {
                data[0 + i] = (byte)seconds;
                seconds = seconds >> 8;
            }
            for (int i = 7; i >= 4; i--)
            {
                data[0 + i] = (byte)fractions;
                fractions = fractions >> 8;
            }
        }

        /// <summary>
        /// Converts a 32bit seconds (16 integer part, 16 fractional part) into a double that represents the value in seconds.
        /// </summary>
        /// <param name="data">The data at which to convert.</param>
        /// <returns>A double that represents the value in seconds</returns>
        private double SecondsStampToSeconds(byte[] data)
        {
            ulong seconds = 0;
            for (int i = 0; i <= 1; i++)
                seconds = (seconds << 8) | data[0 + i];
            ulong fractions = 0;
            for (int i = 2; i <= 3; i++)
                fractions = (fractions << 8) | data[0 + i];
            ulong ticks = (seconds * TicksPerSecond) + ((fractions * TicksPerSecond) / 0x10000L);
            return (double)ticks / TicksPerSecond;
        }

        /// <summary>
        /// Converts a byte array starting at the position specified into a DateTime.
        /// </summary>
        /// <param name="data">The data at which to convert.</param>
        /// <returns>A DateTime converted from a byte array starting at the position specified.</returns>
        private DateTime TimestampToDateTime(byte[] data)
        {
            ulong seconds = 0;
            for (int i = 0; i <= 3; i++)
                seconds = (seconds << 8) | data[0 + i];
            ulong fractions = 0;
            for (int i = 4; i <= 7; i++)
                fractions = (fractions << 8) | data[0 + i];
            ulong ticks = (seconds * TicksPerSecond) + ((fractions * TicksPerSecond) / 0x100000000L);
            return (Epoch + TimeSpan.FromTicks((long)ticks));
        }

        /// <summary>
        /// Converts a byte array into a IPAddress.
        /// </summary>
        /// <param name="data">The data at which to convert.</param>
        /// <returns>A IPAddress converted from a byte array.</returns>
        private IPAddress ReferenceToIPAddress(byte[] data)
        {
            try { return new IPAddress(data); }
            catch { return IPAddress.Any; }
        }

        /// <summary>
        /// Converts a IPAddress into a byte array .
        /// </summary>
        /// <param name="data">The data at which to convert.</param>
        /// <returns>A IPAddress converted from a byte array.</returns>
        private byte[] IPAddressToReference(IPAddress address)
        {
            try { return address.GetAddressBytes(); }
            catch { return new IPAddress(new byte[4] { 0, 0, 0, 0 }).GetAddressBytes(); }
        }

        // Public Methods

        /// <summary>
        /// Converts sntp message into a byte array.
        /// </summary>
        public byte[] ToArray()
        {
            using (ByteWriter byteWriter = new ByteWriter(Constants.SNTP_MIN_MESSAGE_SIZE, ByteOrder.Network))
            {
                byteWriter.Write(_flags);
                byteWriter.Write(_stratum);
                byteWriter.Write(_poll);
                byteWriter.Write(_precision);
                byteWriter.Write(_rootdelay);
                byteWriter.Write(_rootDispersion);
                byteWriter.Write(_referenceIdentifier);
                byteWriter.Write(_referenceTimestamp);
                byteWriter.Write(_originateTimestamp);
                byteWriter.Write(_receiveTimestamp);
                byteWriter.Write(_transmitTimestamp);
                byteWriter.Write(_keyIdentifier);  // optional  
                byteWriter.Write(_messageDigest);  //optional  

                return byteWriter.GetBytes();
            }
        }

        /// <summary>
        /// Converts sntp message into string object.
        /// </summary>
        public override string ToString()
        {
            string dateFormat = "MMM d, yyyy HH:mm:ss.fff 'GMT'";
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("  SNTP PACKET");
            sb.AppendLine(String.Concat("  Message Timestamp              : ", this.Timestamp.ToString()));
            sb.AppendLine(String.Concat("  Leap Indicator (LI)            : ", LeapIndicatorString.GetName(this.LeapIndicator)));
            sb.AppendLine(String.Concat("  Version Number (VI)            : ", VersionNumberString.GetName(this.VersionNumber)));
            sb.AppendLine(String.Concat("  Mode                           : ", ModeString.GetName(this.Mode)));
            sb.AppendLine(String.Concat("  Stratum                        : ", StratumString.GetName(this.Stratum)));
            sb.AppendLine(String.Concat("  Poll Interval                  : ", this.PollInterval.ToString(), " second(s)"));
            sb.AppendLine(String.Concat("  Precision                      : ", this.Precision.ToString(), " second(s)"));
            sb.AppendLine(String.Concat("  Root Delay                     : ", this.RootDelay.ToString(), " second(s)"));
            sb.AppendLine(String.Concat("  Root Dispersion                : ", this.RootDispersion.ToString(), " second(s)"));
            sb.AppendLine(String.Concat("  Reference Identifier           : ", ReferenceIdentifierString.GetName(this.ReferenceIdentifier)));
            sb.AppendLine(String.Concat("  Reference IP Address           : ", this.ReferenceIPAddress.ToString()));                                                  
            sb.AppendLine(String.Concat("  Reference DateTime             : ", this.ReferenceDateTime.ToString(dateFormat)));
            sb.AppendLine(String.Concat("  Originate DateTime             : ", this.OriginateDateTime.ToString(dateFormat)));
            sb.AppendLine(String.Concat("  Receive DateTime               : ", this.ReceiveDateTime.ToString(dateFormat)));
            sb.AppendLine(String.Concat("  Transmit DateTime              : ", this.TransmitDateTime.ToString(dateFormat)));
            sb.AppendLine(String.Concat("  Destination DateTime           : ", this.DestinationDateTime.ToString(dateFormat)));
            sb.AppendLine(String.Concat("  Local DateTime                 : ", this.LocalDateTime.ToString(dateFormat),
                                                                               "(", TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString(), ")") );
            sb.AppendLine(String.Concat("  Local Clock Offset             : ", this.LocalClockOffset.ToString(), " second(s)"));
            sb.AppendLine(String.Concat("  Round Trip Delay               : ", this.RoundTripDelay.ToString(), " second(s)"));
            sb.AppendLine(String.Concat("  Key Identifier                 : ", ByteUtility.PrintBytes(_keyIdentifier, false)));
            sb.AppendLine(String.Concat("  Message Digest                 : ", ByteUtility.PrintBytes(_messageDigest, false)));
            sb.AppendLine();

            return sb.ToString();
        }

        #endregion Methods
    }
}
