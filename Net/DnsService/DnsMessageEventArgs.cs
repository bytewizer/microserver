using System;
using System.Net;
using Microsoft.SPOT;
using System.Collections;

using MicroServer.Logging;
using MicroServer.Extensions;
using MicroServer.Net.Sockets;

namespace MicroServer.Net.Dns
{
    public class DnsMessageEventArgs : EventArgs
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
        public DnsMessage RequestMessage { get; private set; }

        /// <summary>
        ///     Response message for the connected client.
        /// </summary>
        public DnsMessage ResponseMessage { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SntpMessageEventArgs" /> class.
        /// </summary>
        /// <param name="channel">Socket channel request is recived on.</param>
        /// <param name="data">Raw data received from socket.</param>
        public DnsMessageEventArgs(SocketChannel channel, SocketBuffer data)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            this.Channel = channel;

            if (data == null) throw new ArgumentNullException("data");
            this.ChannelBuffer = data;

            try
            {
                // Parse the sntp message
                this.RequestMessage = new DnsMessage(data.Buffer);

                // log that the packet was successfully parsed
                Logger.WriteDebug(this, "PACKET with channel id " +
                    this.Channel.ChannelId.ToString() +
                    " successfully parsed from client endpoint " +
                    this.Channel.RemoteEndpoint.ToString());

                if (this.RequestMessage.TransactionId > 0)
                {
                    Logger.WriteInfo(this, "PACKET with message id " +
                        this.RequestMessage.TransactionId.ToHexString("0x") +
                        " successfully parsed from client endpoint " +
                        this.Channel.RemoteEndpoint.ToString());
                }

                Logger.WriteDebug(this.RequestMessage.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteError(this, "Error parsing message:" + ex.Message.ToString(), ex);
                return;
            }
        }
    }
}
