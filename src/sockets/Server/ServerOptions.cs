using System;
using System.Net;
using System.Threading;

namespace Bytewizer.Sockets
{
    public class ServerOptions
    {
        internal SocketListenerOptions Listener { get; private set; } = new SocketListenerOptions();

        internal IPipelineBuilder Pipeline { get; set; } = new PipelineBuilder();

        public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.AboveNormal;

        public void Register(IMiddleware filter)
        {
            Pipeline.Register(filter);
        }
        public void Register(FilterDelegate filter)
        {
            Pipeline.Register(filter);
        }

        public void Listen()
        {
            Listen(IPAddress.Any, 80);
        }

        public void Listen(IPEndPoint endPoint)
        {
            Listen(endPoint, _ => { });
        }

        public void Listen(IPAddress address, int port)
        {
            Listen(address, port, _ => { });
        }

        public void Listen(IPAddress address, int port, SocketListenerOptionsDelegate configure)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort)
            {
                throw new ArgumentOutOfRangeException(nameof(port), $"Must be less then { IPEndPoint.MaxPort } and greater then { IPEndPoint.MinPort }.");
            }

            Listen(new IPEndPoint(address, port), configure);
        }

        public void Listen(IPEndPoint endPoint, SocketListenerOptionsDelegate configure)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            var listenOptions = new SocketListenerOptions(endPoint);
            configure(listenOptions);

            Listener = listenOptions;
        }
    }
}