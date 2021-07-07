using System;
using System.Net;

namespace Bytewizer.TinyCLR.Sntp.Internal
{
    internal class IPAddressHelper
    {
        /// <summary>
        /// Determines whether a string is a valid IP address.
        /// </summary>
        /// <param name="ipString">The string to validate.</param>
        /// <param name="address">The IPAddress version of the string.</param>
        public static bool TryParse(string ipString, out IPAddress address)
        {
            address = IPAddress.Any;
            if (ipString.Length > 6 && ipString.Contains("."))
            {
                string[] s = ipString.Split('.');
                if (s.Length == 4 && s[0].Length > 0 && s[1].Length > 0 && s[2].Length > 0 && s[3].Length > 0)
                {
                    try
                    {
                        address = IPAddress.Parse(ipString);
                        return true;
                    }
                    catch
                    {
                        address = null;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Converts a byte array into a IPAddress.
        /// </summary>
        /// <param name="data">The data at which to convert.</param>
        /// <returns>An <see cref="IPAddress"/> converted from a byte array.</returns>
        public static IPAddress ToIPAddress(byte[] data)
        {
            try
            {
                return new IPAddress(data);
            }
            catch
            {
                return IPAddress.Any;
            }
        }

        /// <summary>
        /// Converts a <see cref="IPAddress"/> into a <see cref="uint"/>.
        /// </summary>
        /// <param name="address">The <see cref="IPAddress"/> at which to convert.</param>
        /// <returns>A <see cref="IPAddress"/> converted from a byte array.</returns>
        public static uint FromIPAddress(IPAddress address)
        {
            try
            {
                return BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            }
            catch
            {
                return BitConverter.ToUInt32(new IPAddress(new byte[4] { 0, 0, 0, 0 }).GetAddressBytes(), 0);
            }
        }

        public static bool TryResolve(string hostName, out IPAddress address)
        {
            int retry;

            address = IPAddress.Any;

            while (true)
            {
                retry = 0;

                try
                {
                    var hostEntry = Dns.GetHostEntry(hostName);

                    if ((hostEntry != null) && (hostEntry.AddressList.Length > 0))
                    {
                        var i = 0;
                        while (hostEntry.AddressList[i] == null) i++;
                        {
                            address = hostEntry.AddressList[i];
                            return true;
                        }
                    }
                    else 
                    {
                        return false;
                    }
                }
                catch
                {
                    if (retry > 3)
                    {
                        return false;
                    }

                    retry++;
                    continue;
                }
            }
        }
    }
}