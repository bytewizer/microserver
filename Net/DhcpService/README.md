Example Micro Framework Console Application
-------------------------------------------
Start a new emulator console application, install Microsoft .NET Micro Framework DHCP Service and create a Program.cs file
with the following source code:

Nuget
-----
```
PM> Install-Package MicroServer.Net.Dhcp
```

## DHCP Service
```csharp
using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Dhcp;
namespace DhcpServer
{
    public class Program
    {
        // This service creates a full featured Dhcp server
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            //  Create the Dhcp server
            DhcpService DhcpServer = new DhcpService();

            // set the device server name and Dhcp suffix offered to client 
            DhcpServer.ServerName = "example";
            DhcpServer.DnsSuffix = "iot.local";

            // Set a Dhcp pool for the clients to use
            DhcpServer.PoolRange("172.16.10.100", "172.16.10.250");
            DhcpServer.GatewayAddress = "172.16.10.1";
            DhcpServer.SubnetMask = "255.255.255.0";

            // Set a Dhcp reservation that assigns as specific ip address to a client MAC address
            DhcpServer.PoolReservation("172.16.10.15", "000C29027338");

            // Add a NTP server option for time-a.nist.gov as client time source 
            DhcpServer.AddOption(DhcpOption.NTPServer, IPAddress.Parse("129.6.15.28").GetAddressBytes());

            // Sets interface ip address
            DhcpServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Starts Dhcp service
            DhcpServer.Start();
        }
    }
}
 ```