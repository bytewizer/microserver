
namespace MicroServer.Net.Dhcp
{
    public enum RequestState : int
    {
        Selecting = 0,
        Renewing = 1,
        Rebinding = 2,
        InitReboot = 3
    }

    public static class RequestStateString
    {
        public static string GetName(RequestState RequestStateEnum)
        {
            switch (RequestStateEnum)
            {
                case RequestState.Selecting:
                    return "SELECTING";
                case RequestState.Renewing:
                    return "RENEWING";
                case RequestState.Rebinding:
                    return "REBINDING";
                case RequestState.InitReboot:
                    return "INIT-REBOOT";
                default:
                    return "UNKNOWN";
            }
        }
    }
}
