using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Storage;
using GHIElectronics.TinyCLR.Native;

namespace Bytewizer.Playground
{
    public static class StorageProvider
    {
        private static readonly object _lock = new object();

        private static bool _initialized;

        public static StorageController Controller { get; private set; }

        public static IDriveProvider Drive { get; private set; }

        public static void Initialize()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                if (DeviceInformation.DeviceName == "SC20100")
                {
                    Controller = StorageController.FromName(SC20100.StorageController.SdCard);
                }
                else
                { 
                    Controller = StorageController.FromName(SC20260.StorageController.SdCard);
                }

                Drive = FileSystem.Mount(Controller.Hdc);
            }

            _initialized = true;
        }
    }
}