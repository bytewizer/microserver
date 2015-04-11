using System;
using Microsoft.SPOT;
using System.Net;
using System.Text;
using System.Collections;

using Rover.Core.IO;
using Rover.Network.Dns.ResourceRecords;

namespace Rover.Network.Dns

{
    class Message
    {
        private static readonly Random RANDOM = new Random();

        private Header header;
        private Question[] questions;
        private IResourceRecord[] answers;
        private IResourceRecord[] authority;
        private IResourceRecord[] additional;

        public struct Header
        {
            public const int SIZE = 12;

            private ushort id;

            private byte flag0;
            private byte flag1;

            // Question count: number of questions in the Question section
            private ushort qdCount;

            // Answer record count: number of records in the Answer section
            private ushort anCount;

            // Authority record count: number of records in the Authority section
            private ushort nsCount;

            // Additional record count: number of records in the Additional section
            private ushort arCount;

            public int Id
            {
                get { return id; }
                set { id = (ushort)value; }
            }

            public int QuestionCount
            {
                get { return qdCount; }
                set { qdCount = (ushort)value; }
            }

            public int AnswerRecordCount
            {
                get { return anCount; }
                set { anCount = (ushort)value; }
            }

            public int AuthorityRecordCount
            {
                get { return nsCount; }
                set { nsCount = (ushort)value; }
            }

            public int AdditionalRecordCount
            {
                get { return arCount; }
                set { arCount = (ushort)value; }
            }

            public bool Response
            {
                get { return Qr == 1; }
                set { Qr = Convert.ToByte(value); }
            }

            public OperationCode OperationCode
            {
                get { return (OperationCode)Opcode; }
                set { Opcode = (byte)value; }
            }

            public bool AuthorativeServer
            {
                get { return Aa == 1; }
                set { Aa = Convert.ToByte(value); }
            }

            public bool Truncated
            {
                get { return Tc == 1; }
                set { Tc = Convert.ToByte(value); }
            }

            public bool RecursionDesired
            {
                get { return Rd == 1; }
                set { Rd = Convert.ToByte(value); }
            }

            public bool RecursionAvailable
            {
                get { return Ra == 1; }
                set { Ra = Convert.ToByte(value); }
            }

            public ResponseCode ResponseCode
            {
                get { return (ResponseCode)RCode; }
                set { RCode = (byte)value; }
            }

            public int Size
            {
                get { return Header.SIZE; }
            }

            public byte[] ToArray()
            {
                //return Marshalling.Struct.GetBytes(this);
            }

            // Query/Response Flag
            private byte Qr
            {
                get { return Flag0.GetBitValueAt(7, 1); }
                set { Flag0 = Flag0.SetBitValueAt(7, 1, value); }
            }

            // Operation Code
            private byte Opcode
            {
                get { return Flag0.GetBitValueAt(3, 4); }
                set { Flag0 = Flag0.SetBitValueAt(3, 4, value); }
            }

            // Authorative Answer Flag
            private byte Aa
            {
                get { return Flag0.GetBitValueAt(2, 1); }
                set { Flag0 = Flag0.SetBitValueAt(2, 1, value); }
            }

            // Truncation Flag
            private byte Tc
            {
                get { return Flag0.GetBitValueAt(1, 1); }
                set { Flag0 = Flag0.SetBitValueAt(1, 1, value); }
            }

            // Recursion Desired
            private byte Rd
            {
                get { return Flag0.GetBitValueAt(0, 1); }
                set { Flag0 = Flag0.SetBitValueAt(0, 1, value); }
            }

            // Recursion Available
            private byte Ra
            {
                get { return Flag1.GetBitValueAt(7, 1); }
                set { Flag1 = Flag1.SetBitValueAt(7, 1, value); }
            }

            // Zero (Reserved)
            private byte Z
            {
                get { return Flag1.GetBitValueAt(4, 3); }
                set { }
            }

            // Response Code
            private byte RCode
            {
                get { return Flag1.GetBitValueAt(0, 4); }
                set { Flag1 = Flag1.SetBitValueAt(0, 4, value); }
            }

            private byte Flag0
            {
                get { return flag0; }
                set { flag0 = value; }
            }

            private byte Flag1
            {
                get { return flag1; }
                set { flag1 = value; }
            }
        }

        public interface IResponse : IMessage
        {
            int Id { get; set; }
            ArrayList AnswerRecords { get; }
            ArrayList AuthorityRecords { get; }
            ArrayList AdditionalRecords { get; }
            bool RecursionAvailable { get; set; }
            bool AuthorativeServer { get; set; }
            bool Truncated { get; set; }
            OperationCode OperationCode { get; set; }
            ResponseCode ResponseCode { get; set; }
        }

        public interface IMessage
        {
            Question[] Questions { get; }

            int Size { get; }
            byte[] ToArray();
        }

        public class Domain
        {
            private string[] labels;

            public static Domain PointerName(IPAddress ip)
            {
                return new Domain(FormatReverseIP(ip));
            }

            private static string FormatReverseIP(IPAddress ip)
            {
                byte[] address = ip.GetAddressBytes();

                if (address.Length == 4)
                {
                    return string.Join(".", address.Reverse().Select(b => b.ToString())) + ".in-addr.arpa";
                }

                byte[] nibbles = new byte[address.Length * 2];

                for (int i = 0, j = 0; i < address.Length; i++, j = 2 * i)
                {
                    byte b = address[i];

                    nibbles[j] = b.GetBitValueAt(4, 4);
                    nibbles[j + 1] = b.GetBitValueAt(0, 4);
                }

                return string.Join(".", nibbles.Reverse().Select(b => b.ToString("x"))) + ".ip6.arpa";
            }

            public Domain(string domain) : this(domain.Split('.')) { }

            public Domain(string[] labels)
            {
                this.labels = labels;
            }

            public int Size
            {
                get { return labels.Sum(l => l.Length) + labels.Length + 1; }
            }

            public byte[] ToArray()
            {
                byte[] result = new byte[Size];
                int offset = 0;

                foreach (string label in labels)
                {
                    byte[] l = Encoding.UTF8.GetBytes(label);

                    result[offset++] = (byte)l.Length;
                    l.CopyTo(result, offset);

                    offset += l.Length;
                }

                result[offset] = 0;

                return result;
            }

            public int CompareTo(Domain other)
            {
                return ToString().CompareTo(other.ToString());
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is Domain))
                {
                    return false;
                }

                return CompareTo(obj as Domain) == 0;
            }
        }

        public class Question : IMessageEntry
        {
            private Domain domain;
            private RecordType type;
            private RecordClass klass;

            public Question(Domain domain, RecordType type = RecordType.A, RecordClass klass = RecordClass.IN)
            {
                this.domain = domain;
                this.type = type;
                this.klass = klass;
            }

            public Domain Name
            {
                get { return domain; }
            }

            public RecordType Type
            {
                get { return type; }
            }

            public RecordClass Class
            {
                get { return klass; }
            }

            public int Size
            {
                get { return domain.Size + Tail.SIZE; }
            }

            public byte[] ToArray()
            {
                ByteStream result = new ByteStream(Size);

                result
                    .Append(domain.ToArray())
                    .Append(Marshalling.Struct.GetBytes(new Tail { Type = Type, Class = Class }));

                return result.ToArray();
            }


            private struct Tail
            {
                public const int SIZE = 4;

                private ushort type;
                private ushort klass;

                public RecordType Type
                {
                    get { return (RecordType)type; }
                    set { type = (ushort)value; }
                }

                public RecordClass Class
                {
                    get { return (RecordClass)klass; }
                    set { klass = (ushort)value; }
                }
            }
        }

        public interface IMessageEntry
        {
            Domain Name { get; }
            RecordType Type { get; }
            RecordClass Class { get; }

            int Size { get; }
            byte[] ToArray();
        }

        public enum ResponseCode
        {
            NoError = 0,
            FormatError,
            ServerFailure,
            NameError,
            NotImplemented,
            Refused,
            YXDomain,
            YXRRSet,
            NXRRSet,
            NotAuth,
            NotZone,
        }

        public enum RecordClass
        {
            IN = 1,
            ANY = 255,
        }

        public enum RecordType
        {
            A = 1,
            NS = 2,
            CNAME = 5,
            SOA = 6,
            WKS = 11,
            PTR = 12,
            MX = 15,
            TXT = 16,
            AAAA = 28,
            SRV = 33,
            ANY = 255,
        }

        public enum OperationCode
        {
            Query = 0,
            IQuery,
            Status,
            // Reserved = 3
            Notify = 4,
            Update,
        }
    }
}
