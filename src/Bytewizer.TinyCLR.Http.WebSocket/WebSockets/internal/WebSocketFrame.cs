using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.Http.WebSockets
{ 
    internal class WebSocketFrame
    {
        internal static readonly byte[] EmptyBytes = new byte[0];

        private WebSocketFrame()
        {
        }

        internal WebSocketFrame(Fin fin, Opcode opcode, PayloadData payloadData, bool compressed, bool mask)
        {
            Fin = fin;
            Opcode = opcode;

            Rsv1 = opcode.IsData() && compressed ? Rsv.On : Rsv.Off;
            Rsv2 = Rsv.Off;
            Rsv3 = Rsv.Off;

            var len = payloadData.Length;
            if (len < 126)
            {
                PayloadLength = (byte)len;
                ExtendedPayloadLength = EmptyBytes;
            }
            else if (len < 0x010000)
            {
                PayloadLength = 126;
                ExtendedPayloadLength = ((ushort)len).InternalToByteArray(ByteOrder.Big);
            }
            else
            {
                PayloadLength = 127;
                ExtendedPayloadLength = len.InternalToByteArray(ByteOrder.Big);
            }

            if (mask)
            {
                Mask = Mask.On;
                MaskingKey = CreateMaskingKey();
                payloadData.Mask(MaskingKey);
            }
            else
            {
                Mask = Mask.Off;
                MaskingKey = EmptyBytes;
            }

            PayloadData = payloadData;
        }

        internal ulong ExactPayloadLength
        {
            get
            {
                return PayloadLength < 126
                       ? PayloadLength
                       : PayloadLength == 126
                         ? ExtendedPayloadLength.ToUInt16(ByteOrder.Big)
                         : ExtendedPayloadLength.ToUInt64(ByteOrder.Big);
            }
        }

        internal int ExtendedPayloadLengthWidth
        {
            get
            {
                return PayloadLength < 126
                       ? 0
                       : PayloadLength == 126
                         ? 2
                         : 8;
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets the final frame state.
        /// </summary>
        public Fin Fin { get; private set; }

        /// <summary>
        /// Gets the extension switch one.
        /// </summary>
        public Rsv Rsv1 { get; private set; }

        /// <summary>
        /// Gets the extension switch two.
        /// </summary>
        public Rsv Rsv2 { get; private set; }

        /// <summary>
        /// Gets the extension switch three.
        /// </summary>
        public Rsv Rsv3 { get; private set; }

        /// <summary>
        /// Gets the type of frame.
        /// </summary>
        public Opcode Opcode { get; private set; }

        /// <summary>
        /// Gets the frame mask.
        /// </summary>
        public Mask Mask { get; private set; }

        /// <summary>
        /// Gets the frame payload data.
        /// </summary>
        public PayloadData PayloadData { get; private set; }

        /// <summary>
        /// Gets the frame payload length.
        /// </summary>
        public byte PayloadLength { get; private set; }

        /// <summary>
        /// Gets the extended payload lengh.
        /// </summary>
        public byte[] ExtendedPayloadLength { get; private set; }

        /// <summary>
        /// Gets the masking key.
        /// </summary>
        public byte[] MaskingKey { get; private set; }

        public bool IsBinary
        {
            get
            {
                return Opcode == Opcode.Binary;
            }
        }

        public bool IsClose
        {
            get
            {
                return Opcode == Opcode.Close;
            }
        }

        public bool IsCompressed
        {
            get
            {
                return Rsv1 == Rsv.On;
            }
        }

        public bool IsContinuation
        {
            get
            {
                return Opcode == Opcode.Cont;
            }
        }

        public bool IsControl
        {
            get
            {
                return Opcode >= Opcode.Close;
            }
        }

        public bool IsData
        {
            get
            {
                return Opcode == Opcode.Text || Opcode == Opcode.Binary;
            }
        }

        public bool IsFinal
        {
            get
            {
                return Fin == Fin.Final;
            }
        }

        public bool IsFragment
        {
            get
            {
                return Fin == Fin.More || Opcode == Opcode.Cont;
            }
        }

        public bool IsMasked
        {
            get
            {
                return Mask == Mask.On;
            }
        }

        public bool IsPing
        {
            get
            {
                return Opcode == Opcode.Ping;
            }
        }

        public bool IsPong
        {
            get
            {
                return Opcode == Opcode.Pong;
            }
        }

        public bool IsText
        {
            get
            {
                return Opcode == Opcode.Text;
            }
        }

        #endregion

        private static byte[] CreateMaskingKey()
        {
            var random = new Random();

            var key = new byte[4];
            random.NextBytes(key);

            return key;
        }

        private static WebSocketFrame ReadHeader(Stream stream)
        {
            var buffer = new byte[2];
            stream.Read(buffer, 0, 2);

            return ProcessHeader(buffer);
        }

        private static WebSocketFrame ReadPayloadData(Stream stream, WebSocketFrame frame)
        {
            var exactLen = frame.ExactPayloadLength;
            if (exactLen > PayloadData.MaxLength)
            {
                throw new InvalidOperationException("A frame has too long payload length.");
            }

            if (exactLen == 0)
            {
                frame.PayloadData = PayloadData.Empty;
                return frame;
            }

            var len = (int)exactLen;  // TODO:  Array.CopyTo dosn't support long
            var bytes = frame.PayloadLength < 127
                        ? stream.ReadBytes((int)exactLen)
                        : stream.ReadBytes(len, 1024);

            if (bytes.Length != len)
            {
                throw new InvalidOperationException("The payload data of a frame could not be read.");
            }

            frame.PayloadData = new PayloadData(bytes, len);
            return frame;
        }

        private static WebSocketFrame ReadMaskingKey(Stream stream, WebSocketFrame frame)
        {
            if (!frame.IsMasked)
            {
                frame.MaskingKey = EmptyBytes;
                return frame;
            }

            var len = 4;
            var bytes = stream.ReadBytes(len);

            if (bytes.Length != len)
            {
                throw new InvalidOperationException("The masking key of a frame could not be read.");
            }

            frame.MaskingKey = bytes;
            return frame;
        }
        
        private static WebSocketFrame ReadExtendedPayloadLength(Stream stream, WebSocketFrame frame)
        {
            var len = frame.ExtendedPayloadLengthWidth;
            if (len == 0)
            {
                frame.ExtendedPayloadLength = EmptyBytes;
                return frame;
            }

            var bytes = stream.ReadBytes(len);
            if (bytes.Length != len)
            {
                throw new InvalidOperationException("The extended payload length of a frame could not be read.");
            }

            frame.ExtendedPayloadLength = bytes;
            return frame;
        }

        private static string Print(WebSocketFrame frame)
        {
            // Payload Length
            var payloadLen = frame.PayloadLength;

            // Extended Payload Length
            var extPayloadLen = payloadLen > 125
                                ? frame.ExactPayloadLength.ToString()
                                : string.Empty;

            // Masking Key
            var maskingKey = BitConverter.ToString(frame.MaskingKey);

            // Payload Data
            var payload = payloadLen == 0
                          ? string.Empty
                          : payloadLen > 125
                            ? "---"
                            : !frame.IsText
                              || frame.IsFragment
                              || frame.IsMasked
                              || frame.IsCompressed
                              ? frame.PayloadData.ToString()
                              : Utf8Decode(frame.PayloadData.ApplicationData);

            var fmt = @"
                    FIN: {0}
                   RSV1: {1}
                   RSV2: {2}
                   RSV3: {3}
                 Opcode: {4}
                   MASK: {5}
         Payload Length: {6}
Extended Payload Length: {7}
            Masking Key: {8}
           Payload Data: {9}";

            return string.Format(
                     fmt,
                     frame.Fin,
                     frame.Rsv1,
                     frame.Rsv2,
                     frame.Rsv3,
                     frame.Opcode,
                     frame.Mask,
                     payloadLen,
                     extPayloadLen,
                     maskingKey,
                     payload
                   );
        }

        private static string Utf8Decode(byte[] bytes)
        {
            try
            {
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }
        }

        private static WebSocketFrame ProcessHeader(byte[] header)
        {
            if (header.Length != 2)
            {
                throw new InvalidOperationException("The header part of a frame could not be read.");
            }

            // FIN
            var fin = (header[0] & 0x80) == 0x80 ? Fin.Final : Fin.More;

            // RSV1
            var rsv1 = (header[0] & 0x40) == 0x40 ? Rsv.On : Rsv.Off;

            // RSV2
            var rsv2 = (header[0] & 0x20) == 0x20 ? Rsv.On : Rsv.Off;

            // RSV3
            var rsv3 = (header[0] & 0x10) == 0x10 ? Rsv.On : Rsv.Off;

            // Opcode
            var opcode = (byte)(header[0] & 0x0f);

            // MASK
            var mask = (header[1] & 0x80) == 0x80 ? Mask.On : Mask.Off;

            // Payload Length
            var payloadLen = (byte)(header[1] & 0x7f);

            if (!opcode.IsSupported())
            {
                throw new InvalidOperationException("A frame has an unsupported opcode.");
            }

            if (!opcode.IsData() && rsv1 == Rsv.On)
            {
                throw new InvalidOperationException("A non data frame is compressed.");
            }

            if (opcode.IsControl())
            {
                if (fin == Fin.More)
                {
                    throw new InvalidOperationException("A control frame is fragmented.");
                }

                if (payloadLen > 125)
                {
                    throw new InvalidOperationException("A control frame has too long payload length.");
                }
            }

            var frame = new WebSocketFrame()
            {
                Fin = fin,
                Rsv1 = rsv1,
                Rsv2 = rsv2,
                Rsv3 = rsv3,
                Opcode = (Opcode)opcode,
                Mask = mask,
                PayloadLength = payloadLen
            };

            return frame;
        }

        public byte[] ToArray()
        {
            using (var buff = new MemoryStream())
            {
                var header = (int)Fin;
                header = (header << 1) + (int)Rsv1;
                header = (header << 1) + (int)Rsv2;
                header = (header << 1) + (int)Rsv3;
                header = (header << 4) + (int)Opcode;
                header = (header << 1) + (int)Mask;
                header = (header << 7) + (int)PayloadLength;

                buff.Write(
                  ((ushort)header).InternalToByteArray(ByteOrder.Big), 0, 2
                );

                if (PayloadLength > 125)
                    buff.Write(ExtendedPayloadLength, 0, PayloadLength == 126 ? 2 : 8);

                if (Mask == Mask.On)
                    buff.Write(MaskingKey, 0, 4);

                if (PayloadLength > 0)
                {
                    var bytes = PayloadData.ToArray();

                    if (PayloadLength < 127)
                        buff.Write(bytes, 0, bytes.Length);
                    else
                        buff.WriteBytes(bytes, 1024);
                }

                buff.Close();
                return buff.ToArray();
            }
        }

        public string PrintToString()
        {
            return Print(this);
        }

        internal static WebSocketFrame CreateCloseFrame(PayloadData payloadData, bool mask)
        {
            return new WebSocketFrame(
                     Fin.Final, Opcode.Close, payloadData, false, mask
                   );
        }

        internal static WebSocketFrame CreatePingFrame(bool mask)
        {
            return new WebSocketFrame(
                     Fin.Final, Opcode.Ping, PayloadData.Empty, false, mask
                   );
        }

        internal static WebSocketFrame CreatePingFrame(byte[] data, bool mask)
        {
            return new WebSocketFrame(
                     Fin.Final, Opcode.Ping, new PayloadData(data), false, mask
                   );
        }

        internal static WebSocketFrame CreatePongFrame(
          PayloadData payloadData, bool mask
        )
        {
            return new WebSocketFrame(
                     Fin.Final, Opcode.Pong, payloadData, false, mask
                   );
        }

        internal static WebSocketFrame ReadFrame(Stream stream, bool unmask)
        {
            var frame = ReadHeader(stream);
            ReadExtendedPayloadLength(stream, frame);
            ReadMaskingKey(stream, frame);
            ReadPayloadData(stream, frame);

            if (unmask)
                frame.Unmask();

            return frame;
        }

        internal void Unmask()
        {
            if (Mask == Mask.Off)
                return;

            Mask = Mask.Off;
            PayloadData.Mask(MaskingKey);
            MaskingKey = EmptyBytes;
        }
    }
}
