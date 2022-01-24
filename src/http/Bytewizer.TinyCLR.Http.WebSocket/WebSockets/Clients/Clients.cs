using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public partial class Clients
    {
        private readonly Hashtable _channels;
        private readonly SocketChannel _caller;

        public Clients(Hashtable channels, SocketChannel caller)
        {
            _channels = channels;
            _caller = caller;
        }

        public void SendText(string[] id, string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            SendText(id, buffer, buffer.Length);
        }

        public void SendText(string[] id, byte[] buffer, long length)
        {
            var payload = new PayloadData(buffer, length);
            var frame = new WebSocketFrame(Fin.Final, Opcode.Text, payload, false, false);

            foreach (string item in id)
            {
                if (TryGetSession(item, out Stream outputStream))
                {
                    outputStream.Write(frame.ToArray());
                }
            }
        }

        public bool TryGetSession(string id, out Stream outputStream)
        {
            outputStream = default;

            if (_channels == null)
            {
                return false;
            }

            if (_channels.Contains(id))
            {
                outputStream = ((SocketChannel)_channels[id]).OutputStream;
                return true;
            }

            return false;
        }
    }
}




        //private bool TryGetOtherSessions(string key, out Stream[] sessions)
        //{
        //    sessions = default;

        //    if (_channels == null)
        //    {
        //        return false;
        //    }

        //    if (_channels.Contains(key))
        //    {

        //        for (int i = 0; i < _channels.Count; i++)
        //        {
        //            if ()
        //            _channels.Values.CopyTo(sessions, i);
        //        }

        //        return sessions;



        //        sessions = new Stream[] { ((SocketChannel)_channels[key]).OutputStream };
        //        return true;
        //    }

        //    return false;
        //}




        //private void SendText(Stream sessions, string text)
        //{
        //    var buffer = Encoding.UTF8.GetBytes(text);
        //    SendText(sessions, buffer, buffer.Length);
        //}

        //private void SendText(Stream[] sessions, byte[] buffer, long length)
        //{
        //    var payload = new PayloadData(buffer, length);
        //    var frame = new WebSocketFrame(Fin.Final, Opcode.Text, payload, false, false);

        //    foreach (Stream session in sessions)
        //    {
        //        SendText(session, buffer, length);
        //    }
        //}

        //private void SendText(Stream session, byte[] buffer, long length)
        //{
        //    var payload = new PayloadData(buffer, length);
        //    var frame = new WebSocketFrame(Fin.Final, Opcode.Text, payload, false, false);
        //    session.Write(frame.ToArray());
        //}
//    }
//}

//private void SendBinary(Stream[] sessions, byte[] buffer, long length)
//{
//    var payload = new PayloadData(buffer, length);
//    var frame = new WebSocketFrame(Fin.Final, Opcode.Binary, payload, false, false);
//    WriteSessions(sessions, frame);
//}
