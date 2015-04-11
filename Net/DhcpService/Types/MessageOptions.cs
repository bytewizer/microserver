using System;

namespace MicroServer.Net.Dhcp
{
    public struct MessageOptions
    {
        public byte[] MessageType;
        public byte[] ClientId;
        public byte[] HostName;
        public byte[] AddressRequest;
        public byte[] ServerIdentifier;
        public byte[] ParameterList;
    }
}
