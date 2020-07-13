namespace Bytewizer.Sockets
{
    public interface IPipelineBuilder
    {
        PipelineBuilder Register(FilterDelegate filter);
        PipelineBuilder Register(IMiddleware filter);
    }
}