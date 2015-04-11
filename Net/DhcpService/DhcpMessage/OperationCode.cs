using System.Collections;

namespace MicroServer.Net.Dhcp
{
    /// <summary>
    /// The operation code (OP) type.
    /// </summary>
    public enum OperationCode : byte
    {
        BootRequest = 0x01,
        BootReply = 0x02
    }

    /// <summary>
    /// A class that gets the name of the Operations Code (OP).
    /// </summary>
    public static class OperationString
    {
        public static string GetName(OperationCode OperationEnum)
        {
            switch (OperationEnum)
            {
                case OperationCode.BootRequest:
                    return "BootRequest";
                case OperationCode.BootReply:
                    return "BootReply";
                default:
                    return "Unknown";
            }
        }
    }
}
