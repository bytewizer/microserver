using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

using Microsoft.SPOT.Net.NetworkInformation;
using GHI.Networking;
using System.Text;

using Microsoft.SPOT.IO;
using MicroServer.Logging;

using MicroServer.Service;
using MicroServer.Net.Dhcp;


namespace MicroServer.Raptor
{
    public partial class Program
    {

        ServiceManager Server = new ServiceManager(LogType.Output, LogLevel.Info, @"\sd");
   
        void ProgramStarted()
        {
            //INITIALIZING : Status LED
            multicolorLED.GreenBlueSwapped = true; 
            multicolorLED.BlinkRepeatedly(GT.Color.Blue);

            //INITIALIZING : Display
            display.BacklightEnabled = false;

            //INITIALIZING : Ethernet Interface
            ethernet.UseThisNetworkInterface();
            ethernet.UseStaticIP("172.16.10.1", "255.255.255.0", "172.16.10.1", new string[] { "172.16.10.1" });

            //INITIALIZING : Server Services
            Server.InterfaceAddress = ethernet.NetworkInterface.IPAddress;
            Server.ServerName = "raptor";
            Server.DnsSuffix = "iot.local";

            //SERVICE: DHCP
            Server.DhcpEnabled = true;
            Server.DhcpService.PoolRange("172.16.10.100", "172.16.10.254");
            Server.DhcpService.GatewayAddress = "172.16.10.1";
            Server.DhcpService.SubnetMask = "255.255.255.0";
            Server.DhcpService.NameServers = new NameServerCollection("172.16.10.1");

            //INITIALIZING : Memory Card
            RemovableMedia.Insert += new InsertEventHandler(RemovableMedia_Insert);
            RemovableMedia.Eject += new EjectEventHandler(RemovableMedia_Eject);
            sdCard.Mounted += new SDCard.MountedEventHandler(cardReader_Mounted);
            sdCard.Unmounted += new SDCard.UnmountedEventHandler(cardReader_Unmounted);
            
            //INITIALIZING : Ethernet Network
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

            //INITIALIZING : Led Status
            Gadgeteer.Timer ledTimer = new Gadgeteer.Timer(5000, Gadgeteer.Timer.BehaviorType.RunContinuously);
            ledTimer.Tick += new Gadgeteer.Timer.TickEventHandler(TimerTick_StatusLed);
            ledTimer.Start(); 
        }

        #region Status

        private void TimerTick_StatusLed(Gadgeteer.Timer timer)
        {
            if (sdCard.IsCardMounted && ethernet.IsNetworkConnected)
            {
                multicolorLED.TurnBlue();
            }
            else
            {
                if (!sdCard.IsCardMounted || !sdCard.IsCardInserted)
                {
                    multicolorLED.BlinkRepeatedly(GT.Color.Blue, TimeSpan.FromTicks(2000000), GT.Color.Yellow, TimeSpan.FromTicks(2000000));
                }

                if (!ethernet.IsNetworkConnected || !ethernet.IsNetworkUp)
                {
                    multicolorLED.BlinkRepeatedly(GT.Color.Blue, TimeSpan.FromTicks(2000000), GT.Color.Red, TimeSpan.FromTicks(2000000));
                }
            }
        }

        #endregion Status

        #region Card Reader

        private void RemovableMedia_Insert(object sender, MediaEventArgs e)
        {
            if (!e.Volume.IsFormatted)
            {
                Logger.WriteInfo("RPTR", "Media is not formatted. Formatting card...");
                Thread.Sleep(5000);
                try
                {
                    sdCard.Mount();
                    Thread.Sleep(2000);

                    e.Volume.Format("FAT", 0, "SD", true);

                    Logger.WriteInfo("RPTR", "Formatting completed");
                }
                catch (Exception ex)
                {
                    Logger.WriteError("RPTR", "Formatting failed ", ex);
                }
            }
            else
            {
                Logger.WriteInfo("RPTR", "Media inserted is formatted with volume: " + e.Volume.Name);
            }
        }

        private void RemovableMedia_Eject(object sender, MediaEventArgs e)
        {
            Logger.WriteInfo("RPTR", "Media ejected");
        }

        private void cardReader_Unmounted(SDCard sender, EventArgs e)
        {
            Logger.WriteInfo("RPTR", "Media unmounted");
        }

        private void cardReader_Mounted(SDCard sender, Gadgeteer.StorageDevice device)
        {
            if (sender.StorageDevice.Volume.IsFormatted)
            {
                if (sdCard.IsCardMounted)
                {
                    Logger.WriteInfo("RPTR", "Media directory/file listing: ");
                    FileListing(sdCard.StorageDevice.RootDirectory);
                }
            }
        }

        private void Flush()
        {
            try
            {
                sdCard.StorageDevice.Volume.FlushAll();
            }
            catch (Exception ex)
            {
                Logger.WriteError("RPTR", "FlushAll failed", ex);
            }
        }

        private void FileListing(string directories)
        {
            try
            {
                foreach (string file in sdCard.StorageDevice.ListFiles(directories))
                {
                    Logger.WriteInfo(" ~" + file.ToLower());
                }
                foreach (string directory in sdCard.StorageDevice.ListDirectories(directories))
                {
                    Logger.WriteInfo("  ~" + directory.ToLower());
                    FileListing(directory);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("RPTR", "Error parsing files", ex);
            }
        }

        #endregion Card Reader

        #region Networking

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Logger.WriteInfo("RPTR", "Network availability: " + e.IsAvailable.ToString());

            if (e.IsAvailable == true)
            {
                Server.StartAll();
            }
            else
            {
                Server.StopAll();
            }
        }
        
        #endregion Networking
    }
}
