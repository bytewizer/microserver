using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Sockets
{
    public abstract class SocketService
    {
        protected SocketListener listener;
        protected IPipelineFilter pipeline;
        protected IContext context = new Context();      
        
        protected readonly ServerOptions _options;

        public SocketService()
            : this(_ => { }) { }

        public SocketService(IPipelineBuilder pipeline)
            : this(options =>
            {
                options.Pipeline = pipeline;
            })
        { }

        public SocketService(int port)
            : this(options => 
            { 
                options.Listen(IPAddress.Any, port); 
            })
        { }

        public SocketService(IPAddress address, int port, IPipelineBuilder pipeline)
            : this(options =>
            {
                options.Pipeline = pipeline;
                options.Listen(address, port);
            })
        { }

        public SocketService(ServerOptionsDelegate configure)
        {
            var options = new ServerOptions();
            configure(options);
            _options = options;
        }

        public bool Start()
        {
            if (listener != null)
            {
                return true;
            }
            else
            {
                try
                {
                    var options = _options.Listener;
                    pipeline = ((PipelineBuilder)_options.Pipeline).Build();

                    listener = new SocketListener(options);
                    listener.ClientConnected += ClientConnected;

                    return listener.Start();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Stop()
        {
            if (listener == null)
            {
                return true;
            }
            else
            {
                var status = listener.Stop();
                listener = null;
                pipeline = null;
                return status;
            }
        }

        private void ClientConnected(Socket socket)
        {
            var options = _options.Listener;
            try
            {
                using (socket)
                {
                    if (options.IsTls)
                    {
                        context.Session.Assign(socket, options.Certificate, options.SslProtocols);
                    }
                    else
                    {
                        context.Session.Assign(socket);
                    }

                    pipeline.Invoke(context);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to accept incoming connection. Exception: { ex.Message } StackTrace: {ex.StackTrace}");
                return;
            }
        }
    }
}