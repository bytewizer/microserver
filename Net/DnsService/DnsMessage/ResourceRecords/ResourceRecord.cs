using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// Represents a Resource Record (RFC1035 4.1.3)
    /// </summary>
    [Serializable]
    public class ResourceRecord
    {
        private string _domain;    // NAME
        private RecordType _qtype;
        private RecordClass _qclass;
        private int _ttl;
        private RecordBase _record;

        #region Public Properties

        /// <summary>
        /// A domain name to which this resource record pertains.
        /// </summary>
        public string Domain
        {
            get { return _domain; }
            internal set { _domain = value; }
        }
        
        public RecordType Type 
        { 
            get { return _qtype; }
            internal set { _qtype = value; }
        }
        
        public RecordClass Class 
        { 
            get { return _qclass; }
            internal set { _qclass = value; }
        }
        
        /// <summary>
        /// A 32 bit unsigned integer that specifies the time
        /// interval (in seconds) that the resource record may be
        /// cached before it should be discarded.  Zero values are
        /// interpreted to mean that the RR can only be used for the
        /// transaction in progress, and should not be cached.
        /// </summary>
        public int Ttl 
        { 
            get { return _ttl; }
            internal set { _ttl = value; }
        }

        /// <summary>
        /// RDATA
        /// </summary>
        public RecordBase Record 
        { 
            get { return _record; }
            internal set { _record = value; }
        }

        #endregion

        internal ResourceRecord()
        {
        }

        internal ResourceRecord(DnsReader br)
        {
            _domain = br.ReadDomain();
            _qtype = (RecordType)br.ReadInt16();
            _qclass = (RecordClass)br.ReadInt16();
            _ttl = br.ReadInt32();

            int recordLength = br.ReadInt16();
            if (recordLength != 0)
            {
                switch (_qtype)
                {
                    case RecordType.A:     _record = new ARecord(br);      break;
                    case RecordType.CNAME: _record = new CNAMERecord(br);  break;
                    case RecordType.MX:    _record = new MXRecord(br);     break;
                    case RecordType.NS:    _record = new NSRecord(br);     break;
                    case RecordType.SOA:   _record = new SOARecord(br);    break;
                    case RecordType.TXT:   _record = new TXTRecord(br);    break;
					case RecordType.PTR:   _record = new PTRERecord(br);	break;

                    // NetBIOS related records
                    case RecordType.NB:    _record = new NBRecord(br);     break;
                    
                    default:
                        br += recordLength;
                        break;
                }
            }
        }

        public override string ToString()
        {
            return _domain + " | " + _record.ToString();
        }

        internal void Write(DnsWriter bw)
        {

            byte[] record = Record.GetBytes();

            bw.WriteDomain(Domain);
            bw.Write((short)Type);
            bw.Write((short)Class);
            bw.Write(Ttl);
            bw.Write((short)record.Length);
            bw.Write(record);
        }
    }
}