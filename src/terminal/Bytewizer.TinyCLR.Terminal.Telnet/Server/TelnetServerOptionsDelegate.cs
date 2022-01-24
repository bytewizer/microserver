namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents an options method to configure <see cref="TelnetServerOptions"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="TelnetServerOptions"/> configuration specific features.</param>
    public delegate void ServerOptionsDelegate(TelnetServerOptions configure);
}