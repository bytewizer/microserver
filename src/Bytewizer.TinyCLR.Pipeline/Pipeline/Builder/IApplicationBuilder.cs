using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Pipeline.Builder
{
    /// <summary>
    /// An interface for <see cref="ApplicationBuilder"/>.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        Hashtable Properties { get; }

        /// Gets the value of a property from the <see cref="ApplicationBuilder.Properties"/> collection
        /// using the provided value as the key.
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>
        bool TryGetProperty(string key, out object value);

        /// <summary>
        /// Gets the value of a property from the <see cref="ApplicationBuilder.Properties"/> collection
        /// using the provided value as the key.
        /// </summary>
        /// <param name="key">The key value to get the property.</param>
        object GetProperty(string key);

        /// Sets the value of an property in the <see cref="ApplicationBuilder.Properties"/> collection using
        /// the provided value as the key.
        /// <param name="key">The key value to set the property.</param>
        /// <param name="value">The value of the property.</param>
        void SetProperty(string key, object value);

        /// <summary>
        /// Register a <see cref="IMiddleware"/> to the pipeline. Middleware are executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="IMiddleware"/> to include in the application pipeline.</param>
        IApplicationBuilder Use(IMiddleware middleware);

        /// <summary>
        /// Register a <see cref="IMiddleware"/> to the pipeline. Middleware are executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="InlineMiddleware"/> delegate to include in the application pipeline.</param>
        IApplicationBuilder Use(InlineMiddlewareDelegate middleware);
        
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the application's service container.
        /// </summary>
        IServiceProvider ApplicationServices { get; set; }

        /// <summary>
        /// Builds the delegate used by this application to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        IApplication Build();
    }
}