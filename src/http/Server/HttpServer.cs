using System.Net;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public class HttpServer : SocketService
    {
        public HttpServer()
        {
            context = new HttpContext();
        }

        public HttpServer(IPipelineBuilder pipeline)
            : base(pipeline)
        {
            context = new HttpContext();
        }

        public HttpServer(int port)
            : base(port)
        {
            context = new HttpContext();
        }

        public HttpServer(ServerOptionsDelegate configure)
            : base(configure)
        {
            context = new HttpContext();
        }

        public HttpServer(IPAddress address, int port, IPipelineBuilder pipeline)
            : base(address, port, pipeline)
        {
            context = new HttpContext();
        }
    }
}