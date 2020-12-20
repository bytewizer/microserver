using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Routing
{
    public class EndpointRouteBuilder : IEndpointRouteBuilder
    {
        public ICollection DataSources { get; } = new ArrayList();

        public IServiceProvider ServiceProvider => throw new NotImplementedException();
    }
}
