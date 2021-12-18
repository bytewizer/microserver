using Bytewizer.TinyCLR.Telnet;

namespace Bytewizer.Playground.Telnet.Commands
{
    public class ShowCommand : Command
    {
        public ShowCommand()
        {
            Description = "This is a description of the command used with help";
        }

        public IActionResult Version()
        {
            return Response("1.2.1");
        }

        public IActionResult Info()
        {
            return Response("This is info");
        }

        public IActionResult Help()
        {
            if(CommandContext.Arguments.TryGetValue("switch", out string value))
            {
                return Response($"command line included the 'switch' with {value}.");
            }
            
            return Response("This with out a switch");
        }
    }
}