namespace Bytewizer.Extensions.Console.Internal
{
    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(ApplicationBuilderContext hostContext, object containerBuilder);
    }
}