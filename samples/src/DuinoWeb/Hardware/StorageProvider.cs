using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Storage;

namespace Bytewizer.TinyCLR
{
    public static class StorageProvider
    {
        private static readonly object _lock = new();

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

                Controller = StorageController.FromName(SC20260.StorageController.SdCard);
                Drive = FileSystem.Mount(Controller.Hdc);
            }

            _initialized = true;
        }
    }
}