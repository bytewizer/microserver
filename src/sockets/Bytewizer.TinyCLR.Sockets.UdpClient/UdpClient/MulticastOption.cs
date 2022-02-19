//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//


namespace System.Net.Sockets
{
    /// <summary>
    /// Contains option values for IP multicast management on socket 
    /// using <see cref="Socket.SetSocketOption(SocketOptionLevel, SocketOptionName, byte[])"/>
    /// </summary>
    public class MulticastOption
    {
        /// <summary>
        /// New instance of the MulticastOption class with the specified IP multicast address group
        /// </summary>
        /// <param name="multicastAddress">Multicast <see cref="IPAddress"/></param>
        public MulticastOption(IPAddress multicastAddress) : this(multicastAddress, IPAddress.Any)
        {
        }

        /// <summary>
        /// New instance of the MulticastOption class with the specified IP multicast address group and local address
        /// </summary>
        /// <param name="multicastAddress">Multicast <see cref="IPAddress"/></param>
        /// <param name="localAddress">Local <see cref="IPAddress"/> associated to multicast group</param>
        /// <exception cref="ArgumentNullException"><paramref name="localAddress"/> is null or <paramref name="multicastAddress"/> is null</exception>
        public MulticastOption(IPAddress multicastAddress, IPAddress localAddress)
        {
            MultiCastAddress = multicastAddress ?? throw new ArgumentNullException();
            LocalAddress = localAddress ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Multicast group <see cref="IPAddress"/>
        /// </summary>
        public IPAddress MultiCastAddress { get; }

        /// <summary>
        /// Local <see cref="IPAddress"/>
        /// </summary>
        public IPAddress LocalAddress { get; }

        /// <summary>
        /// Get binary encoded value of the <see cref="MulticastOption"/> object
        /// </summary>
        /// <returns>Byte array for use in calls to <see cref="Socket.SetSocketOption(SocketOptionLevel, SocketOptionName, byte[])"/></returns>
        public byte[] GetBytes()
        {
            byte[] optionValue = new byte[8];
            byte[] mcBytes = MultiCastAddress.GetAddressBytes();
            byte[] localBytes = LocalAddress.GetAddressBytes();
            
            for (int i = 0; i < 4; i++)
            {
                optionValue[i] = mcBytes[i];
                optionValue[i + 4] = localBytes[i];
            }

            return optionValue;
        }
    }
}
