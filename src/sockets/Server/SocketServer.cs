using System.Net;
using System.Threading;

namespace Bytewizer.Sockets
{
    public class SocketServer
    {
        private Thread thread;
        private SocketListener listener;

        private readonly ServerOptions _options;

        public SocketServer()
            : this(_ => { }) { }

        public SocketServer(IPipelineBuilder pipeline)
            : this(options =>
            {
                options.Pipeline = pipeline;
            })
        { }

        public SocketServer(int port)
            : this(options => 
            { 
                options.Listen(IPAddress.Any, port); 
            })
        { }

        public SocketServer(IPAddress address, int port, IPipelineBuilder pipeline)
            : this(options =>
            {
                options.Pipeline = pipeline;
                options.Listen(address, port);
            })
        { }

        public SocketServer(ServerOptionsDelegate configure)
        {
            var options = new ServerOptions();
            configure(options);
            _options = options;
        }

        public bool Start()
        {
            try
            {
                var options = _options.Listener;
                var pipeline = ((PipelineBuilder)_options.Pipeline).Build();

                listener = new SocketListener(options, pipeline);
                var status = listener.Start();

                thread = new Thread(() =>
                {
                    if (listener != null)
                    {
                        listener.AcceptConnections();
                    }
                });
                thread.Priority = _options.ThreadPriority;
                thread.Start();

                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool Stop()
        {
            if (listener != null)
            {
                return listener.Stop();
            }

            return true;
        }
    }
}