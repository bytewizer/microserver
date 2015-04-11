namespace MicroServer.Raptor
{
    using Gadgeteer;
    using GTM = Gadgeteer.Modules;

    public partial class Program : Gadgeteer.Program
    {
        /// <summary>The BreadBoard X1 module using socket 15 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.BreadBoardX1 breadBoard;

        /// <summary>The USB Client DP module using socket 8 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.USBClientDP usbClient;

        /// <summary>The Ethernet ENC28 module using socket 3 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.EthernetENC28 ethernet;

        /// <summary>The Multicolor LED module using socket 14 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolorLED;

        /// <summary>The Breakout TB10 module using socket 17 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.BreakoutTB10 breakout;

        /// <summary>The Button module using socket 18 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Button button;

        /// <summary>The SD Card module using socket 9 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.SDCard sdCard;

        /// <summary>The Display N18 module using socket 11 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.DisplayN18 display;

        /// <summary>This property provides access to the Mainboard API. This is normally not necessary for an end user program.</summary>
        protected new static GHIElectronics.Gadgeteer.FEZRaptor Mainboard
        {
            get
            {
                return ((GHIElectronics.Gadgeteer.FEZRaptor)(Gadgeteer.Program.Mainboard));
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
            Program.Mainboard = new GHIElectronics.Gadgeteer.FEZRaptor();
            Program p = new Program();
            p.InitializeModules();
            p.ProgramStarted();
            // Starts Dispatcher
            p.Run();
        }

        private void InitializeModules()
        {
            this.usbClient = new GTM.GHIElectronics.USBClientDP(8);
            this.display = new GTM.GHIElectronics.DisplayN18(11);
            this.ethernet = new GTM.GHIElectronics.EthernetENC28(3);
            this.sdCard = new GTM.GHIElectronics.SDCard(9);
            this.multicolorLED = new GTM.GHIElectronics.MulticolorLED(14);
            this.breakout= new GTM.GHIElectronics.BreakoutTB10(17);
            this.breadBoard = new GTM.GHIElectronics.BreadBoardX1(15);
            this.button = new GTM.GHIElectronics.Button(18);
        }
    }
}
