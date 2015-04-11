using System;
using System.Net;
using Microsoft.SPOT;

using MicroServer.Service;
using MicroServer.Net.Sntp;

namespace MicroServer.Example
{
    public class Program
    {
        public static void Main()
        {
            using (ServiceManager Server = new ServiceManager(LogType.Output, LogLevel.Debug, @"\winfs"))
            {  
                //INITIALIZING : Server Services
                Server.InterfaceAddress = System.Net.IPAddress.GetDefaultLocalAddress().ToString();
                Server.ServerName = "example";
                Server.DnsSuffix = "iot.local";
                Server.AllowListing = false;

                //SERVICE: DHCP
                Server.DhcpService.PoolRange("172.16.10.100", "172.16.10.254");
                Server.DhcpService.GatewayAddress = "172.16.10.1";
                Server.DhcpService.SubnetMask = "255.255.255.0";

                //SERVICES: Start all serivces
                Server.StartAll();
            }
        }
    }
}
