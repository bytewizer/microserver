using System;

namespace MicroServer.Net.Dhcp
{
    /// <summary>
    /// Contains global variables for project.
    /// </summary>
    public static class Constants
    {
        public static int DHCP_SERVICE_PORT { get { return 67; } }
        public static int DHCP_CLIENT_PORT { get { return 68; } }
        public static int DHCP_MIN_MESSAGE_SIZE { get { return 236; } }
        public static int DHCP_MAX_MESSAGE_SIZE { get { return 1024; } }
        public static int DHCP_RECEIVE_TIMEOUT { get { return -1; } }
        public static int DHCP_SEND_TIMEOUT { get { return -1; } }
        public static uint DHCP_OPTIONS_MAGIC_NUMBER { get { return 1669485411; } }
        public static uint DHCP_WIN_OPTIONS_MAGIC_NUMBER { get { return 1666417251; } }
    }
}
