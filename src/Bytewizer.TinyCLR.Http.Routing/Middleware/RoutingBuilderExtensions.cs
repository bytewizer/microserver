using System;
using System.Collections;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http.Routing
{
    public static class EndpointRoutingExtensions
    {

        public static void UseRouting(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(new EndpointRoutingMiddleware());
        }

        public static void UseEndpoints(this ServerOptions app, EndpointRouteAction configure)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            //var endpointRouteBuilder = new EndpointRouteBuilder();
            //configure(endpointRouteBuilder);

            //var routes = (ArrayList)endpointRouteBuilder.DataSources;

            app.UseMiddleware(new EndpointMiddleware());
        }
    }
}
