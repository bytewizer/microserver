using System;
using System.Collections;

using MicroServer.Collections;

namespace MicroServer.Net.Dhcp
{
    public class NameServerCollection : ArrayListCollection
    {

        public NameServerCollection()
        {
            this._arrayList = new ArrayList();
        }
        
        public NameServerCollection(String preferredAddress)
        {
            this._arrayList = new ArrayList();
            this.Add(new InternetAddress(preferredAddress));
        }

        public NameServerCollection(String preferredAddress, String alternetAddress)
        {
            this._arrayList = new ArrayList();
            this.Add(new InternetAddress(preferredAddress));
            this.Add(new InternetAddress(alternetAddress));
        }

        public int Add(InternetAddress item)
        {
            return this._arrayList.Add(item);
        }

        public int Add(string value)
        {
            return this._arrayList.Add(new InternetAddress(value));
        }

        public bool Contains(InternetAddress item)
        {
            return this._arrayList.Contains(item);
        }

        public void Remove(InternetAddress item)
        {
            this._arrayList.Remove(item);
        }
    }
}
