//using System;
//using System.Collections;
//using System.Text;
//using System.Threading;

//namespace Bytewizer.TinyCLR.WebServer
//{
//    class Program
//    {
//        static void Main()
//        {
//            //Networking.SetupEthernet();

//            CreateHostBuilder().Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder() =>
//       Host.CreateDefaultBuilder()
//           .ConfigureWebHostDefaults(webBuilder =>
//           {
//               webBuilder.UseHttpServer(options =>
//               {
//                   options.Listen(IPAddress.Any, 80, listener =>
//                   {
//                       listener.MinThreads = 3;
//                   });
//                   options.Register(new HttpSessionMiddleware());
//                   options.Register(new DeveloperExceptionPageMiddleware());
//                   options.Register(new StaticFileMiddleware());
//                   options.Register(new HttpResponse());
//               });
//               webBuilder.UseStartup(typeof(Startup));
//           });

//        public class Startup
//        {
//            public Startup() { }

//            public void Configure(IApplicationBuilder app)
//            {
//                var sd = StorageController.FromName(SC20100.StorageController.SdCard);
//                var drive = FileSystem.Mount(sd.Hdc);

//                app.UseStaticFiles(new StaticFileOptions()
//                {
//                    DriveProvider = drive,
//                });
//            }
//        }
//    }
//}
