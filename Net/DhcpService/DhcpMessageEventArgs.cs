using System;
using Microsoft.SPOT;

using MicroServer.Net.Sockets;
using MicroServer.Extensions;
using MicroServer.Logging;
using MicroServer.Utilities;

namespace MicroServer.Net.Dhcp
{
    public class DhcpMessageEventArgs : EventArgs
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
        public DhcpMessage RequestMessage { get; private set; }

        /// <summary>
        ///     Requested options for the connected client.
        /// </summary>
        public MessageOptions RequestOptions { get; private set; }

        /// <summary>
        ///     Response message for the connected client.
        /// </summary>
        public DhcpMessage ResponseMessage { get; set; }

        /// <summary>
        ///     Response lease binding for the connected client.
        /// </summary>
        public BindingLease ResponseBinding { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DhcpMessageEventArgs"/> class.
        /// </summary>
        /// <param name="channel">Socket channel request is recived on.</param>
        /// <param name="data">Raw data received from socket.</param>
        public DhcpMessageEventArgs(SocketChannel channel, SocketBuffer data)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            this.Channel = channel;

            if (data == null) throw new ArgumentNullException("data");
            this.ChannelBuffer = data;

            try
            {
                // Parse the dhcp message
                this.RequestMessage = new DhcpMessage(data.Buffer);

                // get message options
                MessageOptions options = new MessageOptions();

                options.MessageType = this.RequestMessage.GetOptionData(DhcpOption.DhcpMessageType);
                // get message type option
                options.ClientId = this.RequestMessage.GetOptionData(DhcpOption.ClientId);
                if (options.ClientId == null)
                {
                    // if the client id is not provided use the client hardware address from the message
                    options.ClientId = this.RequestMessage.ClientHardwareAddress.ToArray();
                }
                // get host name option
                options.HostName = this.RequestMessage.GetOptionData(DhcpOption.Hostname);
                // get address request option
                options.AddressRequest = this.RequestMessage.GetOptionData(DhcpOption.AddressRequest);
                //get server identifier option
                options.ServerIdentifier = this.RequestMessage.GetOptionData(DhcpOption.ServerIdentifier);
                // get paramerter list opiton
                options.ParameterList = this.RequestMessage.GetOptionData(DhcpOption.ParameterList);

                // set the response options object
                this.RequestOptions = options;

                // set the response binding object
                ResponseBinding = new BindingLease(
                    ByteUtility.GetString(this.RequestOptions.ClientId),
                    this.RequestMessage.ClientHardwareAddress,
                    this.RequestMessage.ClientAddress,
                    this.RequestOptions.HostName,
                    DateTime.MinValue,
                    this.RequestMessage.SessionId,
                    LeaseState.Unassigned);

                // log that the packet was successfully parsed
                Logger.WriteDebug(this, "PACKET with message id " +
                        this.RequestMessage.SessionId.ToHexString("0x") + " successfully parsed from client endpoint " +
                        this.Channel.RemoteEndpoint.ToString());

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
