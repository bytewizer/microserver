using System;

using MicroServer.Utilities;
using MicroServer.Extensions;

namespace MicroServer.Net.Dhcp
{
    public class BindingLease 
    {
        private static readonly Random RANDOM = new Random();

        private string _clientId;
        private PhysicalAddress _owner;
        private InternetAddress _address;
        private byte[] _hostName;
        private DateTime _expiration;
        private uint _sessionId;
        private LeaseState _state;

        public string ClientId
        {
            get { return this._clientId; }
            set { this._clientId = value; }
        }

        public PhysicalAddress Owner
        {
            get { return this._owner; }
            set { this._owner = value; }
        }

        public InternetAddress Address
        {
            get { return this._address; }
            set { this._address = value; }
        }

        public Byte[] HostName
        {
            get { return this._hostName; }
            set { this._hostName = value; }
        }

        public DateTime Expiration
        {
            get { return this._expiration; }
            set { this._expiration = value; }
        }

        public uint SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        public LeaseState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public BindingLease(PhysicalAddress owner, InternetAddress address)
            : this(RANDOM.Next(int.MaxValue).ToString(), owner, address, new byte[0], DateTime.Now.AddDays(7), 0, LeaseState.Unassigned)
        {
        }

        public BindingLease(string clientId, PhysicalAddress owner, InternetAddress address, byte[] hostName, DateTime expiration, UInt32 sessionId, LeaseState state)
        {
            this._clientId = clientId;
            this._owner = owner;
            this._address = address;
            this._hostName = hostName;
            this._expiration = expiration;
            this._sessionId = sessionId;
            this._state = state;
        }

        public override string ToString()
        {
            string response = "CID: " + this.ClientId.ToString() +
                        " | SESID: " + this.SessionId.ToHexString("0x") +
                        " | IP: " + this.Address.ToString() +
                        " | OWNER: " + this.Owner.ToString() +
                        " | NAME: " + ByteUtility.GetSafeString(_hostName) +
                        " | EXPN: " + this.Expiration.ToLocalTime().ToString() +
                        " | STATE: " + LeaseStateString.GetName(_state);     
            return response;
        }
    }
}
