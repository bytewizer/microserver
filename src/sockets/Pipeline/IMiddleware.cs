namespace Bytewizer.Sockets
{
    public interface IMiddleware
    {
        void Register(IMiddleware filter);
        void Execute(Context context);
    }
}
