using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Hardware;
using Bytewizer.TinyCLR.Tests.Sockets.Properties;
using System.Threading;
using GHIElectronics.TinyCLR.Native;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Tests.Sockets
{
    class Program
    {
        static void Main()
        {


            Mainboard.Initialize();
            //StartStopServer();
            SetupHttpServer();
            //SetupSecureServer();

            //var freeRam = Memory.ManagedMemory.FreeBytes;
            //var usedRam = Memory.ManagedMemory.UsedBytes;
            //var percent = ((double)(usedRam + freeRam) - freeRam) / (usedRam + freeRam) * 100;
            //while (true)
            //{
            //    Debug.WriteLine($"Free Memory: {freeRam} / Used Memory: {usedRam} {percent}%");
            //    Thread.Sleep(1000);
            //}

        }

        static void StartStopServer()
        {
            var server = new SocketServer(options =>
            {
                options.Register(new SimpleResponse());
            });
            server.Start();
            Thread.Sleep(15000);
            server.Stop();
            Thread.Sleep(10000);
            server.Start();
            //Thread.Sleep(5000);
            //server.Stop();
            //Thread.Sleep(5000);
            //server.Start();
            //Thread.Sleep(5000);
            //server.Stop();
            //Thread.Sleep(5000);
            //server.Start();
            //Thread.Sleep(5000);
            //server.Stop();
            //Thread.Sleep(5000);
            //server.Start();
            //Thread.Sleep(5000);
            //server.Stop();
            //Thread.Sleep(5000);
            //server.Start();
            //Thread.Sleep(5000);
            //server.Stop();
            //Thread.Sleep(5000);
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
            var X509cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DeviceCert))
            {
                PrivateKey = Resources.GetBytes(Resources.BinaryResources.DeviceKey)
            };

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
    }
}