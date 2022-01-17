namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents an options method to configure <see cref="ConsoleServerOptions"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="ConsoleServerOptions"/> configuration specific features.</param>
    public delegate void ServerOptionsDelegate(ConsoleServerOptions configure);
}