using System.Collections;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public static class HubClientsExtensions
    {
        public static void SendText(this WebSocketClients clients)
        {
            clients.SendText();
        }

        public static void SendBinary(this WebSocketClients clients)
        {
            clients.SendBinary();
        }
    }
}
