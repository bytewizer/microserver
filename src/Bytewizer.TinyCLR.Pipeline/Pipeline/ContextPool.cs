using System;
using System.Collections;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Context pool for reusing context objects.
    /// </summary>
    public sealed class ContextPool
    {
        private readonly ArrayList _used;
        private readonly ArrayList _available;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextPool"/> class.
        /// </summary>
        public ContextPool()
        {
            _available = new ArrayList();
            _used = new ArrayList();
        }

        /// <summary>
        /// Gets a <see cref="IContext"/> object from pool.
        /// </summary>
        public IContext GetContext(Type context)
        {
            //Debug.WriteLine($"Used Count:{_used.Count}");
            //Debug.WriteLine($"Available Count:{_available.Count}");

            lock (_available)
            {
                if (_available.Count > 0)
                {
                    IContext ctx = _available[0] as IContext;
                    _available.RemoveAt(0);
                    _used.Add(ctx);
                    return ctx;
                }
                else
                {
                    IContext ctx = Activator.CreateInstance(context) as IContext;
                    _used.Add(ctx);
                    return ctx;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="IContext"/> object back to the pool and clears channel.
        /// </summary>
        public void Release(IContext context)
        {
            // Close connection and clears channel.
            context.Clear();

            lock (_available)
            {
                _used.Remove(context);
                _available.Add(context);
            }
        }
    }
}
