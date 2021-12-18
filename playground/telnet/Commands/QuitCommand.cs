using Bytewizer.TinyCLR.Telnet;

namespace Bytewizer.Playground.Telnet.Commands
{
    public class QuitCommand : Command
    {
        public IActionResult Default()
        {
            CommandContext.TelnetContext.Active = false;
            
            return new EmptyResult();
        }
    }
}