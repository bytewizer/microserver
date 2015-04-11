
using System;
using System.Collections;

using MicroServer.Collections;

namespace MicroServer.Net.Dhcp
{
    public class ReservationCollection : HashTableCollection
    {
        public ReservationCollection()
        {
            this._hashTable = new Hashtable();
        }

        public InternetAddress this[PhysicalAddress macAddress]
		{
            get { return (InternetAddress)this._hashTable[macAddress]; }
            set { this._hashTable[macAddress] = value; }
		}

        public void Add(InternetAddress ipAddress, PhysicalAddress macAddress)
		{
            this._hashTable[ipAddress] = macAddress;
		}

        public void Add(String IpAddress, String macAddress)
        {
            this._hashTable.Add(PhysicalAddress.Parse(macAddress), InternetAddress.Parse(IpAddress));
        }

        public bool Contains(PhysicalAddress macAddress)
		{
			return this._hashTable.Contains(macAddress);
		}

		public void Remove(PhysicalAddress macAddress)
		{
			this._hashTable.Remove(macAddress);
		}
    }
}
