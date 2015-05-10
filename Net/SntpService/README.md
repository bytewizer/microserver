Example Micro Framework Console Application
-------------------------------------------
Start a new emulator console application, install Microsoft .NET Micro Framework SNTP Service and create a Program.cs file
with the following source code:

Nuget
-----
```
PM> Install-Package MicroServer.Net.Sntp
```

## SNTP Service
 ```csharp
using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Sntp;

namespace SntpServer
{
    public class Program
    {
        // This service creates a Sntp server that offers time from a relayed server or the local hardware
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            //  Create the Sntp server
            SntpService SntpServer = new SntpService();

            // Enable Sntp server to use local device for it's time reference.
            //SntpServer.UseLocalTimeSource = true;

            // enable Sntp server to relay requests to another Sntp server.
            SntpServer.UseLocalTimeSource = false;
            SntpServer.PrimaryServer = "0.pool.ntp.org";

            // Sets interface ip address
            SntpServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Starts Sntp service
            SntpServer.Start();
        }
    }
}
 ```