 
namespace MicroServer.Net.Dhcp
{
    /// <summary>
    /// The message type.
    /// </summary>
    public enum MessageType : byte
    {
        Discover = 0x01,
        Offer = 0x02,
        Request = 0x03,
        Decline = 0x04,
        Ack = 0x05,
        Nak = 0x06,
        Release = 0x07,
        Inform = 0x08,
    }

    /// <summary>
    /// A class that gets the name of the Message Type.
    /// </summary>
    public static class MessageTypeString
    {
        public static string GetName(MessageType MessageTypeEnum)
        {
            switch (MessageTypeEnum)
            {
                case MessageType.Discover:
                    return "Discover";
                case MessageType.Offer:
                    return "Offer";
                case MessageType.Request:
                    return "Request";
                case MessageType.Decline:
                    return "Decline";
                case MessageType.Ack:
                    return "Ack";
                case MessageType.Nak:
                    return "Nak";
                case MessageType.Release:
                    return "Release";
                case MessageType.Inform:
                    return "Inform";
                default:
                    return "Unknown";
            }
        }
    }
}
