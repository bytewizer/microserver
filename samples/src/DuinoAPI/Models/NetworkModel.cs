namespace Bytewizer.TinyCLR.DuinoAPI
{
    public class NetworkModel
    {
        public NetworkModel()
        {
            var settings = NetworkProvider.Controller.GetIPProperties();

            Address = settings.Address.GetAddressBytes();
            GatewayAddress = settings.GatewayAddress.GetAddressBytes();
            SubnetMask = settings.SubnetMask.GetAddressBytes();
        }

        public byte[] Address { get; private set; }
        public byte[] GatewayAddress { get; private set; }
        public byte[] SubnetMask { get; private set; }
    }
}