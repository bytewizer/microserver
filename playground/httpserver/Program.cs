using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Storage;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.WebServer.Properties;


namespace Bytewizer.TinyCLR.WebServer
{
    class Program
    {
        static void Main()
        {
            Networking.SetupEthernet();

            SetupHttpServer();
            //SetupSecureServer();  // Install self signed browser certs from the certificate folder before running secure server
        }

        static void SetupHttpServer()
        {
            var sd = StorageController.FromName(SC20100.StorageController.SdCard);
            var drive = FileSystem.Mount(sd.Hdc);

            var authOpitons = new AuthenticationOptions(new UserService(), "device.bytewizer.local");
            var filesOptions = new StaticFileOptions(drive);

            var server = new HttpServer(options =>
            {
                options.Register(new HttpSessionMiddleware());
                options.Register(new DeveloperExceptionPageMiddleware());
                //options.Register(new AuthenticationMiddleware(authOpitons));
                options.Register(new StaticFileMiddleware(filesOptions));
                options.Register(new HttpResponse());
            });
            server.Start();
        }

        static void SetupSecureServer()
        {
            // Read certifiacate from project resource
            var X509cert = ReadCertFromResources();

            // Read certificate from SD card
            //var X509cert = ReadCertFromSdCard();

            var authOpitons = new AuthenticationOptions(new UserService(), "device.bytewizer.local");

            var sslserver = new HttpServer(options =>
            {
                options.Listen(IPAddress.Any, 443, listener =>
                {
                    listener.UseHttps(X509cert);
                });
                options.Register(new HttpSessionMiddleware());
                options.Register(new AuthenticationMiddleware(authOpitons));
                options.Register(new HttpResponse());
            });

            sslserver.Start();
        }

        static X509Certificate ReadCertFromResources()
        {
            var X509cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DeviceCert))
            {
                PrivateKey = Resources.GetBytes(Resources.BinaryResources.DeviceKey)
            };

            return X509cert;
        }

        static X509Certificate ReadCertFromSdCard()
        {
            var storageController = StorageController.FromName(SC20260.StorageController.SdCard);
            var drive = FileSystem.Mount(storageController.Hdc);

            var certFilename = drive.Name + "\\bytewizer.local.pem";
            FileStream certRead = new FileStream(certFilename, FileMode.Open);

            var cert = new byte[certRead.Length];
            certRead.Read(cert, 0, cert.Length);

            var keyFileName = drive.Name + "\\bytewizer.local_key.pem";
            FileStream keyRead = new FileStream(keyFileName, FileMode.Open);

            var key = new byte[keyRead.Length];
            keyRead.Read(key, 0, key.Length);

            var X509cert = new X509Certificate(cert)
            {
                PrivateKey = key
            };

            return X509cert;
        }
    }
}