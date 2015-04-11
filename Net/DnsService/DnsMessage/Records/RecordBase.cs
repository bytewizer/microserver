using System;

namespace MicroServer.Net.Dns
{
    public abstract class RecordBase
    {
        internal virtual byte[] GetBytes() { return new byte[0]; }
    }
}
