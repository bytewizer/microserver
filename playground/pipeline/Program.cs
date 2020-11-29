using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.Playground.Pipeline
{
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

                Debug.WriteLine("Module A: Code executed before 'next'");
                next(context);
                Debug.WriteLine("Module A: Code executed after 'next'");
                Debug.WriteLine(ctx.Message);
            }
        }

        public class Module2 : PipelineFilter
        {
            protected override void Invoke(IContext context, RequestDelegate next)
            {
                Debug.WriteLine("Module B: Code executed before 'next'");

                // if you do not include the 'next' delegate in the module. Execution will turn around in
                // the pipeline skipping down stream modules.  
                Random rnd = new Random();
                if (rnd.Next(5) == 0) // random true/false
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

        public class Module3 : PipelineFilter
        {
            protected override void Invoke(IContext context, RequestDelegate next)
            {
                Debug.WriteLine("Module C: Code executed before 'next'");
                next(context); // this is optional and skipped in the last module of the pipeline
                Debug.WriteLine("Module C: Code executed after 'next'");
            }
        }
    }
}