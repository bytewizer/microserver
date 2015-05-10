using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using Microsoft.SPOT;

using MicroServer.Threading;
using MicroServer.Utilities;
using MicroServer.Extensions;
using MicroServer.Logging;

using MicroServer.Net.Sockets;
using MicroServer.Net.Http.Serializers;
using MicroServer.Net.Http.Messages;
using MicroServer.Net.Http.Modules;

namespace MicroServer.Net.Http
{
    public class HttpService : SocketService
    {
        #region Private Properties

        private static HttpService _service;
        private TcpListener _listener;

        private IPAddress _interfaceAddress = IPAddress.GetDefaultLocalAddress();
        private int _servicePort = Constants.HTTP_SERVER_PORT;

        private readonly IMessageDecoder _decoder;
        private readonly IMessageEncoder _encoder ;
        private readonly IModuleManager _moduleManager;

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        ///   Gets or sets the ip address for receiving data
        /// </summary>
        public new IPAddress InterfaceAddress
        {
            get { return _interfaceAddress; }
            set { _interfaceAddress = value; }
        }

        /// <summary>
        ///   Gets or sets the port for receiving data
        /// </summary>
        public new int ServicePort
        {
            get { return _servicePort; }
            set { _servicePort = value; }
        }

        /// <summary>
        ///     Gets port that the server is listening on.
        /// </summary>
        public int ActivePort
        {
            get { return _listener.ActivePort; }
        }
        /// <summary>
        /// Gets current server.
        /// </summary>
        /// <remarks>
        /// Only valid when a request have been received and is being processed.
        /// </remarks>
        public static HttpService Current
        {
            get { return _service; }
        }

        /// <summary>
        /// if set, the body decoder will be used and the server will produce <see cref="HttpRequest"/> instead of <see cref="HttpRequestBase"/>.
        /// </summary>
        /// <remarks>
        /// <para>Specified per default</para>
        /// </remarks>
        public IMessageSerializer BodyDecoder { get; set; }

        /// <summary>
        ///     You can fill this item with application specific information
        /// </summary>
        /// <remarks>
        ///     It will be supplied for every request in the <see cref="IHttpContext" />.
        /// </remarks>
        public IItemStorage ApplicationInfo { get; set; }

        #endregion Public Properties

        #region Constructors / Deconstructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpService" /> class.
        /// </summary>
        public HttpService(IModuleManager moduleManager)
        {
            ApplicationInfo = new MemoryItemStorage();
            BodyDecoder = new CompositeSerializer();

            _service = this;
            _moduleManager = moduleManager;
            _encoder =  new HttpMessageEncoder();
            _decoder = new HttpMessageDecoder(BodyDecoder);
            _decoder.MessageReceived = OnMessageReceived;
        }

        #endregion  Constructors / Deconstructors

        #region Methods

        public override bool Start()
        {
            try
            {
                _listener = new TcpListener();
                _listener.InterfaceAddress = _interfaceAddress;
                _listener.BufferSize = Constants.HTTP_MAX_MESSAGE_SIZE;
                _listener.ReceiveTimeout = Constants.HTTP_RECEIVE_TIMEOUT;
                _listener.SendTimeout = Constants.HTTP_SEND_TIMEOUT;
                _listener.ListenBacklog = Constants.HTTP_LISTEN_BACKLOG;
                _listener.ClientConnected += OnClientConnected;
                _listener.ClientDisconnected += OnClientDisconnect;
                return _listener.Start(_servicePort);
            }
            catch
            {
                return false;
            }
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
        ///     Add a HTTP module
        /// </summary>
        /// <param name="module">Module to include</param>
        /// <remarks>Modules are executed in the order they are added.</remarks>
        public void Add(IHttpModule module)
        {
            _moduleManager.Add(module);
        }

        private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs args)
        {
            //Logger.WriteDebug(this, "Pipeline => OnClientDisconnect");
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            //Logger.WriteDebug(this, "Pipeline => OnClientRequest");

            try
            {
                _decoder.ProcessReadBytes(args.Channel, args.ChannelBuffer);
            }
            catch (Exception ex)
            {
                SendFailure(args.Channel, ex);
            }
        }

        private void OnMessageReceived(SocketChannel channel, HttpMessage message)
        {
            //Logger.WriteDebug(this, "Pipeline => OnMessageReceived");

            _service = this;

            var context = new HttpContext
            {
                Application = ApplicationInfo,
                Channel = channel,
                Items = new MemoryItemStorage(),
                Request = (IHttpRequest)message,
                Response = ((IHttpRequest)message).CreateResponse()
                
            };

            context.Request.RemoteEndPoint = channel.RemoteEndpoint;
            context.Response.AddHeader("X-Powered-By", "MicroServer");
            _moduleManager.InvokeAsync(context, SendResponse);
        }

        private void SendResponse(IAsyncModuleResult obj)
        {
            //Logger.WriteDebug(this, "Pipeline => SendResponse");

            var context = (HttpContext)obj.Context;
            SendChannel(context.Channel, context);
        }

        private void SendChannel(SocketChannel channel, object message)
        {
            //Logger.WriteDebug(this, "Pipeline => SendChannel");

            try
            {
                Stream sendMessage = _encoder.Prepare(message);
                if (sendMessage.Length > 0)
                    SendCompleted(channel.Send(sendMessage));
            }
            catch (Exception ex)
            {
               SendFailure(channel, ex);
            }
        }

        private void SendCompleted(int bytesTransferred)
        {
            //Logger.WriteDebug(this, "Pipeline => SendCompleted");
            
            bool isComplete = _encoder.SendCompleted(bytesTransferred);
            if (!isComplete)
            {
                return;
            }
        }

        private void SendFailure(SocketChannel channel, Exception ex)
        {
            //Logger.WriteDebug("this, Pipeline => MessageFailure");

            var pos = ex.Message.IndexOfAny(new[] { '\r', '\n' });
            var descr = pos == -1 ? ex.Message : ex.Message.Substring(0, pos);
            byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 400 OK\r\n" +
                    "Content-Type: text/html; charset=UTF-8\r\n\r\n" +
                    "<doctype !html><html><head><title></title></head>" +
                    "<body>" + descr + "</body></html>\r\n");

            channel.Send(response);
            channel.Close();
        }

        #endregion Methods

        #region Events


        #endregion Events
    }
}
