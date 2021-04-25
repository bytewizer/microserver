using System;
using System.Text;
using System.Diagnostics;

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
            Debug.WriteLine(Caller.ConnectionId);
            Debug.WriteLine(Caller.GetHttpContext().Request.ToString());
        }

        public override void OnMessage(WebSocketContext context)
        {
            if (context.IsText)
            {
                var encodedPayload = Encoding.UTF8.GetString(context.Payload);
                Clients.Caller.SendText($"ECHO: {encodedPayload}");
            }
        }

        public override void OnDisconnected(Exception exception)
        {
            Debug.WriteLine(exception.Message);
        }
    }
}
