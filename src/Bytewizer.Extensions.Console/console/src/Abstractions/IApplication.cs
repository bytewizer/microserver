using System.Threading;
using System.Threading.Tasks;

namespace Bytewizer.Extensions.Console
{
    public interface IApplication
    {
        Task MainAsync(string[] args, CancellationToken cancellationToken);
    }
}
