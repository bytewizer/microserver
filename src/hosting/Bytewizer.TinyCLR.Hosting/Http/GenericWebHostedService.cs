using System.Threading;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Hosting
{
    internal class GenericWebHostedService : IHostedService
    {
        public GenericWebHostedService(IServer server)
        {
            Server = server;
        }

        public IServer Server { get; }

        public Thread Start()
        {
            Server.Start();
            return Thread.CurrentThread;
        }

        public void Stop(int timeout = 1000)
        {
            Server.Stop();
        }
    }
}