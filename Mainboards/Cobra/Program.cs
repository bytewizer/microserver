using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Touch;
using GHI.IO.Storage;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

using Microsoft.SPOT.Net.NetworkInformation;
using GHI.Networking;
using System.Text;

using Microsoft.SPOT.IO;

using Microsoft.SPOT.Hardware;
using Gadgeteer;
using System.IO;

using MicroServer.Service;
using MicroServer.Utilities;
using MicroServer.Net.Dhcp;
using MicroServer.Net.Dns;
using MicroServer.Net.Http;
using MicroServer.Net.Sntp;

namespace MicroServer.CobraII
{
    public partial class Program
    {

        private ServiceManager Server; 
        private static VolumeInfo sdCardVolume;

        void ProgramStarted()
        {

            //INITIALIZING : Memory Card
            RemovableMedia.Insert += new InsertEventHandler(RemovableMedia_Insert);
            RemovableMedia.Eject += new EjectEventHandler(RemovableMedia_Eject);

            //INITIALIZING : Wireless network
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

            //INITIALIZING : Startup
            Gadgeteer.Timer networkTimer = new Gadgeteer.Timer(500, Gadgeteer.Timer.BehaviorType.RunOnce);
            networkTimer.Tick += new Gadgeteer.Timer.TickEventHandler(TimerTick_Startup);
            networkTimer.Start();
        }

        #region Startup

        private void TimerTick_Startup(Gadgeteer.Timer timer)
        {
            //INITIALIZING : SD Storage
            Debug.Print("CobraII: Initializing memory card storage...");

            if (Mainboard.IsSDCardInserted && Mainboard.IsSDCardMounted)
            {
                StorageDevice sd = Mainboard.SDCardStorageDevice;

                if (sd.Volume.IsFormatted)
                {
                    Server = new ServiceManager(LogType.Output, LogLevel.Debug, sd.RootDirectory);
                }
            }
            else
            {
                Server = new ServiceManager(LogType.Output, LogLevel.Debug, null);
            }

            //INITIALIZING : Wireless
            Debug.Print("CobraII: Initializing wireless network...");

            if (!netWifi.Opened)
                netWifi.Open();

            WiFiRS9110.NetworkParameters wifiParms = new WiFiRS9110.NetworkParameters();
            wifiParms.NetworkType = WiFiRS9110.NetworkType.AdHoc;
            wifiParms.SecurityMode = WiFiRS9110.SecurityMode.Open;
            wifiParms.Ssid = "cobra";
            wifiParms.Key = "";                //GetWepBytes("1234567890");  // must be 10 or 26 digits
            wifiParms.Channel = 6;
            netWifi.StartAdHocNetwork(wifiParms);
            Debug.Print("CobraII: Started AdHoc Network: SSID=" + wifiParms.Ssid);

            netWifi.EnableStaticIP("192.168.50.1", "255.255.255.0", "192.168.50.1");
            netWifi.EnableStaticDns(new string[] { "192.168.50.1" });


            //netWifi.EnableStaticIP("192.168.10.6", "255.255.255.0", "192.168.10.1");
            //netWifi.EnableStaticDns(new string[] { "192.168.10.1" });

            //netWifi.NetworkInterface.EnableDhcp();
            //netWifi.NetworkInterface.EnableDynamicDns();

            //netWifi.Join("crytek","cd5gtefrd9976jgytfhto");
            //netWifi.NetworkInterface.Join("crytek-guest", "ricerice");

            if (!netWifi.LinkConnected)
            {
                while (netWifi.NetworkInterface.IPAddress == "0.0.0.0")
                {
                    Debug.Print("CobraII: Waiting for DHCP...");
                    Thread.Sleep(1000);
                }
            }

            //INITIALIZING : MicroServer 
            StartMicroServer();
        }

        private void StartMicroServer()
        {

            // set settings file location
            ConfigManager.Load(Server.StorageRoot + @"\settings.xml");

            // general interface information settings
            Server.DnsSuffix = ConfigManager.GetSetting("dnsSuffix", "iot.local");
            Server.ServerName = ConfigManager.GetSetting("serverName", "cobra");

            // enable the services you would like to start
            Server.HttpEnabled = true;
            Server.SntpService.UseLocalTimeSource = true;

            // dhcp server settings
            bool dhcpEnabled;
            if (ParseUtility.TryParseBool(ConfigManager.GetSetting("dhcpEnabled", "true"), out dhcpEnabled))
            {
                Server.DhcpEnabled = dhcpEnabled;
                Server.DhcpService.PoolRange(
                    ConfigManager.GetSetting("startAddress", "192.168.50.100"),
                    ConfigManager.GetSetting("endAddress", "192.168.50.250")
                    );
                Server.DhcpService.GatewayAddress = Server.InterfaceAddress;
                Server.DhcpService.SubnetMask = ConfigManager.GetSetting("subnetMask", "255.255.255.0");

            }

            // dns server settings
            bool dnsEnabled;
            if (ParseUtility.TryParseBool(ConfigManager.GetSetting("dnsEnabled", "true"), out dnsEnabled))
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
        }
        
        #endregion Startup

        #region Card Reader

        private void RemovableMedia_Eject(object sender, MediaEventArgs e)
        {
            sdCardVolume = null;
            Debug.Print("Card Ejected");
        }

        private void RemovableMedia_Insert(object sender, MediaEventArgs e)
        {
            sdCardVolume = e.Volume;
            Debug.Print("Card Inserted.");
        }

        //private void RemovableMedia_Insert(object sender, MediaEventArgs e)
        //{
        //    if (!e.Volume.IsFormatted)
        //    {
        //        Debug.Print("CobraII -> Media is not formatted. Formatting card...");
        //        Thread.Sleep(5000);
        //        try
        //        {
        //            sdCardStorageDevice.Mount();
        //            Thread.Sleep(2000);

        //            e.Volume.Format("FAT", 0, "SD", true);

        //            Debug.Print("CobraII -> Formatting completed");
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.Print("CobraII -> Formatting failed: " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        Debug.Print("CobraII -> Media inserted is formatted volume: " + e.Volume.Name);
        //    }
        //}

        //private void RemovableMedia_Eject(object sender, MediaEventArgs e)
        //{
        //    Debug.Print("CobraII -> Media ejected");
        //}

        #endregion Card Reader

        #region Networking

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("CobraII: Address for network interface changed");
            Debug.Print("CobraII:  Dhcp enabled: " + netWifi.NetworkInterface.IsDhcpEnabled);
            Debug.Print("CobraII:  Dynamic dns enabled: " + netWifi.NetworkInterface.IsDynamicDnsEnabled);
            Debug.Print("CobraII:  Network Interface type " + netWifi.NetworkInterface.NetworkInterfaceType);
            Debug.Print("CobraII:  IP address: " + netWifi.NetworkInterface.IPAddress);
            Debug.Print("CobraII:  Subnet mask: " + netWifi.NetworkInterface.SubnetMask);
            Debug.Print("CobraII:  Default gateway: " + netWifi.NetworkInterface.GatewayAddress);
            for (int i = 0; i < netWifi.NetworkInterface.DnsAddresses.Length; i++)
                Debug.Print("CobraII:  DNS server(" + i.ToString() + "): " + netWifi.NetworkInterface.DnsAddresses[i]);

        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable == true)
            {
                cyclingDisplay.AddMessage("key1", "IP Address" + "\n" + netWifi.NetworkInterface.IPAddress);
                cyclingDisplay.AddMessage("key2", "Host Name" + "\n" + "cobra");
                cyclingDisplay.AddMessage("key3", "SSID" + "\n" + "cobra");
                cyclingDisplay.AddMessage("key4", "Key" + "\n" + "12345");

                Server.InterfaceAddress = netWifi.NetworkInterface.IPAddress;
                Server.DhcpService.NameServers = new NameServerCollection(netWifi.NetworkInterface.IPAddress);

                // start all enabled services
                Server.StartAll();
                cyclingDisplay.StartStopCycle(true);
            }
            else
            {
                // stop all enabled services
                Server.StopAll();
                cyclingDisplay.StartStopCycle(false);
            }
        }

        #endregion Networking

        #region Event Handlers

        static void DhcpService_OnDhcpMessageReceived(object sender, DhcpMessageEventArgs args)
        {
            Debug.Print("CobraII: DHCP Message Received " + args.RequestMessage.ToString());
        }

        static void DhcpService_OnDhcpMessageSent(object sender, DhcpMessageEventArgs args)
        {
            Debug.Print("CobraII: DHCP Message Sent " + args.RequestMessage.ToString());
        }

        static void DhcpService_OnLeaseAcknowledged(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("CobraII: DHCP Lease Acknowleged " + args.Lease.ToString());
        }

        static void DhcpService_OnLeaseDeclined(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("CobraII: DHCP Lease Declined " + args.Lease.ToString());
        }

        static void DhcpService_OnLeaseReleased(object sender, DhcpLeaseEventArgs args)
        {
            Debug.Print("CobraII: DHCP Lease Released " + args.Lease.ToString());
        }

        static void DnsService_OnDnsMessageReceived(object sender, DnsMessageEventArgs args)
        {
            Debug.Print("CobraII: DNS Message Received " + args.RequestMessage.ToString());
        }

        static void DnsService_OnDnsMessageSent(object sender, DnsMessageEventArgs args)
        {
            Debug.Print("CobraII: DNS Message Sent " + args.RequestMessage.ToString());
        }

        static void SntpService_OnSntpMessageReceived(object sender, SntpMessageEventArgs args)
        {
            Debug.Print("CobraII: SNTP Message Received " + args.RequestMessage.ToString());
        }

        static void SntpService_OnSntpMessageSent(object sender, SntpMessageEventArgs args)
        {
            Debug.Print("CobraII: SNTP Message Sent " + args.RequestMessage.ToString());
        }

        #endregion Event Handlers

        //#region Helpers
        //static void DisplayNetworkType(GHINET.WiFiNetworkInfo info)
        //{
        //    switch (info.networkType)
        //    {
        //        case GHINET.NetworkType.AccessPoint:
        //            {
        //                Debug.Print("Wireless Network Type: Access Point");
        //                break;
        //            }
        //        case GHINET.NetworkType.AdHoc:
        //            {
        //                Debug.Print("Wireless Network Type: Ad Hoc");
        //                break;
        //            }
        //    }
        //}

        //static void DisplayMACAddress(GHINET.WiFiNetworkInfo info)
        //{
        //    string MAC = new string(null);
        //    int splitCount = 0;

        //    for (int i = 0; i < info.PhysicalAddress.Length; i++)
        //    {
        //        MAC += (info.PhysicalAddress[i].ToString("X"));

        //        if (splitCount < 5)
        //        {
        //            MAC += ":";
        //            splitCount++;
        //        }
        //    }

        //    Debug.Print("MAC Address: " + MAC);
        //}

        //static void DisplaySecurityMode(GHINET.WiFiNetworkInfo info)
        //{
        //    switch (info.SecMode)
        //    {
        //        case GHINET.SecurityMode.Open:
        //            {
        //                Debug.Print("Wireless Security Mode: Open");
        //                break;
        //            }
        //        case GHINET.SecurityMode.WEP:
        //            {
        //                Debug.Print("Wireless Security Mode: WEP");
        //                break;
        //            }
        //        case GHINET.SecurityMode.WPA:
        //            {
        //                Debug.Print("Wireless Security Mode: WPA");
        //                break;
        //            }
        //        case GHINET.SecurityMode.WPA2:
        //            {
        //                Debug.Print("Wireless Security Mode: WPA2");
        //                break;
        //            }
        //    }
        //}
        //#endregion


        static string GetWepBytes(string WepPass)
        {
            string WepKey = string.Empty;
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(WepPass);
            foreach (byte item in bytes)
            {
                WepKey += ByteToHex(item);
            }
            return WepKey;
        }

        static string ByteToHex(byte value)
        {
            string[] arrHex = { "A", "B", "C", "D", "E", "F" };
            string ByteHex = string.Empty;
            byte[] nibbles = { (byte)(value >> 4), (byte)(value & 0x0F) };

            foreach (byte nibble in nibbles)
            {
                switch (10 <= nibble)
                {
                    case true:
                        ByteHex += arrHex[nibble - 10];
                        break;
                    default:
                        ByteHex += nibble.ToString();
                        break;
                }
            }
            return ByteHex;
        }

    }
}
