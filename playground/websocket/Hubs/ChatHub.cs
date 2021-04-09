using System;

using Bytewizer.TinyCLR.Http.WebSockets;

namespace Bytewizer.Playground.WebSocket
{
    public class ChatHub : Hub
    {
        public override void OnConnected()
        {
        }

        public override void OnDisconnected(Exception exception)
        {
        }

        public void SendMessage(string user, string message)
        {
            Clients.SendText();
        }
    }
}
