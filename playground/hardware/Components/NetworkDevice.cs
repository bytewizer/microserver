using System;
using System.Net;
using System.Threading;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    public abstract class NetworkDevice : DisposableObject
    {
        private readonly AutoResetEvent _dhcpEvent = new AutoResetEvent(false);

        private readonly NetworkController _networkController;

        private readonly GpioPin _resetPin;

        private const int _resetDelay = 100;

        private int _timeout = 5000;

        protected NetworkDevice(NetworkController networkController, GpioPin resetPin)
        {
            // Set GPIO reset pin
            _resetPin = resetPin;

            // Set network controller
            _networkController = networkController;
            _networkController.SetAsDefaultController();
            _networkController.NetworkAddressChanged += NetworkAddressChanged;
            _networkController.NetworkLinkConnectedChanged += NetworkLinkConnectedChanged;
        }

        protected override void Dispose(bool disposing)
        {
            // Only managed resources to dispose
            if (!disposing)
                return;

            // Close device
            _resetPin?.Dispose();
            _networkController?.Dispose();
        }

        public bool IsLinkReady { get; private set; }

        public bool IsConnected { get; private set; }

        public NetworkInterfaceSettings Settings { get; set; }

        public void UseDHCP()
        {
            Settings.IsDhcpEnabled = true;
            Settings.IsDynamicDnsEnabled = true;

            Enabled();
        }

        public void UseStaticIP(string ip, string subnet, string gateway, string[] dns)
        {
            var address = new IPAddress[dns.Length];
            for (int i = 0; i < dns.Length; i++)
            {
                address[i] = IPAddress.Parse(dns[i]);
            }

            UseStaticIP(IPAddress.Parse(ip), IPAddress.Parse(subnet), IPAddress.Parse(gateway), address);
        }

        public void UseStaticIP(IPAddress ip, IPAddress subnet, IPAddress gateway, IPAddress[] dns)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            if (gateway == null)
                throw new ArgumentNullException(nameof(gateway));

            if (subnet == null)
                throw new ArgumentNullException(nameof(subnet));

            if (dns == null && dns.Length < 1)
                throw new ArgumentNullException(nameof(dns));

            Settings.Address = ip;
            Settings.SubnetMask = subnet;
            Settings.GatewayAddress = gateway;
            Settings.DnsAddresses = dns;
            Settings.IsDhcpEnabled = false;
            Settings.IsDynamicDnsEnabled = false;

            Enabled();
        }

        public int Timeout
        {
            get { return _timeout; }

            set
            {
                if (value <= 0) 
                    throw new ArgumentOutOfRangeException(nameof(value), "value must be positive.");

                _timeout = value;
            }
        }

        public void Reset()
        {
            // Toggle reset pin and wait for reset completion
            _resetPin.Write(GpioPinValue.Low);
            Thread.Sleep(_resetDelay);

            _resetPin.Write(GpioPinValue.High);
            Thread.Sleep(_resetDelay);
        }

        public void Enabled()
        {
            _networkController.Enable();

            if (_dhcpEvent.WaitOne(Timeout, true))
            {
                //Debug.WriteLine("Work method signaled.");
            }
            else
            {
                //Debug.WriteLine("Timed out waiting for work method to signal.");
            }
        }

        public void Disable()
        {
            _networkController.Disable();
        }

        private void NetworkLinkConnectedChanged(
            NetworkController sender,
            NetworkLinkConnectedChangedEventArgs args)
        {
            IsLinkReady = args.Connected;
        }

        private void NetworkAddressChanged(
            NetworkController sender,
            NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                Debug.WriteLine($"Network interface address assinged: {ipProperties.Address}");
            }

            IsConnected = true;
        }
    }
}