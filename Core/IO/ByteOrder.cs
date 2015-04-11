using System;

namespace MicroServer.IO
{
    public enum ByteOrder
    {
        /// <summary>Use the default byte order for the computer.</summary>
        Default = 0,

        /// <summary>Use big-endian byte order, also known as Motorola byte order.</summary>
        BigEndian = 1,

        /// <summary>Use little-endian byte order, also known as Intel byte order.</summary>
        LittleEndian = 2,

        /// <summary>Use Motorola byte order. Corresponds to <see cref="BigEndian"/>.</summary>
        Motorola = BigEndian,

        /// <summary>Use Intel byte order. Corresponds to <see cref="LittleEndian"/>.</summary>
        Intel = LittleEndian,

        /// <summary>The order which multi-byte values are transmitted on a network.</summary>
        Network = BigEndian
    }
}
