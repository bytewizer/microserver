namespace Bytewizer.TinyCLR.Sockets
{
    public interface IPipelineFilter
    {
        void Register(IPipelineFilter filter);
        void Invoke(IContext context);
    }
}
