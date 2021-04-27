# Pipeline

Pipleline is a generic middleware pipeline built for TinyCLR OS.

## Simple Pipeline Example

```CSharp
class Program
{
    static void Main()
    {
        var ctx = new Context() { Message = "Context: Finished" };

        IApplicationBuilder builder = new ApplicationBuilder();
        builder.Use(new Middleware1());
        builder.Use(new Middleware2());
        builder.Use(new Middleware3());

        // Properties use to set values used by other middleware
        builder.SetProperty("key", "Property Value");  
        Debug.WriteLine((string)builder.GetProperty("key"));

        IApplication app = builder.Build();
        app.Use((context, next) =>
        {
            Debug.WriteLine("Inline: Code executed before 'next'");
            next(context);
            Debug.WriteLine("Inline: Code executed after 'next'");
        });
        app.Invoke(ctx);
    }

    public class Context : IContext
    {
        public string Message { get; set; }
    }

    public class Middleware1 : Middleware
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            var ctx = context as Context;

            Debug.WriteLine("Middleware 1: Code executed before 'next'");
            next(context);
            Debug.WriteLine("Middleware 1: Code executed after 'next'");
            Debug.WriteLine(ctx.Message);
        }
    }

    public class Middleware2 : Middleware
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            Debug.WriteLine("Middleware 2: Code executed before 'next'");

            // if you do not include the 'next' delegate in the module. Execution will turn around in
            // the pipeline skipping down stream modules.  
            Random rnd = new Random();
            if (rnd.Next(5) == 0) // random true/false
            {
                next(context);
            }
            else
            {
                Debug.WriteLine("Skipping Middleware 2 in pipeline and turning back");
            }

            Debug.WriteLine("Middleware 2: Code executed after 'next'");
        }
    }

    public class Middleware3 : Middleware
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            Debug.WriteLine("Middleware 3: Code executed before 'next'");
            next(context); // this is optional and skipped in the last module of the pipeline
            Debug.WriteLine("Middleware 3: Code executed after 'next'");
        }
    }
}
```

Output: If True

```console
Middleware 1: Code executed before 'next'
Middleware 2: Code executed before 'next'
Middleware 3: Code executed before 'next'
Inline: Code executed before 'next'
Inline: Code executed after 'next'
Middleware 3: Code executed after 'next'
Middleware 2: Code executed after 'next'
Middleware 1: Code executed after 'next'
Context: Finished
```

Output: If False

```console
Middleware 1: Code executed before 'next'
Middleware 2: Code executed before 'next'
Skipping Middleware 2 in pipeline and turning back
Middleware 2: Code executed after 'next'
Middleware 1: Code executed after 'next'
Context: Finished
```

## TinyCLR Packages
```bash
PM> Install-Package Bytewizer.TinyCLR.Pipeline
```