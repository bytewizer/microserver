
namespace MicroServer.Net.Dhcp
{
    /// <summary>
    /// The dhcp defined options.
    /// </summary>
    public enum DhcpOption : byte
    {
        Pad = 0x00,
        SubnetMask = 0x01,
        TimeOffset = 0x02,
        Router = 0x03,
        NTPServer = 0x2A,
        NameServer = 0x05,
        DomainNameServer = 0x06,
        Hostname = 0x0C,
        DomainNameSuffix = 0x0F,
        AddressRequest = 0x32,
        AddressTime = 0x33,
        ServerIdentifier = 0x36,
        DhcpMessageType = 0x35,
        ParameterList = 0x37,
        DhcpMessage = 0x38,
        Renewal = 0x3A,
        Rebinding = 0x3B,
        DhcpMaxMessageSize = 0x39,
        ClassId = 0x3C,
        ClientId = 0x3D,
        AutoConfig = 0x74,
        End = 0xFF
    }
}
