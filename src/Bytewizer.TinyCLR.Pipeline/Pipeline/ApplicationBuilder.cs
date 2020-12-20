using System;

namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Represents an implementation of the <see cref="ApplicationBuilder"/> for creating application request pipelines.
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        private MiddlewareDelegate[] _components = new MiddlewareDelegate[0];

        /// <inheritdoc/>
        public IApplicationBuilder Register(InvokeMiddlewareDelegate middleware)
        {
            Register(new InvokeMiddleware(middleware));
            return this;
        }

        /// <inheritdoc/>
        public IApplicationBuilder Register(IMiddleware middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            _components = _components.Append(() => middleware);
            return this;
        }

        /// <summary>
        /// Builds an application request pipeline from registered middleware. 
        /// </summary>
        public IApplication Build()
        {
            if (_components.Length < 1)
            {
                throw new InvalidOperationException("Register one or more middleware objects.");
            }

            var root = _components[0].Invoke();

            for (int i = 1; i < _components.Length; i++)
            {
                root.UseMiddleware(_components[i].Invoke());
            }

            return root;
        }
    }
}