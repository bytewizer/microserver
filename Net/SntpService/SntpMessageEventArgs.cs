using System;
using System.Net;
using Microsoft.SPOT;

using MicroServer.Logging;
using Microsoft.SPOT.Hardware;
using MicroServer.Net.Sockets;

namespace MicroServer.Net.Sntp
{
    public class SntpMessageEventArgs : EventArgs
    {
        /// <summary>
        ///     Channel for the connected client.
        /// </summary>
        public SocketChannel Channel { get; private set; }

        /// <summary>
        ///     Buffer for the connected client.
        /// </summary>
        public SocketBuffer ChannelBuffer { get; private set; }
        
        /// <summary>
        ///     Requested message for the connected client.
        /// </summary>
        public SntpMessage RequestMessage { get; private set; }
        
        /// <summary>
        ///     Response message for the connected client.
        /// </summary>
        public SntpMessage ResponseMessage { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SntpMessageEventArgs" /> class.
        /// </summary>
        /// <param name="channel">Socket channel request is recived on.</param>
        /// <param name="data">Raw data received from socket.</param>
        public SntpMessageEventArgs(SocketChannel channel, SocketBuffer data)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            this.Channel = channel;

            if (data == null) throw new ArgumentNullException("data");
            this.ChannelBuffer = data;

            try
            {
                // Parse the sntp message
                this.RequestMessage = new SntpMessage(data.Buffer);

                // log that the packet was successfully parsed
                Logger.WriteDebug(this, "PACKET with channel id " + this.Channel.ChannelId.ToString() +
                        " successfully parsed from client endpoint " + this.Channel.RemoteEndpoint.ToString());
                Logger.WriteDebug(this.RequestMessage.ToString());

            }
            catch (Exception ex)
            {
                Logger.WriteError(this, "Error parsing message:" + ex.Message.ToString(), ex);
                // disconnect ?
                return;
            }
        }
    }
}
