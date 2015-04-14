using System;

namespace MicroServer.Net.Dns
{
    public abstract class RecordBase
    {
        public virtual byte[] GetBytes() { return new byte[0]; }
    }
}
