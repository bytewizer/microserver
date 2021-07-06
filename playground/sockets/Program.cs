using Bytewizer.TinyCLR.Sockets;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.Playground.Sockets
{
    class Program
    {
        private static readonly ILoggerFactory loggerFactory = new LoggerFactory();

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
           
            loggerFactory.AddDebug(LogLevel.Trace);

            IServer server = new SocketServer(loggerFactory, options =>
            {
                options.Listen(80);
                options.Pipeline(app =>
                {
                    //app.UseMemoryInfo();
                    app.UseHttpResponse();
                });
            });
            server.Start();
        }
    }
}