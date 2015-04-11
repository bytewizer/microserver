using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Text;

using MicroServer.IO;
using MicroServer.Utilities;
using MicroServer.Extensions;
using Microsoft.SPOT;

namespace MicroServer.Net.Dns
{
    #region RFC Specification

    /* Description: Object that represents a DNSv4 message as defined in
    *               RFC 2131.
    *               
    *  The following diagram illustrates the header format of DNS messages sent
    *  between server and clients as defined by 4.1.1:
    *
    *    						       1  1  1  1  1  1
	*    0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    *  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    *  |                      ID                       |
    *  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    *  |QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |
    *  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    *  |                    QDCOUNT                    |
    *  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    *  |                    ANCOUNT                    |
    *  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    *  |                    NSCOUNT                    |
    *  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    *  |                    ARCOUNT                    |
    *  +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    *  
    *  where:
    *  ID              A 16 bit identifier assigned by the program that
	*  			    generates any kind of query.  This identifier is copied
	*  			    the corresponding reply and can be used by the requester
	*  			    to match up replies to outstanding queries.
    *  			    
    *  QR              A one bit field that specifies whether this message is a
	*  			    query (0), or a response (1).
    *  
    *  OPCODE       A four bit field that specifies kind of query in this
	*  			    message.  This value is set by the originator of a query
	*  			    and copied into the response.  The values are:
    *  
	*  			    0               a standard query (QUERY)
	*  			    1               an inverse query (IQUERY)  
	*  			    2               a server status request (STATUS)
	*  			    3-15            reserved for future use
    *  
    *  AA           Authoritative Answer - this bit is valid in responses,
	*  			    and specifies that the responding name server is an
	*  			    authority for the domain name in question section.
    *  
	*  			    Note that the contents of the answer section may have
	*  			    multiple owner names because of aliases.  The AA bit
	*  			    corresponds to the name which matches the query name, or
	*  			    the first owner name in the answer section.
    *  
    *  TC           TrunCation - specifies that this message was truncated
	*  			    due to length greater than that permitted on the
	*  			    transmission channel.
    *  
    *  RD           Recursion Desired - this bit may be set in a query and
	*  			    is copied into the response.  If RD is set, it directs
	*  			    the name server to pursue the query recursively.
	*  			    Recursive query support is optional.
    *  
    *  RA           Recursion Available - this be is set or cleared in a
	*  			    response, and denotes whether recursive query support is
	*  			    available in the name server.
    *  
    *  Z            Reserved for future use.  Must be zero in all queries
	*  			    and responses.
    *  
    *  RCODE        Response code - this 4 bit field is set as part of
	*  			    responses.  The values have the following
	*  			    interpretation:
    *  
	*  			    0               No error condition
	*  			    1               Format error - The name server was
	*  							    unable to interpret the query.
	*  			    2               Server failure - The name server was
	*  							    unable to process this query due to a
	*  							    problem with the name server.
	*  			    3               Name Error - Meaningful only for
	*  							    responses from an authoritative name
	*  							    server, this code signifies that the
	*  							    domain name referenced in the query does
	*  							    not exist.
	*  			    4               Not Implemented - The name server does
	*  							    not support the requested kind of query.
	*  			    5               Refused - The name server refuses to
	*  							    perform the specified operation for
	*  							    policy reasons.  For example, a name
	*  							    server may not wish to provide the
	*  							    information to the particular requester,
	*  							    or a name server may not wish to perform
	*  							    a particular operation (e.g., zone
	*  							    transfer) for particular data.
	*  			    6-15            Reserved for future use.
    *  
    *  QDCOUNT      an unsigned 16 bit integer specifying the number of
	*  			    entries in the question section.
    *  
    *  ANCOUNT      an unsigned 16 bit integer specifying the number of
	*  			    resource records in the answer section.
    *  
    *  NSCOUNT      an unsigned 16 bit integer specifying the number of name
	*  			    server resource records in the authority records
	*  			    section.
    *  
    *  ARCOUNT      an unsigned 16 bit integer specifying the number of
	*  			    resource records in the additional records section.
    *  
    */

    #endregion RFC Specification

    public class DnsMessage
    {
        #region Private Properties

        private static readonly Random RANDOM = new Random();

        #endregion Private Properties

        #region Constructors

        public DnsMessage()
        {

            Timestamp = DateTime.Now;
            Header = new Header();
            Header.TransactionId = (ushort)RANDOM.Next(ushort.MaxValue);
            Header.QR = false;
            Header.OperationCode = OperationCode.Query;

            Questions = new Question[0];
            Answers = new Answer[0];
            Authorities = new Authority[0];
            Additionals = new Additional[0];

        }

        public DnsMessage(byte[] data)
        {
            Timestamp = DateTime.Now;
            DnsReader byteReader = new DnsReader(data);

            // Header
            Header = new Header(byteReader);

            // Question, Answer, Authority, Additional Counts
            Questions = new Question[this.QuestionRecordCount];
            Answers = new Answer[this.AnswerRecordCount];
            Authorities = new Authority[this.AuthorityRecordCount];
            Additionals = new Additional[this.AdditionalRecordCount];

            // Read Records
            for (int i = 0; i < this.QuestionRecordCount; i++)
                this.Questions[i] = new Question(byteReader);

            for (int i = 0; i < this.AnswerRecordCount; i++)
                this.Answers[i] = new Answer(byteReader);

            for (int i = 0; i < this.AuthorityRecordCount; i++)
                this.Authorities[i] = new Authority(byteReader);

            for (int i = 0; i < this.AdditionalRecordCount; i++)
                this.Additionals[i] = new Additional(byteReader);

        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// TimeStamp when cached.
        /// </summary>
        public DateTime Timestamp;

        /// <summary>
        /// Header record
        /// </summary>
        public Header Header;

        /// <summary>
		/// List of Question records.
		/// </summary>
        public Question[] Questions;

		/// <summary>
		/// List of AnswerRR records.
		/// </summary>
		public Answer[] Answers;

		/// <summary>
		/// List of AuthorityRR records.
		/// </summary>
		public Authority[] Authorities;

		/// <summary>
		/// List of AdditionalRR records.
		/// </summary>
		public Additional[] Additionals;

        /// <summary>
        /// An identifier assigned by the service.
        /// </summary>
        public UInt16 TransactionId
        {
            get { return this.Header.TransactionId; }
            set { this.Header.TransactionId = value; }
        }

        /// <summary>
        /// the number of entries in the question section.
        /// </summary>
        public Int32 QuestionRecordCount
        {
            get { return this.Header.QDCOUNT; }
            set { this.Header.QDCOUNT = (ushort)value; }
        }

        /// <summary>
        /// The number of resource records in the answer section.
        /// </summary>
        public int AnswerRecordCount
        {
            get { return this.Header.ANCOUNT; }
            set { this.Header.ANCOUNT = (ushort)value; }
        }

        /// <summary>
        /// The number of name server resource records in the authority records section.
        /// </summary>
        public int AuthorityRecordCount
        {
            get { return this.Header.NSCOUNT; }
            set { this.Header.NSCOUNT = (ushort)value; }
        }

        /// <summary>
        /// The number of resource records in the additional records section.
        /// </summary>
        public int AdditionalRecordCount
        {
            get { return this.Header.ARCOUNT; }
            set { this.Header.ARCOUNT = (ushort)value; }
        }

        /// <summary>
        /// Is authoritative answer (AA).
        /// </summary>
        public bool IsAuthoritiveAnswer
        {
            get { return this.Header.AuthoritiveAnswer; }
            set { this.Header.AuthoritiveAnswer = (bool)value; }
        }

        /// <summary>
        /// Is Query(true) or Response(false) flag (QR).
        /// </summary>
        public bool IsQuery
        {
            get { return this.Header.QR; }
            set { this.Header.QR = (bool)value; }
        }

        /// <summary>
        /// Is Recursion Available (AA).
        /// </summary>
        public bool IsRecursionAvailable
        {
            get { return this.Header.RecursionAvailable; }
            set { this.Header.RecursionAvailable = value; }
        }

        /// <summary>
        /// Is Recursion Desired (RD).
        /// </summary>
        public bool IsRecursionDesired
        {
            get { return this.Header.RecursionDesired; }
            set { this.Header.RecursionDesired = value; }
        }

        /// <summary>
        /// TrunCation (TC).
        /// </summary>
        public bool IsTruncated
        {
            get { return this.Header.Truncated; }
            set { this.Header.Truncated = value; }
        }

        /// <summary>
        /// Specifies type of query (OPCODE).
        /// </summary>
        public OperationCode OperationCode
        {
            get { return this.Header.OperationCode; }
            set { this.Header.OperationCode = value; }
        }

        /// <summary>
        /// Specifies the response code (RCODE).
        /// </summary>
        public ReturnCode ResponseCode
        {
            get { return this.Header.ResponseCode; }
            set { this.Header.ResponseCode = value; }
        }

        #endregion Public Properties

        #region Methods

        public byte[] ToArray ()
        { 
            DnsWriter byteWriter = new DnsWriter();

            // Header
            Header.WriteBytes(byteWriter);

            // Question, Answer, Authority, Additional Counts
            byteWriter.Write((short)this.QuestionRecordCount);
            byteWriter.Write((short)this.AnswerRecordCount);
            byteWriter.Write((short)this.AuthorityRecordCount);
            byteWriter.Write((short)this.AdditionalRecordCount);

            // Write Records
            foreach (Question record in Questions)
            {
                record.Write(byteWriter);
            }

            foreach (Answer record in Answers)
            {
                record.Write(byteWriter);
            }

            foreach (Authority record in Authorities)
            {
                record.Write(byteWriter);
            }

            foreach (Additional record in Additionals)
            {
                record.Write(byteWriter);
            }

            return byteWriter.GetBytes();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("  DNS PACKET");
            sb.AppendLine(string.Concat("  Message Timestamp              : ", this.Timestamp.ToLocalTime().ToString()));
            sb.AppendLine(string.Concat("  Transaction Id (ID)            : ", this.TransactionId.ToHexString("0x")));
            sb.AppendLine(string.Concat("  Is Query (RQ)                  : ", this.IsQuery.ToString()));
            sb.AppendLine(string.Concat("  Operation Code (Opcode)        : ", this.OperationCode.ToString()));
            sb.AppendLine(string.Concat("  Is Authoritive Answer (AA)     : ", this.IsAuthoritiveAnswer.ToString()));
            sb.AppendLine(string.Concat("  Is Truncated (TC)              : ", this.IsTruncated.ToString()));
            sb.AppendLine(string.Concat("  Is Recursion Desired (RD)      : ", this.IsRecursionDesired.ToString()));
            sb.AppendLine(string.Concat("  Is Recursion Available (RA)    : ", this.IsRecursionAvailable.ToString()));
            sb.AppendLine(string.Concat("  Response Code (RCODE)          : ", this.ResponseCode.ToString()));

            sb.AppendLine(string.Concat("  Question Count (QDCOUNT)       : ", this.QuestionRecordCount.ToString()));
            foreach (Question question in this.Questions)
            {
                Question record = question as Question;
                if (record != null)
                {
                    sb.AppendLine(string.Concat("  -Question Record (Name)        : ", record.Domain.ToString()));
                    sb.AppendLine(string.Concat("  -Class                         : ", record.Class.ToString()));
                    sb.AppendLine(string.Concat("  -Type                          : ", record.Type.ToString()));
                }
            }

            sb.AppendLine(string.Concat("  Answer Count (ANCOUNT)         : ", this.AnswerRecordCount.ToString()));
            foreach (Answer answer in this.Answers)
            {
                Answer record = answer as Answer;
                if (record != null)
                {
                    sb.AppendLine(string.Concat("  -Answer Record (Name)          : ", record.Domain.ToString()));
                    sb.AppendLine(string.Concat("  -Class                         : ", record.Class.ToString()));
                    sb.AppendLine(string.Concat("  -Type                          : ", record.Type.ToString()));
                    sb.AppendLine(string.Concat("  -Ttl                           : ", record.Ttl.ToString()));
                    sb.AppendLine(string.Concat("  -Record                        : ", record.Record.ToString()));
                }
            }

            sb.AppendLine(string.Concat("  Additional Count (ARCOUNT)     : ", this.AdditionalRecordCount.ToString()));
            foreach (Additional additional in this.Additionals)
            {
                Additional record = additional as Additional;
                if (record != null)
                {
                    sb.AppendLine(string.Concat("  -Additional Record (Name)      : ", record.Domain.ToString()));
                    sb.AppendLine(string.Concat("  -Class                         : ", record.Class.ToString()));
                    sb.AppendLine(string.Concat("  -Type                          : ", record.Type.ToString()));
                    sb.AppendLine(string.Concat("  -Ttl                           : ", record.Ttl.ToString()));
                    sb.AppendLine(string.Concat("  -Record                        : ", record.Record.ToString()));
                }
            }

            sb.AppendLine(string.Concat("  Authority Count (NACOUNT)      : ", this.AuthorityRecordCount.ToString()));
            foreach (Authority authority in this.Authorities)
            {
                Authority record = authority as Authority;
                if (record != null)
                {
                    sb.AppendLine(string.Concat("  -Authority Record (Name)       : ", record.Domain.ToString()));
                    sb.AppendLine(string.Concat("  -Class                         : ", record.Class.ToString()));
                    sb.AppendLine(string.Concat("  -Type                          : ", record.Type.ToString()));
                    sb.AppendLine(string.Concat("  -Ttl                           : ", record.Ttl.ToString()));
                    sb.AppendLine(string.Concat("  -Record                        : ", record.Record.ToString()));
                }
            }

            sb.AppendLine();
            return sb.ToString();
        }

        #endregion Methods
    }
}
