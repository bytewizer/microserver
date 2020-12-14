using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.TinyCLR.Hosting
{
    public class SocketHostService : IHostedService
    {
        public ILogger Logger { get; }
        public IHardware Hardware { get; }
        public IServer Server { get; }

        public SocketHostService(ILoggerFactory loggerFactory, IHardware hardware, IServer server)
        {
            Logger = loggerFactory.CreateLogger(typeof(SocketHostService));
            Server = server;
            Hardware = hardware;
        }

        public void Start()
        {            
            Hardware.Network.Enabled();
            
            Server.Start();

            Logger.LogInformation("Host server is started.");
        }

        public void Stop()
        {
            Hardware.Network.Disable();
            
            Server.Stop();
            
            Logger.LogInformation("Host server is stopped.");
        }
    }
}