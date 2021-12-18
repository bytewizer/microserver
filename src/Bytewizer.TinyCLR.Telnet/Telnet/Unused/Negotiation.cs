//using System;
//using System.Collections;
//using System.Text;
//using System.Threading;

//namespace Bytewizer.TinyCLR.Telnet
//{
//    /// <summary>
//    /// Handles Telnet negotiation
//    /// </summary>
//    public static class Negotiation
//    {
//        /// <summary>
//        /// Negotiation codes as described in RFC 854
//        /// </summary>
//        /// <see cref="http://www.faqs.org/rfcs/rfc854.html"/>
//        public enum Tokens
//        {
//            /// <summary>
//            /// End of subnegotiation parameters.
//            /// </summary>
//            SE = 240,
//            /// <summary>
//            /// No operation.
//            /// </summary>
//            NOP = 241,
//            /// <summary>
//            /// The data stream portion of a Synch.
//            /// This should always be accompanied
//            /// by a TCP Urgent notification. 
//            /// </summary>
//            DataMark = 242,
//            /// <summary>
//            /// NVT character BRK.
//            /// </summary>
//            Break = 243,
//            /// <summary>
//            /// The function IP. Interrupt Process
//            /// </summary>
//            IP = 244,
//            /// <summary>
//            /// The function AO. Abort output
//            /// </summary>
//            AO = 245,
//            /// <summary>
//            /// The function AYT. Are You There
//            /// </summary>
//            AYT = 246,
//            /// <summary>
//            /// The function EC.
//            /// </summary>
//            EraseChar = 247,
//            /// <summary>
//            /// The function EL.
//            /// </summary>
//            EraseLine = 248,
//            /// <summary>
//            /// The GA signal.
//            /// </summary>
//            GoAhead = 249,
//            /// <summary>
//            /// Indicates that what follows is
//            /// subnegotiation of the indicated
//            /// option. 
//            /// </summary>
//            Subnegotiation = 250,
//            /// <summary>
//            /// Indicates the desire to begin
//            /// performing, or confirmation that
//            /// you are now performing, the
//            /// indicated option. 
//            /// </summary>
//            WILL = 251,
//            /// <summary>
//            /// Indicates the refusal to perform,
//            /// or continue performing, the
//            /// indicated option. 
//            /// </summary>
//            WONT = 252,
//            /// <summary>
//            /// Indicates the request that the
//            /// other party perform, or
//            /// confirmation that you are expecting
//            /// the other party to perform, the
//            /// indicated option. 
//            /// </summary>
//            DO = 253,
//            /// <summary>
//            /// Indicates the demand that the
//            /// other party stop performing,
//            /// or confirmation that you are no
//            /// longer expecting the other party
//            /// to perform, the indicated option. 
//            /// </summary>
//            DONT = 254,
//            /// <summary>
//            /// Data Byte 255.
//            /// </summary>
//            IAC = 255
//        }

//        /// <summary>
//        /// Negotiable options from IANA
//        /// </summary>
//        /// <see cref="https://www.iana.org/assignments/telnet-options/telnet-options.xhtml"/>
//        public enum Operations
//        {
//            /// <summary>
//            /// [RFC856]
//            /// </summary>
//            BinaryTransmission = 0,
//            /// <summary>
//            /// [RFC857]
//            /// </summary>
//            Echo = 1,
//            /// <summary>
//            /// [DDN Protocol Handbook, "Telnet Reconnection Option", "Telnet Output Line Width Option", "Telnet Output Page Size Option", NIC 50005, December 1985.]
//            /// </summary>
//            Reconnection = 2,
//            /// <summary>
//            /// [RFC858]
//            /// </summary>
//            SuppressGoAhead = 3,
//            /// <summary>
//            /// ["The Ethernet, A Local Area Network: Data Link Layer and Physical Layer Specification", AA-K759B-TK, Digital Equipment Corporation, Maynard, MA.Also as: "The Ethernet - A Local Area Network", Version 1.0, Digital Equipment Corporation, Intel Corporation, Xerox Corporation, September 1980. And: "The Ethernet, A Local Area Network: Data Link Layer and Physical Layer Specifications", Digital, Intel and Xerox, November 1982. And: XEROX, "The Ethernet, A Local Area Network: Data Link Layer and Physical Layer Specification", X3T51/80-50, Xerox Corporation, Stamford, CT., October 1980.]
//            /// </summary>
//            ApproxMessageSizeNegotiation = 4,
//            /// <summary>
//            /// [RFC859]
//            /// </summary>
//            Status = 5,
//            /// <summary>
//            /// [RFC860]
//            /// </summary>
//            TimingMark = 6,
//            /// <summary>
//            /// [RFC726]
//            /// </summary>
//            RemoteControlledTransAndEcho = 7,
//            /// <summary>
//            /// [DDN Protocol Handbook, "Telnet Reconnection Option", "Telnet Output Line Width Option", "Telnet Output Page Size Option", NIC 50005, December 1985.]
//            /// </summary>
//            OutputLineWidth = 8,
//            /// <summary>
//            /// [DDN Protocol Handbook, "Telnet Reconnection Option", "Telnet Output Line Width Option", "Telnet Output Page Size Option", NIC 50005, December 1985.]
//            /// </summary>
//            OutputPageSize = 9,
//            /// <summary>
//            /// [RFC652]
//            /// </summary>
//            OutputCarriageReturnDisposition = 10,
//            /// <summary>
//            /// [RFC653]
//            /// </summary>
//            OutputHorizontalTabStops = 11,
//            /// <summary>
//            /// [RFC654]
//            /// </summary>
//            OutputHorizontalTabDisposition = 12,
//            /// <summary>
//            /// [RFC655]
//            /// </summary>
//            OutputFormfeedDisposition = 13,
//            /// <summary>
//            /// [RFC656]
//            /// </summary>
//            OutputVerticalTabstops = 14,
//            /// <summary>
//            /// [RFC657]
//            /// </summary>
//            OutputVerticalTabDisposition = 15,
//            /// <summary>
//            /// [RFC658]
//            /// </summary>
//            OutputLinefeedDisposition = 16,
//            /// <summary>
//            ///  [RFC698]
//            /// </summary>
//            ExtendedASCII = 17,
//            /// <summary>
//            /// [RFC727]
//            /// </summary>
//            Logout = 18,
//            /// <summary>
//            /// [RFC735]
//            /// </summary>
//            ByteMacro = 19,
//            /// <summary>
//            /// [RFC1043], [RFC732]
//            /// </summary>
//            DataEntryTerminal = 20,
//            /// <summary>
//            /// [RFC736],  [RFC734]
//            /// </summary>
//            SUPDUP = 21,
//            /// <summary>
//            /// [RFC749]
//            /// </summary>
//            SUPDUPOutput = 22,
//            /// <summary>
//            /// [RFC779]
//            /// </summary>
//            SendLocation = 23,
//            /// <summary>
//            /// [RFC1091]
//            /// </summary>
//            TerminalType = 24,
//            /// <summary>
//            /// [RFC885]
//            /// </summary>
//            EndOfRecord = 25,
//            /// <summary>
//            /// [RFC927]
//            /// </summary>
//            TACACSUserIdentification = 26,
//            /// <summary>
//            /// [RFC933]
//            /// </summary>
//            OutputMarking = 27,
//            /// <summary>
//            /// [RFC946]
//            /// </summary>
//            TerminalLocationNumber = 28,
//            /// <summary>
//            /// [RFC1041]
//            /// </summary>
//            Telnet3270Regime = 29,
//            /// <summary>
//            /// [RFC1053]
//            /// </summary>
//            X3PAD = 30,
//            /// <summary>
//            /// [RFC1073]
//            /// </summary>
//            NegotiateAboutWindowSize = 31,
//            /// <summary>
//            /// [RFC1079]
//            /// </summary>
//            TerminalSpeed = 32,
//            /// <summary>
//            /// [RFC1372]
//            /// </summary>
//            RemoteFlowControl = 33,
//            /// <summary>
//            /// [RFC1184]
//            /// </summary>
//            Linemode = 34,
//            /// <summary>
//            /// [RFC1096]
//            /// </summary>
//            XDisplayLocation = 35,
//            /// <summary>
//            /// [RFC1408]
//            /// </summary>
//            EnvironmentOption = 36,
//            /// <summary>
//            /// [RFC2941]
//            /// </summary>
//            AuthenticationOption = 37,
//            /// <summary>
//            /// [RFC2946]
//            /// </summary>
//            EncryptionOption = 38,
//            /// <summary>
//            /// [RFC1572]
//            /// </summary>
//            NewEnvironmentOption = 39,
//            /// <summary>
//            /// [RFC2355]
//            /// </summary>
//            TN3270E = 40,
//            /// <summary>
//            /// [Rob_Earhart]
//            /// </summary>
//            XAUTH = 41,
//            /// <summary>
//            /// [RFC2066]
//            /// </summary>
//            CHARSET = 42,
//            /// <summary>
//            /// [Robert_Barnes]
//            /// </summary>
//            TelnetRemoteSerialPort = 43,
//            /// <summary>
//            /// [RFC2217]
//            /// </summary>
//            ComPortControlOption = 44,
//            /// <summary>
//            /// [Wirt_Atmar]
//            /// </summary>
//            TelnetSuppressLocalEcho = 45,
//            /// <summary>
//            /// [Michael_Boe]
//            /// </summary>
//            TelnetStartTLS = 46,
//            /// <summary>
//            /// [RFC2840]
//            /// </summary>
//            KERMIT = 47,
//            /// <summary>
//            /// [David_Croft]
//            /// </summary>
//            SEND_URL = 48,
//            /// <summary>
//            /// [Jeffrey_Altman]
//            /// </summary>
//            FORWARD_X = 49,
//            /// <summary>
//            /// [Steve_McGregory]
//            /// </summary>
//            TELOPT_PRAGMA_LOGON = 138,
//            /// <summary>
//            /// [Steve_McGregory]
//            /// </summary>
//            TELOPT_SSPI_LOGON = 139,
//            /// <summary>
//            /// [Steve_McGregory]
//            /// </summary>
//            TELOPT_PRAGMA_HEARTBEAT = 140,
//            /// <summary>
//            /// [RFC861]
//            /// </summary>
//            Extended_Options_List = 255,
//        }

//        /// <summary>
//        /// Sends Do command
//        /// </summary>
//        /// <param name="op">Operation code</param>
//        /// <returns>Negotiation sequence</returns>
//        public static byte[] Do(Operations op) =>
//            new byte[] { (byte)Tokens.IAC, (byte)Tokens.DO, (byte)op };

//        /// <summary>
//        /// Sends Don't command
//        /// </summary>
//        /// <param name="op">Operation code</param>
//        /// <returns>Negotiation sequence</returns>
//        public static byte[] Dont(Operations op) =>
//            new byte[] { (byte)Tokens.IAC, (byte)Tokens.DONT, (byte)op };

//        /// <summary>
//        /// Sends Will command
//        /// </summary>
//        /// <param name="op">Operation code</param>
//        /// <returns>Negotiation sequence</returns>
//        public static byte[] Will(Operations op) =>
//           new byte[] { (byte)Tokens.IAC, (byte)Tokens.WILL, (byte)op };

//        /// <summary>
//        /// Sends Won't command
//        /// </summary>
//        /// <param name="op">Operation code</param>
//        /// <returns>Negotiation sequence</returns>
//        public static byte[] Wont(Operations op) =>
//            new byte[] { (byte)Tokens.IAC, (byte)Tokens.WONT, (byte)op };

//        /// <summary>
//        /// Handles window size negotiation as described in RFC1073
//        /// <see cref="https://tools.ietf.org/html/rfc1073"/>
//        /// </summary>
//        /// <param name="data">received data</param>
//        /// <param name="c">client to update</param>
//        /// <remarks>
//        /// Manages sequences like
//        /// <code>
//        /// IAC SB NAWS "16-bit value" "16-bit valu"> IAC SE
//        /// </code>
//        /// Sent by the Telnet client to inform the Telnet server of the
//        /// window width and height.
//        /// </remarks>
//        public static void HandleWindowSize(byte[] data, int offset, IBBSClient c)
//        {
//            if (data[offset + 1] == (byte)Tokens.Subnegotiation
//                && data[offset + 2] == (byte)Operations.NegotiateAboutWindowSize)
//            {
//                int w = data[offset + 3] * 256 + data[offset + 4];
//                int h = data[offset + 5] * 256 + data[offset + 6];
//                if (w != 0) c.screenWidth = w;
//                if (h != 0) c.screenHeight = h;
//            }
//        }

//        /// <summary>
//        /// Tests if client supports binary transmission
//        /// </summary>
//        /// <param name="data"></param>
//        /// <param name="offset"></param>
//        /// <param name="c"></param>
//        public static void HandleBinary(byte[] data, int offset, IBBSClient c)
//        {
//            if (data[offset + 1] == (byte)Tokens.DO
//                && data[offset + 2] == (byte)Operations.BinaryTransmission)
//                c.BinaryMode = true;
//        }

//        /// <summary>
//        /// Tests if client support Terminal Type Negotiation RFC884 - RFC1091
//        /// <see cref="https://tools.ietf.org/html/rfc884"/>
//        /// <see cref="https://tools.ietf.org/html/rfc1091"/>
//        /// </summary>
//        /// <param name="data">received bytes</param>
//        /// <returns></returns>
//        public static bool ClientWillTerminalType(byte[] data, int offset)
//        {
//            bool ret = false;
//            if (data[offset + 1] == (byte)Tokens.WILL
//                && data[offset + 2] == (byte)Operations.TerminalType)
//                ret = true;
//            return ret;
//        }

//        /// <summary>
//        /// Sequence to request terminal type to the client RFC884 - RFC1091
//        /// <see cref="https://tools.ietf.org/html/rfc884"/>
//        /// <see cref="https://tools.ietf.org/html/rfc1091"/>
//        /// </summary>
//        /// <returns>IAC SB TERMINAL-TYPE SEND IAC SE</returns>
//        public static byte[] AskForTerminalType() => new byte[]
//        {
//            (byte)Tokens.IAC,
//            (byte)Tokens.Subnegotiation,
//            (byte)Operations.TerminalType,
//            1,
//            (byte)Tokens.IAC,
//            (byte)Tokens.SE
//        };

//        /// <summary>
//        /// Receive terminal type from cliente RFC884 - RFC1091
//        /// <see cref="https://tools.ietf.org/html/rfc884"/>
//        /// <see cref="https://tools.ietf.org/html/rfc1091"/>
//        /// </summary>
//        /// <param name="data">received data</param>
//        /// <param name="c">client to update</param>
//        /// <remarks>
//        /// Client sends data in the form
//        /// <code>
//        /// IAC SB TERMINAL-TYPE IS ... IAC SE
//        /// </code>
//        /// The code for IS is 0.
//        /// </remarks>
//        public static bool HandleTerminalType(byte[] data, int offset, IBBSClient c)
//        {
//            if (data[offset + 1] == (byte)Tokens.Subnegotiation
//                && data[offset + 2] == (byte)Operations.TerminalType
//                && data[offset + 3] == 0)
//            {
//                StringBuilder sb = new StringBuilder();
//                for (int j = offset + 4; data[j] != (byte)Tokens.IAC; j++)
//                    sb.Append((char)data[j]);

//                if (string.IsNullOrEmpty(c.terminalType))
//                    c.terminalType = sb.ToString();

//                return c.TryAddTerminalType(sb.ToString());
//            }
//            return false;
//        }
//    }
//}
