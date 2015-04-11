using System;
using System.Net;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// MX (Mail Exchange) Resource Record (RFC 1035 3.3.9)
    /// </summary>
    [Serializable]
    public class MXRecord : RecordBase, IComparable
    {
        private readonly int _preference;
        private readonly string _domainName;

        #region Public Properties

        /// <summary>
        /// A &lt;domain-name&gt; which specifies a host willing to act as
        /// a mail exchange for the owner name.
        /// </summary>
        public int Preference 
        {
            get { return _preference; }
        }

        /// <summary>
        /// A 16 bit integer which specifies the preference given to
        /// this RR among others at the same owner.  Lower values
        /// are preferred.
        /// </summary>
        public string DomainName
        {
            get { return _domainName; }
        }

        #endregion

        internal MXRecord(DnsReader br)
        {
            _preference = br.ReadInt16();
            _domainName = br.ReadDomain();
        }

        public override string ToString()
        {
            return "    MX preference =" + _preference + ", mail exchanger = " + _domainName;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            MXRecord mxOther = (MXRecord)obj;

            if (mxOther._preference < _preference) return 1;
            if (mxOther._preference > _preference) return -1;

            return -mxOther._domainName.CompareTo(_domainName);
        }

        public static bool operator == (MXRecord record1, MXRecord record2)
        {
            return record1.Equals(record2);
        }

        public static bool operator != (MXRecord record1, MXRecord record2)
        {
            return !(record1 == record2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            MXRecord mxOther = obj as MXRecord;

            if (mxOther == null)
                return false;

            if (mxOther._preference != _preference)
                return false;

            if (mxOther._domainName != _domainName)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return _preference;
        }

        #endregion
    }
}