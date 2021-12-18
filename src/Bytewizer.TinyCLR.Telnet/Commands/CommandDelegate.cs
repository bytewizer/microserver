namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// A function that can process a telnet command.
    /// </summary>
    /// <param name="context">The context for the request.</param>
    public delegate void CommandDelegate(TelnetContext context);
}