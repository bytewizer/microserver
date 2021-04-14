using System;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public interface IHub
    {

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        void OnConnected();

        /// <summary>
        /// Called when a new message is sent to the hub.
        /// </summary>
        void OnMessage(byte[] payload);

        /// <summary>
        /// Called when a connection with the hub is terminated.
        /// </summary>
        void OnDisconnected(Exception exception);

    }
}