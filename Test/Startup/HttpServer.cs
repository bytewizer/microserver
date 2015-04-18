using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Http;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Modules;

namespace HttpServer
{
    public class Program
    {       
        // The disk file service allows you to serve and browse files from flash/disk storage
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            // Create Http server pipeline  
            ModuleManager ModuleManager = new ModuleManager();

            // Create file/disk service for storage
            DiskFileService fileService = new DiskFileService(@"/", @"\WINFS\");

            // Add the file module to pipeline and enable the file listing feature
            ModuleManager.Add(new FileModule(fileService) { AllowListing = true });

            // Add the error module as the last module to pipeline
            ModuleManager.Add(new ErrorModule());

            //  Create the http server
            HttpService HttpServer = new HttpService(ModuleManager);

            // Sets interface ip address
            HttpServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Starts Http service
            HttpServer.Start();
        }
    }
}
