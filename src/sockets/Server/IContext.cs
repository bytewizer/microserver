namespace Bytewizer.TinyCLR.Sockets
{
    public interface IContext 
    {
        SocketSession Session { get; set; }
    }
}