using System;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public interface IHub
    {
        /// <summary>
        /// Called when a new message is sent to the hub.
        /// </summary>
        void OnMessage(WebSocketContext context);

    }
}