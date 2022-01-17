using Bytewizer.TinyCLR.Features;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Terminal
{
    public interface ITerminalContext : IContext
    {
        bool Active { get; set; }
        IFeatureCollection Features { get; }
        byte[] OptionCommands { get; set; }
        TerminalOptions Options { get; }
        TerminalRequest Request { get; }
        TerminalResponse Response { get; }
    }
}