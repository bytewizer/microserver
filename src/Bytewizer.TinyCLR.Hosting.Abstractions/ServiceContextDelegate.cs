using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.TinyCLR.Hosting
{
    public delegate void ServiceContextDelegate(HostBuilderContext context, IServiceCollection serviceCollection);
}
