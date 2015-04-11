
namespace MicroServer.Net.Dhcp
{
    public enum LeaseState : int
    {
        Unassigned = 0,
        Released = 1,
        Offered = 2,
        Assigned = 3,
        Declined = 4,
        Static = 5
    }

    public static class LeaseStateString
    {
        public static string GetName(LeaseState LeaseStateEnum)
        {
            switch (LeaseStateEnum)
            {
                case LeaseState.Unassigned:
                    return "Unassigned";
                case LeaseState.Released:
                    return "Released";
                case LeaseState.Offered:
                    return "Offered";
                case LeaseState.Assigned:
                    return "Assigned";
                case LeaseState.Declined:
                    return "Declined";
                case LeaseState.Static:
                    return "Static";
                default:
                    return "Unknown";
            }
        }
    }
}
