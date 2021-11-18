using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Pipeline.Builder;
using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.Playground.Hosting
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpServer(options => 
            {
                options.Listen(8080);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", context =>
                {
                    string response = "<doctype !html><html><head><title>Hello, world!" +
                        "</title><meta http-equiv='refresh' content='5'></head><body>" +
                        "<h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                    context.Response.Write(response);
                });
            });
        }
    }
}
