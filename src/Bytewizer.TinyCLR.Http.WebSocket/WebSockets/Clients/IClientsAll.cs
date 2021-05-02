using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public interface IClientsAll
    {
        void SendText(string text);

        void SendText(byte[] buffer, long length);
    }
}
