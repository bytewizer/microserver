using System.Collections;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly ArrayList _components = new ArrayList();

        private ApplicationBuilder()
        {
        }

        public IApplicationBuilder Use(RequestDelegate middleware)
        {
            _components.Add(middleware);
            return this;
        }

        public IApplicationBuilder New()
        {
            return new ApplicationBuilder();
        }

        public RequestDelegate Build()
        {
            RequestDelegate app = context =>
            {

            };

            foreach (RequestDelegate component in _components) // Reverse
            {
                app = null; //component(app);
            }

            return app;
        }
    }
}
