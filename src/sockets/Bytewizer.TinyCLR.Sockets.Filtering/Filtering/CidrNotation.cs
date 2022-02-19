using System;
using System.Net;

namespace Bytewizer.TinyCLR.Sockets.Filtering
{
    /// <summary>
    /// Type to represent a CIDR notation (e.g. "120.30.24.123/20").
    /// </summary>
    internal sealed class CidrNotation
    {
        /// <summary>
        /// The IP address part of the CIDR notation.
        /// </summary>
        public readonly IPAddress Address;

        /// <summary>
        /// The mask bits of the CIDR notation.
        /// </summary>
        public readonly int MaskBits;

        /// <summary>
        /// Parses a given <paramref name="cidrNotation"/> string to a type of <see cref="CidrNotation"/>.
        /// </summary>
        public static CidrNotation Parse(string cidrNotation) => new CidrNotation(cidrNotation);

        /// <summary>
        /// Checks if an <paramref name="address"/> is within the address space defined by this CIDR notation.
        /// </summary>
        public bool Contains(IPAddress address) =>
            CompareAddressBytes(Address.GetAddressBytes(), address.GetAddressBytes(), MaskBits);

        /// <summary>
        /// Converts the <see cref="CidrNotation"/> object into a <see cref="string"/> object.
        /// </summary>
        public override string ToString() => $"{Address}/{MaskBits}";

        private CidrNotation(string cidrNotation)
        {
            if (string.IsNullOrEmpty(cidrNotation))
            {
                throw new ArgumentException("A CIDR notation string cannot be null or empty.");
            }

            var parts = cidrNotation.Split('/');

            if (parts.Length != 2)
            {
                throw new ArgumentException($"Invalid CIDR notation: {cidrNotation}.");
            }

            try
            {
                var address = IPAddress.Parse(parts[0]);
                var mask = int.Parse(parts[1]);

                var maskBits = Convert.ToInt32(string.Format("{0:X}", mask), 16);
                var maxMaskBit = 32;

                if (maskBits < 0 || maskBits > maxMaskBit)
                {
                    throw new ArgumentException($"Invalid bits in CIDR notation: {maskBits}.");
                }

                Address = address;
                MaskBits = maskBits;
            }
            catch
            {
                throw new ArgumentException($"Invalid address in CIDR notation: {cidrNotation}.");
            }
        }

        private static bool CompareAddressBytes(byte[] cidr, byte[] address, int bits)
        {
            if (cidr.Length != address.Length)
            {
                return false;
            }

            var index = 0;

            for (; bits >= 8; bits -= 8)
            {
                if (address[index] != cidr[index])
                {
                    return false;
                }
                index++;
            }

            if (bits <= 0)
            {
                return true;
            }

            var mask = (byte)~(255 >> bits);

            return (address[index] & mask) == (cidr[index] & mask);
        }
    }
}