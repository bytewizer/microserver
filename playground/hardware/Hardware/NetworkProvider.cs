using System;
using System.Text;
using System.Diagnostics;
using System.Threading;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground
{
    public class NetworkProvider
    {
        private static bool _initialized;
        private static readonly object _lock = new object();

        public static NetworkController Controller { get; private set; }

        /// <summary>
        /// Initialize SC20260D development board onboard ethernet.
        /// </summary>
        public static void InitializeEthernet()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                var gpioController = GpioController.GetDefault();
                var resetPin = gpioController.OpenPin(SC20260.GpioPin.PG3);

                resetPin.SetDriveMode(GpioPinDriveMode.Output);

                resetPin.Write(GpioPinValue.Low);
                Thread.Sleep(100);

                resetPin.Write(GpioPinValue.High);
                Thread.Sleep(100);

                Controller = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

                var networkInterfaceSetting = new EthernetNetworkInterfaceSettings
                {
                    MacAddress = new byte[] { 0x00, 0x8D, 0xB4, 0x49, 0xAD, 0xBD },

                    DhcpEnable = true,
                    //DynamicDnsEnable = true,
                    //Address = new IPAddress(new byte[] { 192, 168, 1, 200 }),
                    //SubnetMask = new IPAddress(new byte[] { 255, 255, 255, 0 }),
                    //GatewayAddress = new IPAddress(new byte[] { 192, 168, 1, 1 }),
                    //DnsAddresses = new IPAddress[]{
                    //new IPAddress(new byte[] { 8, 8, 8, 8 }),
                    //new IPAddress(new byte[] { 8, 8, 4, 4 })
                    //}
                };

                Controller.SetInterfaceSettings(networkInterfaceSetting);
                Controller.SetAsDefaultController();
                Controller.NetworkAddressChanged += NetworkAddressChanged;

                Controller.Enable();

                _initialized = true;
            }
        }

        /// <summary>
        /// Initialize Wifi click installed in slot 1 on SC20260D development board.
        /// </summary>
        public static void InitializeWiFiClick(string ssid, string password)
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                var enablePin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PI0);
                enablePin.SetDriveMode(GpioPinDriveMode.Output);
                enablePin.Write(GpioPinValue.High);

                Controller = NetworkController.FromName(SC20260.NetworkController.ATWinc15x0);

                Controller.SetCommunicationInterfaceSettings(new SpiNetworkCommunicationInterfaceSettings()
                {
                    SpiApiName = SC20260.SpiBus.Spi3,
                    GpioApiName = SC20260.GpioPin.Id,
                    InterruptPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PG6),
                    InterruptEdge = GpioPinEdge.FallingEdge,
                    InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                    ResetPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PI8),
                    ResetActiveState = GpioPinValue.Low,
                    SpiSettings = new SpiConnectionSettings()
                    {
                        ChipSelectLine = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PG12),
                        ClockFrequency = 4000000,
                        Mode = SpiMode.Mode0,
                        ChipSelectType = SpiChipSelectType.Gpio,
                        ChipSelectHoldTime = TimeSpan.FromTicks(10),
                        ChipSelectSetupTime = TimeSpan.FromTicks(10)
                    }
                });

                Controller.SetInterfaceSettings(new WiFiNetworkInterfaceSettings()
                {
                    Ssid = ssid,
                    Password = password,
                });
                Controller.SetAsDefaultController();
                Controller.NetworkAddressChanged += NetworkAddressChanged;

                Controller.Enable();

                _initialized = true;
            }
        }

        /// <summary>
        /// Initialize onboard Wifi installed on FEZ Portal single board computers.
        /// </summary>
        public static void InitializeWiFi(string ssid, string password)
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                var enablePin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PA8);
                enablePin.SetDriveMode(GpioPinDriveMode.Output);
                enablePin.Write(GpioPinValue.High);

                Controller = NetworkController.FromName(SC20260.NetworkController.ATWinc15x0);

                Controller.SetCommunicationInterfaceSettings(new SpiNetworkCommunicationInterfaceSettings()
                {
                    SpiApiName = SC20260.SpiBus.Spi3,
                    GpioApiName = SC20260.GpioPin.Id,
                    InterruptPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PF10),
                    InterruptEdge = GpioPinEdge.FallingEdge,
                    InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                    ResetPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PC3),
                    ResetActiveState = GpioPinValue.Low,
                    SpiSettings = new SpiConnectionSettings()
                    {
                        ChipSelectLine = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PA6),
                        ClockFrequency = 4000000,
                        Mode = SpiMode.Mode0,
                        ChipSelectType = SpiChipSelectType.Gpio,
                        ChipSelectHoldTime = TimeSpan.FromTicks(10),
                        ChipSelectSetupTime = TimeSpan.FromTicks(10)
                    }
                });

                Controller.SetInterfaceSettings(new WiFiNetworkInterfaceSettings()
                {
                    Ssid = ssid,
                    Password = password,
                });

                Controller.SetAsDefaultController();
                Controller.NetworkAddressChanged += NetworkAddressChanged;

                Controller.Enable();
            }
        }

        /// <summary>
        /// Initialize onboard Wifi installed on FEZ Duino/Feather single board computers.
        /// </summary>
        public static void Initialize(string ssid, string password)
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                var enablePin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PA8);
                enablePin.SetDriveMode(GpioPinDriveMode.Output);
                enablePin.Write(GpioPinValue.High);

                Controller = NetworkController.FromName(SC20100.NetworkController.ATWinc15x0);

                Controller.SetCommunicationInterfaceSettings(new SpiNetworkCommunicationInterfaceSettings()
                {
                    SpiApiName = SC20100.SpiBus.Spi3,
                    GpioApiName = SC20100.GpioPin.Id,
                    InterruptPin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PB12),
                    InterruptEdge = GpioPinEdge.FallingEdge,
                    InterruptDriveMode = GpioPinDriveMode.InputPullUp,
                    ResetPin = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PB13),
                    ResetActiveState = GpioPinValue.Low,
                    SpiSettings = new SpiConnectionSettings()
                    {
                        ChipSelectLine = GpioController.GetDefault().OpenPin(SC20100.GpioPin.PD15),
                        ClockFrequency = 4000000,
                        Mode = SpiMode.Mode0,
                        ChipSelectType = SpiChipSelectType.Gpio,
                        ChipSelectHoldTime = TimeSpan.FromTicks(10),
                        ChipSelectSetupTime = TimeSpan.FromTicks(10)
                    }
                });

                Controller.SetInterfaceSettings(new WiFiNetworkInterfaceSettings()
                {
                    Ssid = ssid,
                    Password = password,
                });

                Controller.SetAsDefaultController();

                try
                {
                    Controller.Enable();
                    _initialized = true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public static string Info(NetworkController controller)
        {
            var ipProperties = controller.GetIPProperties();

            var sb = new StringBuilder();

            sb.Append($"Interface Address: {ipProperties.Address} ");
            sb.Append($"Subnet: {ipProperties.SubnetMask} ");
            sb.Append($"Gateway: {ipProperties.Address} ");

            for (int i = 0; i < ipProperties.DnsAddresses.Length; i++)
            {
                sb.Append($"DNS: {ipProperties.DnsAddresses[i]} ");
            }

            return sb.ToString();
        }

        private static void NetworkAddressChanged(
            NetworkController sender,
            NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                Debug.WriteLine(Info(sender));
            }
        }
    }
}