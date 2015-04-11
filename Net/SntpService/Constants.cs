using System;

namespace MicroServer.Net.Sntp
{
    /// <summary>
    /// Contains global variables for project.
    /// </summary>
    public static class Constants
    {
        public static int SNTP_SERVICE_PORT { get { return 123; } }
        public static int SNTP_CLIENT_PORT { get { return 123; } }
        public static int SNTP_MAX_MESSAGE_SIZE { get { return 68; } }
        public static int SNTP_MIN_MESSAGE_SIZE { get { return 48; } }
        public static int SNTP_RECEIVE_TIMEOUT { get { return -1; } }
        public static int SNTP_SEND_TIMEOUT { get { return -1; } }
        public static string SNTP_DEFAULT_PRIMARY_SERVER { get { return "0.pool.ntp.org"; } }
        public static string SNTP_DEFAULT_ALTERNATE_SERVER { get { return "time.windows.com"; } }
    }
}
