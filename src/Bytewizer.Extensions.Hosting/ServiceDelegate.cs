using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.TinyCLR.Hosting
{
    public delegate void ServiceDelegate(HostBuilderContext context, IServiceCollection serviceCollection);
}
