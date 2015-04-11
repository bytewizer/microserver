using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// SOA Resource Record (RFC 1035 3.3.13)
    /// </summary>
    [Serializable]
    public class SOARecord : RecordBase
    {
        private readonly string _mname;
        private readonly string _rname;
        private readonly int _serial;
        private readonly int _refresh;
        private readonly int _retry;
        private readonly int _expire;
        private readonly int _minimumTtl;

        #region Public Properties

        /// <summary>
        /// The &lt;domain-name&gt; of the name server that was the
        /// original or primary source of data for this zone.
        /// </summary>
        public string PrimaryNameServer
        {
            get { return _mname; }
        }

        /// <summary>
        /// A &lt;domain-name&gt; which specifies the mailbox of the
        /// person responsible for this zone.
        /// </summary>
        public string ResponsiblMailAddress
        {
            get { return _rname; }
        }

        /// <summary>
        /// The unsigned 32 bit version number of the original copy
        /// of the zone.  Zone transfers preserve this value.  This
        /// value wraps and should be compared using sequence space
        /// arithmetic.
        /// </summary>
        public int Serial
        {
            get { return _serial; }
        }

        /// <summary>
        /// A 32 bit time interval before the zone should be
        /// refreshed.
        /// </summary>
        public int Refresh
        {
            get { return _refresh; }
        }

        /// <summary>
        /// A 32 bit time interval that should elapse before a
        /// failed refresh should be retried.
        /// </summary>
        public int Retry
        {
            get { return _retry; }
        }

        /// <summary>
        /// A 32 bit time value that specifies the upper limit on
        /// the time interval that can elapse before the zone is no
        /// longer authoritative.
        /// </summary>
        public int Expire
        {
            get { return _expire; }
        }

        /// <summary>
        /// The unsigned 32 bit minimum TTL field that should be
        /// exported with any RR from this zone.
        /// </summary>
        public int DefaultTtl
        {
            get { return _minimumTtl; }
        }

        #endregion

        internal SOARecord(DnsReader br)
        {
            _mname = br.ReadDomain();
            _rname = br.ReadDomain();
            _serial = br.ReadInt32();
            _refresh = br.ReadInt32();
            _retry = br.ReadInt32();
            _expire = br.ReadInt32();
            _minimumTtl = br.ReadInt32();
        }

        internal string GetDuration(int d)
        {
            return d.ToString();
        }

        public override string ToString()
        {
            return @"    primary name server = " + _mname + @"
                responsible mail addr = " + _rname + @"
                serial  = " + _serial.ToString() + @"
                refresh = {3} {7}
                retry   = {4} {8}
                expire  = {5} {9}
                default TTL = {6} {10}";
        }
    }
}