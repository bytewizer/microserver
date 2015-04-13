using System;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using MicroServer.Service;
using MicroServer.Utilities;
using MicroServer.Net.Dhcp;
using MicroServer.Net.Dns;
using MicroServer.Net.Sntp;
using MicroServer.Net.Http;

namespace MicroServer.Emulator
{
    public class Program
    {
        public static void Main()
        {
            using (ServiceManager Server = new ServiceManager(LogType.Output, LogLevel.Info, @"\winfs"))
            {
                Server.LogInfo("MicroServer.Emulator", "Starting service manager main code");

                ConfigManager.Load(Server.StorageRoot + @"\settings.xml");

                // general interface information settings
                //Server.InterfaceAddress = ConfigManager.GetSetting("ipAddress", System.Net.IPAddress.GetDefaultLocalAddress().ToString());
                Server.InterfaceAddress = "192.168.10.8";
                Server.DnsSuffix = ConfigManager.GetSetting("dnsSuffix","iot.local");
                Server.ServerName = ConfigManager.GetSetting("serverName", "mfhost");

                // enable the services you would like to start
                Server.HttpEnabled = true;

                // dhcp server settings
                bool dhcpEnabled;
                if (ParseUtility.TryParseBool(ConfigManager.GetSetting("dhcpEnabled", "false"), out dhcpEnabled))
                {
                    Server.DhcpEnabled = dhcpEnabled;
                    Server.DhcpService.PoolRange(
                        ConfigManager.GetSetting("startAddress", "192.168.50.100"),
                        ConfigManager.GetSetting("endAddress", "192.168.50.254")
                        );
                    //Server.DhcpService.PoolReservation("192.168.10.99", "000C291DBB7D");
                    Server.DhcpService.GatewayAddress = Server.InterfaceAddress;
                    Server.DhcpService.SubnetMask = ConfigManager.GetSetting("subnetMask", "255.255.255.0");
                    Server.DhcpService.NameServers = new NameServerCollection(Server.InterfaceAddress);
                }

                // dns server settings
                bool dnsEnabled;
                if (ParseUtility.TryParseBool(ConfigManager.GetSetting("dnsEnabled", "false"), out dnsEnabled))
                    Server.DnsEnabled = dnsEnabled;

                // sntp server settings
                bool sntpEnabled;
                if (ParseUtility.TryParseBool(ConfigManager.GetSetting("sntpEnabled", "false"), out sntpEnabled))
                    Server.SntpEnabled = sntpEnabled;

                // event handlers for dhcp service
                Server.DhcpService.OnDhcpMessageReceived += DhcpService_OnDhcpMessageReceived;
                Server.DhcpService.OnDhcpMessageSent += DhcpService_OnDhcpMessageSent;
                Server.DhcpService.OnLeaseAcknowledged += DhcpService_OnLeaseAcknowledged;
                Server.DhcpService.OnLeaseDeclined += DhcpService_OnLeaseDeclined;
                Server.DhcpService.OnLeaseReleased += DhcpService_OnLeaseReleased;

                // event handlers for dns service
                Server.DnsService.OnDnsMessageReceived += DnsService_OnDnsMessageReceived;
                Server.DnsService.OnDnsMessageSent += DnsService_OnDnsMessageSent;

                // event handlers for sntp service
                Server.SntpService.OnSntpMessageReceived += SntpService_OnSntpMessageReceived;
                Server.SntpService.OnSntpMessageSent += SntpService_OnSntpMessageSent;

                // start all enabled services
                Server.StartAll();

                Server.LogInfo("MicroServer.Emulator", "Ending service manager main code");
            }

            if (SystemInfo.IsEmulator)
            {
                SntpClient TimeService = new SntpClient();
                TimeService.Synchronize();
            }
        }

        #region Event Handlers

        static void DhcpService_OnDhcpMessageReceived(object sender, DhcpMessageEventArgs args)
        {
            Debug.Print("Emulator: DHCP Message Received " + args.RequestMessage.ToString());
        }

        static void DhcpService_OnDhcpMessageSent(object sender, DhcpMessageEventArgs args)
        {
            Debug.Print("Emulator: DHCP Message Sent " + args.RequestMessage.ToString());
        }

        static void DhcpService_OnLeaseAcknowledged(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("Emulator: DHCP Lease Acknowleged " + args.Lease.ToString());
        }

        static void DhcpService_OnLeaseDeclined(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("Emulator: DHCP Lease Declined " + args.Lease.ToString());
        }

        static void DhcpService_OnLeaseReleased(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("Emulator: DHCP Lease Released " + args.Lease.ToString());
        }

        static void DnsService_OnDnsMessageReceived(object sender, DnsMessageEventArgs args)
        {
            Debug.Print("Emulator: DNS Message Received " + args.RequestMessage.ToString());
        }

        static void DnsService_OnDnsMessageSent(object sender, DnsMessageEventArgs args)
        {
            Debug.Print("Emulator: DNS Message Sent " + args.RequestMessage.ToString());
        }

        static void SntpService_OnSntpMessageReceived(object sender, SntpMessageEventArgs args)
        {
            Debug.Print("Emulator: SNTP Message Received " + args.RequestMessage.ToString());
        }

        static void SntpService_OnSntpMessageSent(object sender, SntpMessageEventArgs args)
        {
            Debug.Print("Emulator: SNTP Message Sent " + args.RequestMessage.ToString());
        }

        #endregion Event Handlers
    }
}
