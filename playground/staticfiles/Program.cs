using System.Collections;
using System.Security.Cryptography.X509Certificates;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Hardware;

using Bytewizer.Playground.StaticFiles.Properties;

namespace Bytewizer.Playground.StaticFiles
{
    class Program
    {
        static void Main()
        {
            InitializeHardware();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseResourceFiles(new ResourceFileOptions()
                    {
                        // Resource manager must be included as an option along with a list of resoruces
                        ResourceManager = Resources.ResourceManager,
                        Resources = new Hashtable()
                        {
                            { "/resource.html", Resources.StringResources.ResourceFile }
                        }
                    });
                    app.UseFileServer();  //TODO: Why should order matter?
                });
            });
            server.Start();

            //var X509cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DeviceCert))
            //{
            //    PrivateKey = Resources.GetBytes(Resources.BinaryResources.DeviceKey)
            //};

            //var sslServer = new HttpServer(options =>
            //{
            //    options.Pipeline(app =>
            //    {
            //        app.UseFileServer();
            //    });
            //    options.Listen(443, listener =>
            //    {
            //        listener.UseHttps(X509cert);
            //    });
            //});
            //sslServer.Start();
        }

        public static void InitializeHardware()
        {
            var hardwareOptions = new HardwareOptions() { BoardModel = BoardModel.Sc20260D };
            var mainBoard = new Mainboard(hardwareOptions).Connect();
            mainBoard.Network.Enabled();
        }
    }
}