namespace Bytewizer.TinyCLR.Hardware
{
    public class Mainboard
    {
        public static IMainboard Initialize()
        {
            var device = DeviceProvider.Connect();
            device.Network.Enabled();

            return device;
        }
    }
}