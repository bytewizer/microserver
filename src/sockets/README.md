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
            options.Register(new SimpleResponse());
        });
        server.Start();
    }
}

public class SimpleResponse : Middleware
{
    protected override void Execute(ServerContext context, RequestDelegate next)
    {       
        try
        {
            if (context.InputStream == null)
                return;
            
            var reader = new StreamReader(context.InputStream);

            // read the context input stream (required or browser will stall the request)
            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                Debug.WriteLine(line);
            }

            string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" +
                    "<html><head><title></title></head><body>" + DateTime.Now + "</body></html>\r\n");

            // send the response to browser
            context.Send(response);           
        }
        catch (Exception ex)
        {
            // try to manage all unhandled exceptions in the pipeline
            Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
        }
        finally
        {
            // close the connection once all data is sent (only after the last send)
            context.Close();
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

## Pipeline Logic

```CSharp
class Program
{
    static void Main()
    {
        var server = new SocketServer(options =>
        {
            options.Listen(IPAddress.Any, 80);
            options.Register(new AModule());
            options.Register(new BModule());
            options.Register(new CModule());
        });
        server.Start();
    }
}

public class AModule : Middleware
{
    protected override void Execute(ServerContext context, RequestDelegate next)
    {
        Debug.WriteLine("Module A: Code executed before 'next'");
        next(context);
        Debug.WriteLine("Module A: Code executed after 'next'");
    }
}

public class BModule : Middleware
{
    protected override void Execute(ServerContext context, RequestDelegate next)
    {
        Debug.WriteLine("Module B: Code executed before 'next'");

        // if you do not include the 'next' delegate in the module. Execution will turn around in
        // the pipeline skipping down stream modules.  
        Random rnd = new Random();
        if (rnd.Next(5) == true) // random true/false
        {
            next(context);
        }
        else
        {
            Debug.WriteLine("Skipping module C in pipeline and turning back");
        }

        Debug.WriteLine("Module B: Code executed after 'next'");
    }
}

public class CModule : Middleware
{
    protected override void Execute(ServerContext context, RequestDelegate next)
    {
        Debug.WriteLine("Module C: Code executed before 'next'");
        next(context); // this is optional and skipped in the last module of the pipeline
        Debug.WriteLine("Module C: Code executed after 'next'");
    }
}
```

Output: If True

```console
Module A: Code executed before 'next'
Module B: Code executed before 'next'
Module C: Code executed before 'next'
Module C: Code executed after 'next'
Module B: Code executed after 'next'
Module A: Code executed after 'next'
```

Output: If False

```console
Module A: Code executed before 'next'
Module B: Code executed before 'next'
Module B: Skipping module C in pipeline and turning back
Module B: Code executed after 'next'
Module A: Code executed after 'next'
```

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.
