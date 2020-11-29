using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bytewizer.Extensions.Console
{
    /// <summary>
    /// A program abstraction.
    /// </summary>
    public interface IConsoleApplication : IDisposable
    {
        /// <summary>
        /// The programs configured services.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Run the application.
        /// </summary>
        /// <param name="cancellationToken">Used to abort program start.</param>
        /// <returns></returns>
        Task RunAsync(string[] args = null, CancellationToken cancellationToken = default);
    }
}
