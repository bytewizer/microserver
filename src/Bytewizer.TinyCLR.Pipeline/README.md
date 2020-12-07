# Pipeline

Pipleline is a generic middleware pipeline built for TinyCLR OS.

## Simple Pipeline Example

```CSharp
class Program
{       
    static void Main()
    {
        var builder = new PipelineBuilder();
        builder.Register(new Module1());
        builder.Register(new Module2());
        builder.Register(new Module3());
        
        var pipeline = builder.Build();
        var ctx = new Context() { Message = "Context: Finished" };
        
        pipeline.Invoke(ctx);
    }

    public class Context : IContext
    {
        public string Message { get; set; }
    }

    public class Module1 : PipelineFilter
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            var ctx = context as Context;

            Debug.WriteLine("Module 1: Code executed before 'next'");
            next(context);
            Debug.WriteLine("Module 1: Code executed after 'next'");
            Debug.WriteLine(ctx.Message);
        }
    }

    public class Module2 : PipelineFilter
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            Debug.WriteLine("Module 2: Code executed before 'next'");

            // if you do not include the 'next' delegate in the module. Execution will turn around in
            // the pipeline skipping down stream modules.  
            Random rnd = new Random();
            if (rnd.Next(5) == 0) // random true/false
            {
                next(context);
            }
            else
            {
                Debug.WriteLine("Skipping module 2 in pipeline and turning back");
            }

            Debug.WriteLine("Module 2: Code executed after 'next'");
        }
    }

    public class Module3 : PipelineFilter
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            Debug.WriteLine("Module 3: Code executed before 'next'");
            next(context); // this is optional and skipped in the last module of the pipeline
            Debug.WriteLine("Module 3: Code executed after 'next'");
        }
    }
}
```

Output: If True

```console
Module 1: Code executed before 'next'
Module 2: Code executed before 'next'
Module 3: Code executed before 'next'
Module 3: Code executed after 'next'
Module 2: Code executed after 'next'
Module 1: Code executed after 'next'
```

Output: If False

```console
Module 1: Code executed before 'next'
Module 2: Code executed before 'next'
Skipping module 3 in pipeline and turning back
Module 2: Code executed after 'next'
Module 1: Code executed after 'next'
```
