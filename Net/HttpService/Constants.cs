using System;

namespace MicroServer.Net.Http
{
    public static class Constants
    {
        public static int HTTP_SERVER_PORT { get { return 80; } }
        public static int HTTP_LISTEN_BACKLOG {get {return 10; } }
        public static int HTTP_MAX_MESSAGE_SIZE { get { return 65535; } }
        public static int HTTP_MIN_MESSAGE_SIZE { get { return 0; } }
        public static int HTTP_RECEIVE_TIMEOUT { get { return -1; } }
        public static int HTTP_SEND_TIMEOUT { get { return -1; } }
    }
}
