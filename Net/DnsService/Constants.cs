using System;
using System.Net;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// Contains global variables for project.
    /// </summary>
    public static class Constants
    {
        public static int DNS_SERVICE_PORT { get { return 53; } }
        public static int DNS_CLIENT_PORT { get { return 53; } }
        public static int DNS_MAX_MESSAGE_SIZE { get { return 512; } }
        public static int DNS_MIN_MESSAGE_SIZE { get { return 0; } }
        public static int DNS_RECEIVE_TIMEOUT { get { return 2000; } }
        public static int DNS_SEND_TIMEOUT { get { return 2000; } }
        public static IPAddress DNS_DEFAULT_PRIMARY_SERVER { get { return IPAddress.Parse("8.8.8.8"); } }
        public static IPAddress DNS_DEFAULT_ALTERNATE_SERVER { get { return IPAddress.Parse("8.8.4.4"); } }
    }
}
