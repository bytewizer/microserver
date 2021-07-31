using System;
using System.Text;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR
{
    public class NetworkProvider
    {
        private static bool _initialized;
        private static readonly object _lock = new object();

        public static NetworkController Controller { get; private set; }

        /// <summary>
        /// Initialize onboard Wifi installed on FEZ Duino single board computers.
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
    }
}