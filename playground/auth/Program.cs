using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Authenticator;

namespace Bytewizer.Playground.Authentication
{
    class Program
    {
        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            var accountService = new DefaultAccountService();
            accountService.Register("admin", "password");

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication(new AuthenticationOptions
                    {
                        AuthenticationProvider = new DigestAuthenticationProvider(),
                        AccountService = accountService
                    });
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            var username = context.GetCurrentUser().Username;
                            string response = "<doctype !html><html><head><meta http-equiv='refresh' content='1'><title>Hello, world!</title>" +
                                              "<style>body { background-color: #43bc69 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                              "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1><h1>" + username + "</h1></body></html>";

                            context.Response.Write(response);
                        });
                    });
                });
            });
            server.Start();
        }
    }
}