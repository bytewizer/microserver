namespace Bytewizer.Extensions.Hosting.Internal
{
    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(HostBuilderContext hostContext);
    }
}