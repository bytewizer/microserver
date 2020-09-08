using Bytewizer.TinyCLR;
using Bytewizer.TinyCLR.Http;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Devices.Storage;

namespace Bytewizer.Toggler
{
    class Program
    {
        static void Main()
        {
            Networking.SetupEthernet();

            var sd = StorageController.FromName(SC20100.StorageController.SdCard);
            FileSystem.Mount(sd.Hdc);

            var server = new HttpServer(options =>
            {
                options.UseMiddleware(new HttpSessionMiddleware());
                options.UseDeveloperExceptionPage();
                options.UseStaticFiles();
                options.UseMvc();
            });
            server.Start();
        }
    }
}
