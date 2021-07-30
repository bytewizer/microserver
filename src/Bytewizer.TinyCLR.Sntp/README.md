# SNTP

Privides a simple NTP (SNTP) server and client to provide accurate network time built for TinyCLR OS.

## Simple SNTP Server Example

```CSharp
static void Main()
{
    // Initialize networking before starting service

    var server = new SntpServer()
    server.Start();
}
```

## Simple SNTP Host Only 

```CSharp
static void Main()
{
    // Synchronizes device with remote time but do not allow clients to connect 
    
    var server = new SntpServer(options =>
    {
        // Set server to secondary status pulling time from an upstream server
        options.Server = "time.google.com";
            
        // Disables udp listener on port 123 preventing clients from connecting 
        options.Listening = false;

    });

    server.Start();
}
```

## Simple SNTP Client 

```CSharp
static void Main()
{
    // Synchronizes device with remote time server
    
    var sntpClient = new SntpClient("time.google.com", 123);
    var accurateTime = DateTime.UtcNow + sntpClient.GetCorrectionOffset();
}
```

## Simple SNTP Server with External Time Source Support

```CSharp
static void Main()
{    
    // local time source used to update clock
    var timeSource = DateTime.UtcNow;

    var server = new SntpServer(options =>
    {
        // Set server to respond a primary time stratum 
        options.Stratum = Stratum.Primary;

        // Set server to respond with GPS as reference id
        options.ReferenceId = ReferenceId.GPS;
        
        // Set an external time source allowing updates
        options.TimeSource = timesource;

    });

    server.Start();

    while(true) // Fake time source
    {
        //  This could be an external time source like a GPS
        timesource = DateTime.UtcNow;
        Thread.Sleep(10000);
    }
}
```

## Simple SNTP Server with Real Time Clock Support

```CSharp
static void Main()
{
    // Specifies the real time clock 
    var controller = RtcController.GetDefault();
    controller.SetChargeMode(BatteryChargeMode.Fast);

    if (controller.IsValid)
    {
        SystemTime.SetTime(controller.Now);
    }

    var server = new SntpServer(options =>
    {
        // Set server to secondary status pulling time from an upstream server
        options.Server = "time.google.com";

        // Set realtime clock provider to get timestamp data from
        options.RealtimeClock = controller;
    });

    server.Start();
}
```
## TinyCLR Packages
```bash
PM> Install-Package Bytewizer.TinyCLR.Sntp
```

## RFC - Related Request for Comments 
- [RFC 4330 - Simple Network Time Protocol (SNTP)](https://tools.ietf.org/html/rfc4330)