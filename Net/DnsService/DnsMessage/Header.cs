using System;
using MicroServer.IO;
using MicroServer.Utilities;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// Header (RFC 1035 4.1.1, 1002 4.2.1)
    /// </summary>
    public class Header
    {
        private ushort _transactionID; // ID
        private ushort _flags; // QR, Opcode, AA, TC, RD, RA, Z, RCODE
        private ushort _numQuestions; // QDCOUNT
        private ushort _numAnswers; // ANCOUNT
        private ushort _numAuthorities;  //NSCOUNT
        private ushort _numAdditionals;  //ARCOUNT

        #region Public Properties

        /// <summary>
        /// ID - A 16 bit identifier assigned by the program that
        /// generates any kind of query.  This identifier is copied
        /// the corresponding reply and can be used by the requester
        /// to match up replies to outstanding queries.
        /// </summary>
        public ushort TransactionId
        {
            get { return _transactionID; }
            internal set { _transactionID = value; }
        }

        /// <summary>
        /// QDCOUNT - An unsigned 16 bit integer specifying the number of
        /// entries in the question section.
        /// </summary>
        public ushort QDCOUNT
        {
            get { return _numQuestions; }
            internal set { _numQuestions = value; }
        }

        /// <summary>
        /// ANCOUNT - An unsigned 16 bit integer specifying the number of
        /// resource records in the answer section.
        /// </summary>
        public ushort ANCOUNT
        {
            get { return _numAnswers; }
            internal set { _numAnswers = value; }
        }

        /// <summary>
        /// NSCOUNT - An unsigned 16 bit integer specifying the number of name
        /// server resource records in the authority records
        /// section.
        /// </summary>
        public ushort NSCOUNT
        {
            get { return _numAuthorities; }
            internal set { _numAuthorities = value; }
        }

        /// <summary>
        /// ARCOUNT - An unsigned 16 bit integer specifying the number of
        /// resource records in the additional records section.
        /// </summary>
        public ushort ARCOUNT
        {
            get { return _numAdditionals; }
            internal set { _numAdditionals = value; }
        }
        
        /// <summary>
        /// QR - A one bit field that specifies whether this message is a
        /// query (true), or a response (false).
        /// </summary>
        public bool QR
        {
            get { return ByteUtility.GetBits(_flags, 15, 1) == 0; }
            set
            {
                if (value == true)
                {
                  _flags = ByteUtility.SetBits(ref _flags, 15, 1, 0);
                }
                else
                {
                    _flags = ByteUtility.SetBits(ref _flags, 15, 1, 1);
                }
            }
        }

        /// <summary>
        /// OPCODE - A four bit field that specifies kind of query in this
        /// message.  This value is set by the originator of a query
        /// and copied into the response.
        /// </summary>
        public OperationCode OperationCode  // OPCODE
        {
            get { return (OperationCode)ByteUtility.GetBits(_flags, 11, 4); }
            set { ByteUtility.SetBits(ref _flags, 11, 4, (ushort)value); }
        }

        /// <summary>
        /// AA - Authoritative Answer - this bit is valid in responses, 
        /// and specifies that the responding name server is an
        /// authority for the domain name in question section.
        /// 
        /// Note that the contents of the answer section may have
        /// multiple owner names because of aliases.  The AA bit
        /// corresponds to the name which matches the query name, or
        /// the first owner name in the answer section.
        /// </summary>
        public bool AuthoritiveAnswer //AA
        {
            get { return ByteUtility.GetBit(_flags, 10) == true; }
            set { ByteUtility.SetBit(ref _flags, 10, value); }
        }

        /// <summary>
        /// TC - TrunCation - specifies that this message was truncated
        /// due to length greater than that permitted on the
        /// transmission channel.
        /// </summary>
        public bool Truncated // TC
        {
            get { return ByteUtility.GetBit(_flags, 9) == true; }
            set { ByteUtility.SetBit(ref _flags, 9, value); }
        }

        /// <summary>
        /// RD - Recursion Desired - this bit may be set in a query and
        /// is copied into the response.  If RD is set, it directs
        /// the name server to pursue the query recursively.
        /// Recursive query support is optional.
        /// </summary>
        public bool RecursionDesired //RD
        {
            get { return ByteUtility.GetBit(_flags, 8) == true; }
            set { ByteUtility.SetBit(ref _flags, 8, value); }
        }

        /// <summary>
        /// RA - Recursion Available - this be is set or cleared in a
        /// response, and denotes whether recursive query support is
        /// available in the name server.
        /// </summary>
        public bool RecursionAvailable  //RA
        {
            get { return ByteUtility.GetBit(_flags, 7) == true; }
            set { ByteUtility.SetBit(ref _flags, 7, value); }
        }

        /// <summary>
        /// Reserved for future use.  Must be zero in all queries
        /// and responses.
        /// </summary>
        private ushort ZReserved
        {
            get { return ByteUtility.GetBits(_flags, 4, 3); }
            set { ByteUtility.SetBits(ref _flags, 4, 3, value); }
        }

        /// <summary>
        /// RCODE - Response code - this 4 bit field is set as part of
        /// responses.  The values have the following
        /// </summary>
        public ReturnCode ResponseCode  //RCODE
        {
            get { return (ReturnCode) ByteUtility.GetBits(_flags, 0, 4); }
            set { ByteUtility.SetBits(ref _flags, 0, 4, (ushort)value); }
        }

        #endregion

        public Header()
        {
            this.RecursionDesired = true;
        }

        internal Header(ByteReader br)
        {
            _transactionID = br.ReadUInt16();
            _flags = br.ReadUInt16();
            _numQuestions = br.ReadUInt16();
            _numAnswers = br.ReadUInt16();
            _numAuthorities = br.ReadUInt16();
            _numAdditionals = br.ReadUInt16();
        }

        internal void WriteBytes(ByteWriter bw)
        {
            bw.Write(_transactionID);
            bw.Write(_flags);
        }
    }
}
