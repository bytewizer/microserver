using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Pipeline.Builder
{
    /// <summary>
    /// Represents an implementation of the <see cref="ApplicationBuilder"/> for creating application request pipelines.
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        private Hashtable _properties;

        private MiddlewareDelegate[] _components = new MiddlewareDelegate[0];

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public Hashtable Properties
        {
            get
            {
                if (_properties == null)
                {
                    return new Hashtable();
                }

                return _properties;
            }
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ApplicationBuilder" /> class.
        /// </summary>
        public ApplicationBuilder()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ApplicationBuilder" /> class.
        /// </summary>
        /// <param name="serviceProvider">A service object of type.</param>
        public ApplicationBuilder(IServiceProvider serviceProvider)
        {
            ApplicationServices = serviceProvider;
        }

        /// <inheritdoc/>
        public IServiceProvider ApplicationServices { get; set; }

        /// <inheritdoc/>
        public bool TryGetProperty(string key, out object value)
        {
            value = default;
            
            if (_properties == null)
            {
                return false;
            }

            if (_properties.Contains(key))
            {
                value = _properties[key];
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public object GetProperty(string key)
        {
            if (_properties == null)
            {
                return default;
            }

            return _properties[key];
        }

        /// <inheritdoc/>
        public void SetProperty(string key, object value)
        {
            if (_properties == null)
            {
                _properties = new Hashtable();
            }

            _properties[key] = value;
        }

        /// <inheritdoc/>
        public IApplicationBuilder Use(InlineMiddlewareDelegate middleware)
        {
            Use(new InlineMiddleware(middleware));
            return this;
        }

        /// <inheritdoc/>
        public IApplicationBuilder Use(IMiddleware middleware)
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
                root.Use(_components[i].Invoke());
            }

            return root;
        }
    }
}