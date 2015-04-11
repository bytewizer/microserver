using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using Microsoft.SPOT;
using Microsoft.SPOT.IO;
using System.IO;

using MicroServer.Net.Sockets;
using MicroServer.Threading;
using MicroServer.Utilities;
using MicroServer.Extensions;
using MicroServer.Logging;

namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// A class that serves SNTP packet requests from remote clients.
    /// </summary>
    public class SntpService : SocketService
    {
        #region Private Properties

        private IPAddress _interfaceAddress = IPAddress.Any;
        private UdpListener _listener;

        bool _useLocalTimeSource = false;
        private string _primaryServer = Constants.SNTP_DEFAULT_PRIMARY_SERVER;
        private string _alternateServer = Constants.SNTP_DEFAULT_ALTERNATE_SERVER;

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// Interface IP address.
        /// </summary>
        public new IPAddress InterfaceAddress
        {
            get { return _interfaceAddress; }
            set { this._interfaceAddress = value; }
        }

        /// <summary>
        /// Use local hardware as time source.
        /// </summary>
        public bool UseLocalTimeSource
        {
            get { return _useLocalTimeSource; }
            set { _useLocalTimeSource = value; }
        }

        /// <summary>
        /// Primary SNTP Server address.
        /// </summary>
        public string PrimaryServer
        {
            get { return _primaryServer; }
            set { _primaryServer = value; }
        }

        /// <summary>
        ///  Alternate SNTP server is used if communication with primary server failed.
        /// </summary>
        public string AlternateServer
        {
            get { return _alternateServer; }
            set { _alternateServer = value; }
        }

        #endregion Public Properties

        #region Methods

        /// <summary>
        ///  Starts the service listener if in a stopped state.
        /// </summary>
        public override bool Start()
        {
            _listener = new UdpListener();
            _listener.InterfaceAddress = _interfaceAddress;
            _listener.BufferSize = Constants.SNTP_MAX_MESSAGE_SIZE;
            _listener.ReceiveTimeout = Constants.SNTP_RECEIVE_TIMEOUT;
            _listener.SendTimeout = Constants.SNTP_SEND_TIMEOUT;
            _listener.ClientConnected += OnClientConnect;
            _listener.ClientDisconnected += OnClientDisconnect;
            return _listener.Start(Constants.SNTP_SERVICE_PORT, false);
        }

        /// <summary>
        ///  Stops the service listener if in started state.
        /// </summary>
        public override bool Stop()
        {
            try
            {
                return _listener.Stop();
            }

            catch (Exception ex)
            {
                Logger.WriteError(this, "Stopping Service failed: " + ex.Message.ToString(), ex);
            }

            return false;
        }

        /// <summary>
        ///  Remote client connects and makes a request.
        /// </summary>
        private void OnClientConnect(object sender, ClientConnectedEventArgs args)
        {

            SocketBuffer channelBuffer = args.ChannelBuffer;

            if (channelBuffer != null
                && args.Channel.IsConnected
                && channelBuffer.BytesTransferred >= Constants.SNTP_MIN_MESSAGE_SIZE
                && channelBuffer.BytesTransferred <= Constants.SNTP_MAX_MESSAGE_SIZE)
            {
                Logger.WriteDebug(this, "PACKET request with channel id " +
                    args.Channel.ChannelId.ToString() +
                    " was received from " +
                    args.Channel.RemoteEndpoint.ToString() +
                    " and queued for processing...");

                SntpMessageEventArgs messageArgs = new SntpMessageEventArgs(args.Channel, args.ChannelBuffer);
                OnSntpMessageReceived(this, messageArgs);

                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRequest), messageArgs);
            }
        }

        /// <summary>
        ///  Remote client is disconnected.
        /// </summary>
        private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs args)
        {
            Logger.WriteDebug(this, "Remote client was disconnected");
        }

        /// <summary>
        ///  Process request and send reply back to rempte client.
        /// </summary>
        private void ProcessRequest(Object state)
        {
            SntpMessageEventArgs args = (SntpMessageEventArgs)state;

            if (args.RequestMessage != null)
            {
                switch (args.RequestMessage.Mode)
                {
                    case Mode.Client:

                        if (_useLocalTimeSource == true)
                        {
                            args.ResponseMessage = args.RequestMessage;
                            args.ResponseMessage.Mode = Mode.Server;
                            args.ResponseMessage.ReceiveDateTime = DateTime.UtcNow;
                            args.ResponseMessage.TransmitDateTime = DateTime.UtcNow;
                            args.ResponseMessage.Stratum = Stratum.Secondary;
                            args.ResponseMessage.ReferenceIdentifier = ReferenceIdentifier.LOCL;
                            this.SendReply(args);
                        }
                        else
                        {
                            this.SendRequest(args);
                            this.SendReply(args);
                        }
                        break;

                    case Mode.Broadcast:
                    case Mode.Reserved:
                    case Mode.ReservedNTPControl:
                    case Mode.ReservedPrivate:
                    case Mode.Server:
                    case Mode.SymmetricActive:
                    case Mode.SymmetricPassive:
                        break;

                    default:
                        Logger.WriteInfo(this, "UNKNOWN message mode received and ignored");
                        break;
                }
            }
        }

        /// <summary>
        /// A message is processsed and reply is sent back. 
        /// </summary>
        private void SendReply(SntpMessageEventArgs args)
        {
            try
            {
                args.Channel.SendTo(args.ResponseMessage.ToArray(), args.Channel.RemoteEndpoint);

                Logger.WriteDebug(this, "PACKET with channel id " +
                                args.Channel.ChannelId.ToString() +
                                " successfully sent to client endpoint " +
                                args.Channel.RemoteEndpoint.ToString());

                Logger.WriteDebug(args.ResponseMessage.ToString());

                OnSntpMessageSent(this, args);
            }
            catch (Exception ex)
            {
                Logger.WriteError(this, "Error Message:" + ex.Message.ToString(), ex);
            }
        }

        /// <summary>
        /// A message was recived then forwared onto a remote time server.  
        /// </summary>
        private void SendRequest(SntpMessageEventArgs args)
        {
            try
            {
                using (Socket _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    Byte[] messageBuffer = new Byte[Constants.SNTP_MAX_MESSAGE_SIZE];

                    IPEndPoint remoteEndPoint = null;

                    try
                    {
                        IPHostEntry timeHost = Dns.GetHostEntry(_primaryServer);
                        remoteEndPoint = new IPEndPoint(timeHost.AddressList[0], Constants.SNTP_CLIENT_PORT);
                        _sendSocket.Connect(remoteEndPoint);
                    }

                    catch
                    {

                        Logger.WriteInfo(this, "Failed to contact primary server " + _primaryServer + " failed.  Trying again using alternate server " + _alternateServer
                            + " endpoint " + remoteEndPoint.ToString());

                        IPHostEntry timeHost = Dns.GetHostEntry(_alternateServer);
                        remoteEndPoint = new IPEndPoint(timeHost.AddressList[0], Constants.SNTP_CLIENT_PORT);
                        _sendSocket.Connect(remoteEndPoint);
                    }

                    if (remoteEndPoint != null)
                    {
                        _sendSocket.ReceiveTimeout = Constants.SNTP_RECEIVE_TIMEOUT;
                        _sendSocket.SendTimeout = Constants.SNTP_SEND_TIMEOUT;

                        // send message
                        _sendSocket.Connect(remoteEndPoint);
                        _sendSocket.Send(args.RequestMessage.ToArray());
                        Logger.WriteDebug(this, "PACKET successfully sent to client endpoint " +
                                        remoteEndPoint.ToString());
                        Logger.WriteDebug(args.RequestMessage.ToString());

                        // receive message
                        _sendSocket.Receive(messageBuffer);
                        _sendSocket.Close();

                        args.ResponseMessage = new SntpMessage(messageBuffer);
                        Logger.WriteDebug(this, "PACKET successfully received from client endpoint " +
                                        remoteEndPoint.ToString());
                        Logger.WriteDebug(args.ResponseMessage.ToString());
                    }
                }
            }
            catch (SocketException ex)
            {
                Logger.WriteError(this, ex.Message + "Socket Error Code: " + ex.ErrorCode.ToString(), ex);
            }
            catch (Exception ex)
            {
                Logger.WriteError(this, "Error Message:" + ex.Message.ToString(), ex);
            }
        }

        #endregion Methods

        #region Events

        /// <summary>
        ///     A sntp message was recived and processed.
        /// </summary>
        public event SntpMessageEventHandler OnSntpMessageReceived = delegate { };

        /// <summary>
        ///     A sntp message was recived and processed and sent to an sntp address.
        /// </summary>
        public event SntpMessageEventHandler OnSntpMessageSent = delegate { };

        #endregion Events
    }
}
