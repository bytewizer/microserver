using System;
using System.Text;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.IO;
using Microsoft.SPOT.Net.NetworkInformation;

using GHI.Networking;
using Gadgeteer;

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

            if (!netWifi.LinkConnected)
            {
                while (netWifi.NetworkInterface.IPAddress == "0.0.0.0")
                {
                    Debug.Print("CobraII: Waiting for DHCP...");
                    Thread.Sleep(1000);
                }
            }
            //INITIALIZING : MicroServer 
            InitMicroServer();
        }

        private void InitMicroServer()
        {

            // INITIALIZING : Server Services
            Server.ServerName = "cobra";
            Server.DnsSuffix = "iot.local";

            // SERVICES:  Enable / disable additional services
            Server.DhcpEnabled = true; // disabled by default as this could be disruptive to existing dhcp servers on the network.
            Server.DnsEnabled = true;
            Server.SntpEnabled = true;

            // SERVICE:  Enable / disable directory browsing
            Server.AllowListing = false;

            Server.DnsService.IsProxy = false;
            Server.SntpService.UseLocalTimeSource = true;

            // SERVICE: DHCP
            Server.DhcpService.PoolRange("192.168.50.100", "192.168.50.250");
            Server.DhcpService.GatewayAddress = "192.168.50.1";
            Server.DhcpService.SubnetMask = "255.255.255.0";
        }

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

        #endregion Startup

        #region Card Reader

        private void RemovableMedia_Eject(object sender, MediaEventArgs e)
        {
            sdCardVolume = null;
            Debug.Print("CobraII:  Card Ejected");
        }

        private void RemovableMedia_Insert(object sender, MediaEventArgs e)
        {
            sdCardVolume = e.Volume;
            Debug.Print("CobraII: Card Inserted.");
        }

        #endregion Card Reader

        #region Networking

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("CobraII:  Address for network interface changed");
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
    }
}
