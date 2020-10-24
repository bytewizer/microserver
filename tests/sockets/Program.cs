using Bytewizer.TinyCLR.Sockets;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.Tests.Sockets
{
    class Program
    {
        static void Main()
        {
            InitializeEthernet();
            SetupHttpServer();
        }

        private static void InitializeEthernet()
        {
            var networkController = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings
            {
                MacAddress = new byte[] { 0x00, 0x8D, 0xA4, 0x49, 0xCD, 0xBD },
                IsDhcpEnabled = true,
                IsDynamicDnsEnabled = true
            };

            networkController.SetInterfaceSettings(networkInterfaceSetting);
            networkController.SetAsDefaultController();

            networkController.Enable();
        }

        static void SetupHttpServer()
        {
            var server = new SocketServer(options =>
            {
                options.Register(new HttpResponse());
            });
            server.Start();
        }
    }
}