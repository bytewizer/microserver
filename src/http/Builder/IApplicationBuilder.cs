using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Defines a class that provides the mechanisms to configure an application's request pipeline.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Adds a middleware delegate to the application's request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware delegate.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        IApplicationBuilder Use(RequestDelegate middleware);

        /// <summary>
        /// Creates a new <see cref="IApplicationBuilder"/> that shares the <see cref="Properties"/> of this
        /// <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IApplicationBuilder"/>.</returns>
        IApplicationBuilder New();

        /// <summary>
        /// Builds the delegate used by this application to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        RequestDelegate Build();
    }
}
