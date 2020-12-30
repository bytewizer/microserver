using System;
using System.Collections;

using Bytewizer.TinyCLR.Http.Routing;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    internal class DefaultEndpointRouteBuilder : IEndpointRouteBuilder
    {
        public DefaultEndpointRouteBuilder(IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            ApplicationBuilder = applicationBuilder;
            DataSources = new Hashtable();
        }

        public IApplicationBuilder ApplicationBuilder { get; }
        
        public Hashtable DataSources { get; }
    }
}