using System;
using System.Diagnostics;
using System.Text;
using Bytewizer.TinyCLR.Http.WebSockets;

namespace Bytewizer.Playground.WebSocket
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {
            Debug.WriteLine("Hit Constructor");
        }

        public override void OnConnected()
        {
            var context = Context.GetHttpContext();
            Debug.WriteLine(context.Request.ToString());

            Debug.WriteLine(Context.ConnectionId);
        }

        public override void OnMessage(byte[] payload)
        {
            var encodedPayload = Encoding.UTF8.GetString(payload);
            //Clients.All.SendText($"ECHO: {encodedPayload}");
        }

        public override void OnDisconnected(Exception exception)
        {
            Debug.WriteLine(exception.Message);
        }
    }
}
