using System;
using System.Net;
using System.Threading;
using Bytewizer.TinyCLR.Ftp;
using Bytewizer.TinyCLR.Sockets.Client;

namespace Bytewizer.Playground.Ftp
{
    class Program
    {
        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            var server = new FtpServer(options =>
            {
                options.AllowAnonymous = true;
                options.Pipeline(app =>
                {
                    //app.Use();
                });
            });
            server.Start();
        }
    }
}