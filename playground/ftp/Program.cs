using System;
using System.Net;
using Bytewizer.TinyCLR.Ftp;

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
                options.Pipeline(app =>
                {
                    //app.Use();
                });
            });
            server.Start();
        }
    }
}