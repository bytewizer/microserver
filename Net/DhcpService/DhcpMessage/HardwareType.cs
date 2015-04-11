
namespace MicroServer.Net.Dhcp
{
    /// <summary>
    /// The Hardware address (htype) type.
    /// </summary>
    public enum HardwareType : byte
    {
        Ethernet = 0x01
    }

    /// <summary>
    /// A class that gets the name of the hardware address type.
    /// </summary>
    public static class HardwareString
    {
        public static string GetName(HardwareType OperationEnum)
        {
            switch (OperationEnum)
            {
                case HardwareType.Ethernet:
                    return "Ethernet";
                default:
                    return "Unknown";
            }
        }
    }
}
