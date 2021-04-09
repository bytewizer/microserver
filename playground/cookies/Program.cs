using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Cookies
{
    class Program
    {
        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseCookies();
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            var cookies = context.GetCookies();                 
                            cookies.TryGetValue("sid", out string id);
                            
                            var responseCookies = context.GetResponseCookies();
                            responseCookies.Append("sux", "45a4ffgra8");

                            responseCookies.Append("sid", "38afes7a9", 86400,
                                "/", "192.168.1.145", false, false);

                            // Remove/Expire a browser cookie 
                            //responseCookies.Append("sux", "45a4ffgra8", 0, // set max age = 0
                            //   "/", "192.168.1.145", false, false);

                            context.Response.Write($"Session Id: {id}");
                        });
                    });
                });
            });
            server.Start();
        }
    }
}