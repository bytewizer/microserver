# SocketServer

SocketServer is a modular socket server built for TinyCLR OS.

## Simple Http Response Example

```CSharp
class Program
{
    static void Main(string[] args)
    {
        var server = new SocketServer(options =>
        {
            options.Register(new HttpResponse());
        });
        server.Start();
    }
}

public class HttpResponse : PipelineFilter
{
    protected override void Invoke(IContext context, RequestDelegate next)
    {
        try
        {
            if (context.Channel.InputStream == null)
                return;
        
            var reader = new StreamReader(context.Channel.InputStream);

            // read the context input stream (required or browser will stall the request)
            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                Debug.WriteLine(line);
            }

            string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" +
                                "<doctype !html><html><head><meta http-equiv='refresh' content='5'><title>Hello, world!</title>" +
                                "<style>body { background-color: #111 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

            // send the response to browser
            context.Channel.Write(response);           
        }
        catch (Exception ex)
        {
            // try to manage all unhandled exceptions in the pipeline
            Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
        }
        finally
        {
            // close the connection once all data is sent (only after the last send)
            context.Channel.Clear();
        }
    }
}
```

## Simple Https Response Example

### Installing self-signed certificate on Windows

Install the required browsers certs from the "certificate" directory by clicking on the following files:

* bytewizer.local.cer - Install domain certificate to the current user store.
* bytewizer.local.pfx - Install CA certificate to the Trusted Root Certification Authorities (required elevated privileges) with password of "bytewizer.local".

Update your DNS server to include the fully qualified domain name of the device or add the following line to your local "host" file in C:\Windows\System32\drivers\etc\ using the ip address of your TinyCRL OS device.

```console
X.X.X.X device.bytewizer.local  
```

```CSharp
static void Main()
{
    var X509cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DeviceCert))
    {
        PrivateKey = Resources.GetBytes(Resources.BinaryResources.DeviceKey)
    };

    var server = new SocketServer(options =>
    {
        options.Listen(IPAddress.Any, 443, listener =>
        {
            listener.UseHttps(X509cert);
        });
        options.Register(new SimpleResponse());
    });

    server.Start();
}
```
