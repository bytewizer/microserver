namespace Bytewizer.Sockets
{
    public delegate IMiddleware FilterDelegate();
    public delegate void RequestDelegate(Context context);
    public delegate void ServerOptionsDelegate(ServerOptions configure);
    public delegate void SocketListenerOptionsDelegate(SocketListenerOptions configure);
}