using System;

using Bytewizer.TinyCLR.DependencyInjection;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Hosting
{
    internal sealed class WebHostBuilder : IWebHostBuilder
    {
        public WebHostBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IServer Build()
        {
            throw new NotImplementedException();
        }
    }
}
