using Bytewizer.TinyCLR.Sockets.Channel;
using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public partial class Clients : IClientsAll
    {
        /// <summary>
        /// Provides methods to access caller operations.
        /// </summary>
        public IClientsAll All { get { return this; } }

        void IClientsAll.SendText(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            All.SendText(buffer, buffer.Length);
        }

        void IClientsAll.SendText(byte[] buffer, long length)
        {
            var payload = new PayloadData(buffer, length);
            var frame = new WebSocketFrame(Fin.Final, Opcode.Text, payload, false, false);

            foreach (SocketChannel session in _channels.Values)
            {
                session.OutputStream.Write(frame.ToArray());
            }
        }
    }
}
