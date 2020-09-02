using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Storage;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.WebServer.Properties;
using Bytewizer.TinyCLR.WebServer.Authentication;

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

            //var authOpitons = new AuthenticationOptions(new UserService()) { Realm = "device.bytewizer.local" };

            var server = new HttpServer(options =>
            {
                //TODO: Build into server so that it is always in the pipeline
                options.UseMiddleware(new HttpSessionMiddleware());
                options.UseDeveloperExceptionPage();
                //options.UseAuthentication(new AuthenticationOptions()
                //{
                //    AuthenticationScheme = AuthenticationSchemes.Basic,
                //    AccountService = new AccountService(),
                //    Realm = "device.bytewizer.local"
                //});
                options.UseStaticFiles();     
                options.UseMvc();
                //options.UseMiddleware(new CustomMiddleware());

            });
            server.Start();
        }

        static void SetupSecureServer()
        {
            // Read certifiacate from project resource
            var X509cert = ReadCertFromResources();

            // Read certificate from SD card
            //var X509cert = ReadCertFromSdCard();

            var sslserver = new HttpServer(options =>
            {
                options.Listen(IPAddress.Any, 443, listener =>
                {
                    listener.UseHttps(X509cert);
                });
                options.Register(new HttpSessionMiddleware());
                options.Register(new CustomMiddleware());
            });

            sslserver.Start();
        }

        static X509Certificate ReadCertFromResources()
        {
            Resources.GetBytes(Resources.BinaryResources.DeviceCert);

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

//options.UseStatusCodePages(new StatusCodePagesOptions
//{
//    Handle = context =>
//    {
//        var response = context.Response;
//        if (response.StatusCode < 500)
//        {
//            response.Write($"Client error ({response.StatusCode})");
//        }
//        else
//        {
//            response.Write($"Server error ({response.StatusCode})");
//        }
//    }
//});

//options.UseResourceFiles(new ResourceFileOptions()
//{
//    Resources = new Hashtable()
//    {
//        { "/index.html", Resources.StringResources.Index },
//        { "/assets/css/bootstrap.css", Resources.StringResources.BootstrapCss },
//        { "/assets/js/bootstrap.js", Resources.StringResources.Bootstrap },
//        { "/assets/js/jquery.js", Resources.StringResources.Jquery },
//    }
//});