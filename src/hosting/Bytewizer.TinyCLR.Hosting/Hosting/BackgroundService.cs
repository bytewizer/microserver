using System;
using System.Threading;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Base class for implementing a long running <see cref="IHostedService"/>.
    /// </summary>
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Thread _executeThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundService"/> class.
        /// </summary>
        protected BackgroundService()
        {
            ServiceName = GetType().Name;
            IsCancellationRequested = false;
        }

        /// <summary>
        /// Gets whether cancellation has been requested for this service.
        /// </summary>
        protected bool IsCancellationRequested { get; private set; }

        /// <summary>
        /// The service name to associate with the service.
        /// </summary>
        /// <remarks>
        /// If not explicitly set, will default to the ServiceId property value.
        /// </remarks>
        public string ServiceName { get; private set; }

        /// <summary>
        /// Gets the <see cref="Thread"/> that executes the background operation.
        /// </summary>
        /// <remarks>
        /// Will return <see langword="null"/> if the background operation hasn't started.
        /// </remarks>
        public virtual Thread ExecuteThread() => _executeThread;

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <returns>A <see cref="Thread"/> that represents the long running operations.</returns>
        protected abstract void ExecuteAsync();

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        public virtual Thread Start()
        {
            IsCancellationRequested = false;

            try
            {
                // Store the thread we're executing
                _executeThread = new Thread(() =>
            {
                try
                {
                    ExecuteAsync();
                }
                catch 
                {
                    throw;
                }
            });
                _executeThread.Start();
            }
            catch
            {
                throw;
            }

            return _executeThread;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        public virtual void Stop(int timeout = 10000)
        {
            if (_executeThread == null)
            {
                return;
            }

            // Request graceful termination.
            IsCancellationRequested = true;

            if (_executeThread != null)
            {
                // Wait for thread to exit
                //_executeThread.Abort();
                _executeThread.Join(timeout);
                _executeThread = null;
            }
        }

        public virtual void Dispose()
        {
        }
    }
}
