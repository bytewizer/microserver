namespace Bytewizer.TinyCLR.Sockets
{
    public class Context : IContext
    {
        public SocketSession Session { get; set; } = new SocketSession();
    }
}