using Bytewizer.TinyCLR.Hardware.Boards;

namespace Bytewizer.TinyCLR.Hardware
{
    public class Mainboard
    {
        public static IHardware Connect(BoardModel model)
        {
            var device = DeviceProvider.Connect(model);
            device.Network.Enabled();

            return device;
        }
    }
}