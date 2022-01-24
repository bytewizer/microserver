namespace Bytewizer.TinyCLR.Hosting.Internal
{
    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(HostBuilderContext hostContext);
    }
}