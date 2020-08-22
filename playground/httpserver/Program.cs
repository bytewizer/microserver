using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using GHIElectronics.TinyCLR.Devices.Rtc;
using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Storage;
using GHIElectronics.TinyCLR.Native;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.WebServer.Properties;


namespace Bytewizer.TinyCLR.WebServer
{
    class Program
    {
        static void Main()
        {
            Thread thread = new Thread(new ThreadStart(new NetworkingDuino().SetupEthernet));
            thread.Start();

            //Setup RTC
            var rtc = RtcController.GetDefault();
            //Start Charging battery
            rtc.SetChargeMode(BatteryChargeMode.Slow);
            //Check if time is already valid
            if (rtc.IsValid)
            {
                Debug.WriteLine("RTC is Valid");
                SystemTime.SetTime(rtc.Now);
            }

            //Http Server setup
            //Thread httpthread = new Thread(new ThreadStart(SetupHttpServer));
            //httpthread.Start();

            //SetupSecureServer();  // Install self signed browser certs from the certificate folder before running secure server
            Thread httpsthread = new Thread(new ThreadStart(SetupSecureServer));
            httpsthread.Start();

            var t = new Thread(StatusBlink);
            t.Start();


            Thread.Sleep(Timeout.Infinite);
        }

        static void SetupHttpServer()
        {
            var server = new HttpServer(options =>
            {
                options.Register(new SessionModule());
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

            try
            {
                var sslserver = new HttpServer(options =>
                {
                    options.Listen(IPAddress.Any, 443, listener =>
                    {
                        listener.UseHttps(X509cert);
                    });
                    options.Register(new HttpResponse());
                });

                sslserver.Start();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
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
            var storageController = StorageController.FromName(SC20100.StorageController.SdCard);
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
        static void StatusBlink()
        {
            var gpioController = GpioController.GetDefault();

            var led = gpioController.OpenPin(SC20100.GpioPin.PE11);
            led.SetDriveMode(GpioPinDriveMode.Output);

            while (true)
            {
                led.Write(led.Read() == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High);
                Thread.Sleep(1000);

            }
        }
    }
}