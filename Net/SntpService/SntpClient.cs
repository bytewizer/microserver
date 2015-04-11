using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Microsoft.SPOT;

using MicroServer.Logging;
using MicroServer.Utilities;

namespace MicroServer.Net.Sntp
{
    public class SntpClient : IDisposable
    {
        #region Private Properties

        private IPAddress _dhcpInterfaceAddress = IPAddress.Any;

        private Timer _pollingTimer;

        private string _primaryServer = Constants.SNTP_DEFAULT_PRIMARY_SERVER;
        private string _alternateServer = Constants.SNTP_DEFAULT_ALTERNATE_SERVER;

        private int _receiveTimeout = Constants.SNTP_RECEIVE_TIMEOUT;
        private int _sendTimeout = Constants.SNTP_SEND_TIMEOUT;

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        ///   Gets or sets the timeout for receiving data
        /// </summary>
        public int ReceiveTimeout
        {
            get { return _receiveTimeout; }
            set { this._receiveTimeout = value; }
        }

        /// <summary>
        ///   Gets or sets the timeout for sending data
        /// </summary>
        public int SendTimeout
        {
            get { return _sendTimeout; }
            set { this._sendTimeout = value; }
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

        #region Constructors / Deconstructors

        public SntpClient() { }

        ~SntpClient()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pollingTimer.Dispose();
            }
        }

        #endregion  Constructors / Deconstructors

        #region Events

        public event TimeChangedEventHandler OnTimeReceived;

        private void OnTimeChanged(TimeChangedEventArgs args)
        {
            if (OnTimeReceived != null)
                OnTimeReceived(this, args);
        }

        #endregion Events

        #region Methods

        public void Start(int pollingDelay = 10000, int pollingInterval = 30000)
        {
            _pollingTimer = new Timer(new TimerCallback(this.Synchronize), null, pollingDelay, pollingInterval);
        }

        public void Stop()
        {
            _pollingTimer.Dispose(); 
        }

        public void Synchronize()
        {
            this.Synchronize(null);
        }

        private void Synchronize(object state)
        {    
            SntpMessage requestMessage = new SntpMessage();
            SntpMessage responseMessage = new SntpMessage();
            responseMessage = SendRequest(requestMessage);

            if (responseMessage != null)
            {
                TimeChangedEventArgs args = new TimeChangedEventArgs(responseMessage);
                this.OnTimeChanged(args);
            }
        }

        public DateTime Now()
        {
            return this.Now(null);
        }

        public DateTime Now(string timeServer)
        {
            try
            {
                SntpMessage requestMessage = new SntpMessage();
                SntpMessage responseMessage = new SntpMessage();

                if (!StringUtility.IsNullOrEmpty(timeServer))
                {
                    // Resolve server address
                    IPHostEntry timeHost = Dns.GetHostEntry(timeServer);
                    IPEndPoint remoteEndPoint = new IPEndPoint(timeHost.AddressList[0], Constants.SNTP_CLIENT_PORT);

                    responseMessage = SendRequest(requestMessage, remoteEndPoint);
                }
                else
                {
                    responseMessage = SendRequest(requestMessage);
                }
               
                return responseMessage.TransmitDateTime;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public SntpMessage SendRequest(SntpMessage sentMessage)
        {
           return this.SendRequest(sentMessage, null);
        }

        public SntpMessage SendRequest(SntpMessage sentMessage, IPEndPoint remoteEndPoint)
        {
            try
            {
                using (Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    if (remoteEndPoint == null)
                    {
                        try
                        {
                            IPHostEntry timeHost = Dns.GetHostEntry(_primaryServer);
                            remoteEndPoint = new IPEndPoint(timeHost.AddressList[0], Constants.SNTP_CLIENT_PORT);
                            _socket.Connect(remoteEndPoint);
                        }
                        catch
                        {

                            Logger.WriteInfo(this, "Failed to contact primary server " + _primaryServer + " failed.  Trying again using alternate server " + _alternateServer
                                + " endpoint " + remoteEndPoint.ToString());

                            IPHostEntry timeHost = Dns.GetHostEntry(_alternateServer);
                            remoteEndPoint = new IPEndPoint(timeHost.AddressList[0], Constants.SNTP_CLIENT_PORT);
                            _socket.Connect(remoteEndPoint);
                        }
                    }

                    try
                    {
                        _socket.Connect(remoteEndPoint);
                    }
                    catch
                    {
                        Logger.WriteInfo(this, "Failed to contact primary server " + _primaryServer + " failed.  Trying again using alternate server " + _alternateServer
                            + " endpoint " + remoteEndPoint.ToString());
                    }

                    if (remoteEndPoint != null)
                    {

                        Byte[] messageBuffer = new Byte[Constants.SNTP_MAX_MESSAGE_SIZE];
                        _socket.ReceiveTimeout = Constants.SNTP_RECEIVE_TIMEOUT;
                        _socket.SendTimeout = Constants.SNTP_SEND_TIMEOUT;

                        // send message
                        _socket.SendTo(sentMessage.ToArray(), remoteEndPoint);
                        Logger.WriteDebug(this, "PACKET was "
                                        + " successfully sent to client endpoint " +
                                        remoteEndPoint.ToString());
                        Logger.WriteDebug(sentMessage.ToString());

                        // receive message
                        _socket.Receive(messageBuffer);
                        _socket.Close();

                        SntpMessage recivedMessage = new SntpMessage(messageBuffer);
                        Logger.WriteDebug(this, "PACKET was successfully received from client endpoint " +
                                        remoteEndPoint.ToString());
                        Logger.WriteDebug(recivedMessage.ToString());

                        return recivedMessage;
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

            return null;
        }

        #endregion Methods
    }
}