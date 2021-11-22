using System;
using System.Collections;
using System.Threading;

using Bytewizer.TinyCLR.Http.Extensions;

namespace Bytewizer.TinyCLR.Http.WebSockets.Middleware
{
    internal class HubDelegateFactory
    {
        private readonly Hashtable _channels = new Hashtable();

        public RequestDelegate CreateRequestDelegate(Type hubType)
        {
            return (context) =>
            {
                var hub = (Hub)Activator.CreateInstance(hubType);
                if (hub == null)
                {
                    throw new InvalidOperationException(nameof(hub));
                }

                hub.Caller = new HubCallerContext(context);
                hub.Clients = new Clients(_channels, context.Channel);

                _channels.Add(context.Channel.Connection.Id, context.Channel);

                var inputStream = context.Channel.InputStream;
                var outputStream = context.Channel.OutputStream;

                try
                {
                    var websocket = context.GetWebSocket();
                    if (websocket == null)
                    {
                        throw new NullReferenceException(nameof(websocket));
                    }

                    hub.OnConnected();

                    var active = true;
                    while (active)  // TODO: More efficent way?
                    {
                        if (inputStream != null & inputStream.Length > 0)
                        {
                            var frame = WebSocketFrame.ReadFrame(inputStream, true);
                            if (frame.IsClose)
                            {
                                _channels.Remove(context.Channel.Connection.Id);

                                var buffer = WebSocketFrame.CreateCloseFrame(PayloadData.Empty, false).ToArray();
                                outputStream.Write(buffer);

                                var execption = new Exception("WebSocket client disconnected");
                                hub.OnDisconnected(execption);

                                context.Channel.Clear();
                                break;

                            }else if(frame.IsFinal)
                            {
                                hub.OnMessage(new WebSocketContext(frame.PayloadData.ToArray(), frame.IsText));
                            }
                            else if (frame.IsFragment)
                            {
                                // TODO: build memory stream of data
                            }
                            else if (frame.IsPing)
                            {
                                var buffer = WebSocketFrame.CreatePongFrame(PayloadData.Empty, false).ToArray();
                                outputStream.Write(buffer);

                            }
                            else if (frame.IsPong)
                            {
                                var buffer = WebSocketFrame.CreatePingFrame(false).ToArray();
                                outputStream.Write(buffer);
                            }
           
                        }

                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    hub.OnDisconnected(ex);
                }
            };
        }
    }
}