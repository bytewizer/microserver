using System;
using System.Net;
using Microsoft.SPOT;

using MicroServer.Service;
using MicroServer.Net.Sntp;

namespace MicroServer.Sample
{
    public class Program
    {
        public static ServiceManager Server;

        public static void Main()
        {
            Server = new ServiceManager(LogType.Output, LogLevel.Debug, @"\winfs");

            //INITIALIZING : Server Services
            Server.InterfaceAddress = System.Net.IPAddress.GetDefaultLocalAddress().ToString();
            Server.ServerName = "example";
            Server.DnsSuffix = "iot.local";

            // SERVICES:  Enable / disable additional services
            Server.DhcpEnabled = false;
            Server.DnsEnabled = false;
            Server.SntpEnabled = false;

            //SERVICE:  Enable / disable directory browsing
            Server.AllowListing = false;

            //SERVICE: DHCP
            Server.DhcpService.PoolRange("172.16.10.100", "172.16.10.254");
            Server.DhcpService.GatewayAddress = "172.16.10.1";
            Server.DhcpService.SubnetMask = "255.255.255.0";

            //SERVICES: Start all serivces
            Server.StartAll();

            string url = string.Concat("http://", Server.HttpService.InterfaceAddress, ":", "81");
            Server.LogInfo ("MicroServer.Sample", "Launch Web Browser: " + url);
        }
    }
}
