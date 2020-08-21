using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Storage;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.TinyServer.Properties;
using Bytewizer.TinyCLR.Threading;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.TinyServer
{
    class Program
    {
        static void Main()
        {
            Networking.SetupEthernet();

            //var server = new SocketServer(options =>
            //{
            //    options.Listen(IPAddress.Any, 80);
            //    options.Register(new AModule());
            //    options.Register(new BModule());
            //    options.Register(new CModule());
            //});
            //server.Start();


            SetupHttpServer();
            //SetupSecureServer();  // Install self signed browser certs from the certificate folder before running secure server
        }

        static void SetupHttpServer()
        {
            var server = new SocketServer(options =>
            {
                options.Register(new SimpleResponse());
            });
            server.Start();
        }

        static void SetupSecureServer()
        {
            // Read certifiacate from project resource
            var X509cert = ReadCertFromResources();

            // Read certificate from SD card
            //var X509cert = ReadCertFromSdCard();

            var sslserver = new SocketServer(options =>
            {
                options.Listen(IPAddress.Any, 443, listener =>
                {
                    listener.UseHttps(X509cert);
                });
                options.Register(new SimpleResponse());
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