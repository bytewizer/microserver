using System.Net;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Sockets.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    public class HttpServer : SocketService
    {
        public HttpServer()
        {
            Context = new HttpContext();
        }

        public HttpServer(IPipelineBuilder pipeline)
            : base(pipeline)
        {
            Context = new HttpContext();
        }

        public HttpServer(int port)
            : base(port)
        {
            Context = new HttpContext();
        }

        public HttpServer(ServerOptionsDelegate configure)
            : base(configure, new HttpMiddleware())
        {
            Context = new HttpContext();
        }

        public HttpServer(IPAddress address, int port, IPipelineBuilder pipeline)
            : base(address, port, pipeline)
        {
            Context = new HttpContext();
        }
    }
}