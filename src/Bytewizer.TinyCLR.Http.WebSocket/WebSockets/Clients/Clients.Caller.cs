using Bytewizer.TinyCLR.Http.Extensions;
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public partial class Clients : IClientsCaller
    {
        /// <summary>
        /// Provides methods to access caller operations.
        /// </summary>
        public IClientsCaller Caller { get { return this; } }

        void IClientsCaller.SendText(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            Caller.SendText(buffer, buffer.Length);
        }

        void IClientsCaller.SendText(byte[] buffer, long length)
        {           
            var payload = new PayloadData(buffer, length);
            var frame = new WebSocketFrame(Fin.Final, Opcode.Text, payload, false, false);
            _caller.OutputStream.Write(frame.ToArray());
        }
    }
}
