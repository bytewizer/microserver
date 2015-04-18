using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Dns;

namespace DnsServer
{
    public class Program
    {
        // This service creates a Dns server
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            // Create the Dns server
            DnsService DnsServer = new DnsService();

            // Set the device server name and Dns suffix offered to client 
            DnsServer.ServerName = "example";
            DnsServer.DnsSuffix = "iot.local";

            // Sets interface ip address
            DnsServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Add a resource record to the zone table 
            Answer record = new Answer();
            record.Domain = string.Concat(DnsServer.ServerName, ".", DnsServer.DnsSuffix);
            record.Class = RecordClass.IN;
            record.Type = RecordType.A;
            record.Ttl = 60;
            record.Record = new ARecord(DnsServer.InterfaceAddress.ToString());
            DnsServer.ZoneFile.Add(record);

            // Enable Dns server to relay requests that can not be looked up locally to another Dns server.
            DnsServer.PrimaryServer = "8.8.8.8";
            DnsServer.IsProxy = true;

            // Starts Dns service
            DnsServer.Start();
        }
    }
}
