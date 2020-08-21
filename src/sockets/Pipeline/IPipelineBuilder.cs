namespace Bytewizer.TinyCLR.Sockets
{
    public interface IPipelineBuilder
    {
        PipelineBuilder Register(FilterDelegate filter);
        PipelineBuilder Register(IPipelineFilter filter);
    }
}