using System;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using MicroServer.Service;
using MicroServer.Net.Http;
using MicroServer.Net.Dhcp;
using MicroServer.Net.Dns;
using MicroServer.Net.Sntp;
using MicroServer.Net.Http.Authentication;

namespace Test.Harness
{
    public class Program
    {
        public static ServiceManager Server;

        public static void Main()
        {
            Server = new ServiceManager(LogType.Output, LogLevel.Info, @"\winfs");
     
            //INITIALIZING : Server Services
            Server.InterfaceAddress = System.Net.IPAddress.GetDefaultLocalAddress().ToString();
            Server.ServerName = "example";
            Server.DnsSuffix = "iot.local";

            // SERVICES:  disable services 
            Server.HttpEnabled = false;

            // SERVICES INIT: quickly enable/disable services
            HttpInit();
            //DhcpInit();
            //DnsInit();
            //SntpInit();

            // SERVICES: Start all services
            Server.StartAll();

            // Test Harness: Set local time
            SntpClient TimeService = new SntpClient();
            TimeService.Synchronize();
            
            // enables basic authentication on server using AccountService class for user name and password
            //Server.HttpService.Add(new AuthenticationModule(new BasicAuthentication(new AccountService(), Server.ServerName)));
        }

        #region Protocol Initialization

        private static void HttpInit()
        {
            //SERVICE: HTTP
            Server.HttpEnabled = true;
            Server.AllowListing = false;

            string url = string.Concat("http://", Server.HttpService.InterfaceAddress, ":", "81");
            Server.LogInfo("Test.Harness", "Launch Web Browser: " + url);
        }

        private static void DhcpInit()
        {
            //SERVICE: DHCP
            Server.DhcpEnabled = true;

            // event handlers for dhcp service
            Server.DhcpService.OnDhcpMessageReceived += DhcpService_OnDhcpMessageReceived;
            Server.DhcpService.OnDhcpMessageSent += DhcpService_OnDhcpMessageSent;
            Server.DhcpService.OnLeaseAcknowledged += DhcpService_OnLeaseAcknowledged;
            Server.DhcpService.OnLeaseDeclined += DhcpService_OnLeaseDeclined;
            Server.DhcpService.OnLeaseReleased += DhcpService_OnLeaseReleased;

            Server.DhcpService.PoolRange("172.16.10.100", "172.16.10.254");
            Server.DhcpService.GatewayAddress = "172.16.10.1";
            Server.DhcpService.SubnetMask = "255.255.255.0";
        }

        private static void DnsInit()
        {
            //SERVICE: DNS
            Server.DnsEnabled = true;

            // event handlers for dns service
            Server.DnsService.OnDnsMessageReceived += DnsService_OnDnsMessageReceived;
            Server.DnsService.OnDnsMessageSent += DnsService_OnDnsMessageSent;

        }

        private static void SntpInit()
        {
            //SERVICE: SNTP
            Server.SntpEnabled = true;

            // event handlers for sntp service
            Server.SntpService.OnSntpMessageReceived += SntpService_OnSntpMessageReceived;
            Server.SntpService.OnSntpMessageSent += SntpService_OnSntpMessageSent;
        }

        #endregion Protocol Initialization

        #region Event Handlers

        static void DhcpService_OnDhcpMessageReceived(object sender, DhcpMessageEventArgs args)
        {
            Debug.Print("Test Harness: DHCP Message Received " + args.RequestMessage.ToString());
        }

        static void DhcpService_OnDhcpMessageSent(object sender, DhcpMessageEventArgs args)
        {
            Debug.Print("Test Harness: DHCP Message Sent " + args.RequestMessage.ToString());
        }

        static void DhcpService_OnLeaseAcknowledged(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("Test Harness: DHCP Lease Acknowledged " + args.Lease.ToString());
        }

        static void DhcpService_OnLeaseDeclined(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("Test Harness: DHCP Lease Declined " + args.Lease.ToString());
        }

        static void DhcpService_OnLeaseReleased(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("Test Harness: DHCP Lease Released " + args.Lease.ToString());
        }

        static void DnsService_OnDnsMessageReceived(object sender, DnsMessageEventArgs args)
        {
            Debug.Print("Test Harness: DNS Message Received " + args.RequestMessage.ToString());
        }

        static void DnsService_OnDnsMessageSent(object sender, DnsMessageEventArgs args)
        {
            Debug.Print("Test Harness: DNS Message Sent " + args.RequestMessage.ToString());
        }

        static void SntpService_OnSntpMessageReceived(object sender, SntpMessageEventArgs args)
        {
            Debug.Print("Test Harness: SNTP Message Received " + args.RequestMessage.ToString());
        }

        static void SntpService_OnSntpMessageSent(object sender, SntpMessageEventArgs args)
        {
            Debug.Print("Test Harness: SNTP Message Sent " + args.RequestMessage.ToString());
        }

        #endregion Event Handlers

    }
}
