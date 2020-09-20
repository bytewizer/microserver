using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Tests.Sockets
{
    public class AModule : PipelineFilter
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            Debug.WriteLine("Module A: Code executed before 'next'");
            next(context);
            Debug.WriteLine("Module A: Code executed after 'next'");
        }
    }

    public class BModule : PipelineFilter
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

    public class CModule : PipelineFilter
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            Debug.WriteLine("Module C: Code executed before 'next'");
            next(context); // this is optional and skipped in the last module of the pipeline
            Debug.WriteLine("Module C: Code executed after 'next'");
        }
    }
}