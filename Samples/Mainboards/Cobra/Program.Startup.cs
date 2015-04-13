namespace MicroServer.CobraII
{
    using Gadgeteer;
    using GHI.Networking;
    using GHI.Pins;
    using GHI.IO.Storage;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using Microsoft.SPOT.Net.NetworkInformation;

    using GTM = Gadgeteer.Modules;
    using G120 = GHI.Pins.G120;


    public partial class Program : Gadgeteer.Program
    {

        /// <summary>The Character Display module using socket 1 of the mainboard.</summary>
        //private Gadgeteer.Modules.GHIElectronics.CharacterDisplay characterDisplay;
        private Gadgeteer.Modules.GHIElectronics.CyclingDisplay cyclingDisplay;

        /// <summary>The WiFi RS9110 module on the mainboard.</summary>
        private GHI.Networking.WiFiRS9110 netWifi;

        /// <summary>This property provides access to the Mainboard API. This is normally not necessary for an end user program.</summary>
        protected new static GHIElectronics.Gadgeteer.FEZCobraIIEco Mainboard
        {
            get
            {
                return ((GHIElectronics.Gadgeteer.FEZCobraIIEco)(Gadgeteer.Program.Mainboard));
            }
            set
            {
                Gadgeteer.Program.Mainboard = value;
            }
        }

        /// <summary>This method runs automatically when the device is powered, and calls ProgramStarted.</summary>
        public static void Main()
        {
            // Important to initialize the Mainboard first
            Program.Mainboard = new GHIElectronics.Gadgeteer.FEZCobraIIEco();
            Program p = new Program();
            p.InitializeModules();
            p.ProgramStarted();
            // Starts Dispatcher
            p.Run();
        }

        private void InitializeModules()
        {
            //this.characterDisplay = new GTM.GHIElectronics.CharacterDisplay(1);
            this.cyclingDisplay = new GTM.GHIElectronics.CyclingDisplay(1);
            this.netWifi = new WiFiRS9110(SPI.SPI_module.SPI2, G120.P1_10, G120.P2_11, G120.P1_9);
        }
    }
}
