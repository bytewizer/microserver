using System;

namespace MicroServer.Net.Dns
{
    /// <summary>
    /// Contains global variables for project.
    /// </summary>
    public static class Constants
    {
        public static int DNS_SERVICE_PORT { get { return 53; } }
        public static int DNS_MAX_MESSAGE_SIZE { get { return 512; } }
        public static int DNS_MIN_MESSAGE_SIZE { get { return 0; } }
        public static int DNS_RECEIVE_TIMEOUT { get { return 16000; } }
        public static int DNS_SEND_TIMEOUT { get { return 16000; } }
    }
}
