
namespace Bytewizer.TinyCLR.Logging.Debug
{
    /// <summary>
    /// The provider for the <see cref="DebugLogger"/>.
    /// </summary>
    public class DebugLoggerProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new DebugLogger(name);
        }

        public void Dispose()
        {
        }
    }
}
