using System;
using System.Net;
using System.Threading;
using Bytewizer.TinyCLR.Ftp;
using Bytewizer.TinyCLR.Ftp.Internal;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.Playground.Ftp
{
    class Program
    {
        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            var server = new SocketServer(options =>
            {
                options.Listen(21);
                options.Pipeline(app =>
                {
                    app.Use(new FtpMiddleware());
                });
            });

            server.Start();
        }
    }
}