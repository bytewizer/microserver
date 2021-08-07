using System;
using System.Reflection;

using GHIElectronics.TinyCLR.Devices.SecureStorage;

namespace Bytewizer.Playground
{   
    public class SettingsProvider
    {
        private static bool _initialized;
        private static readonly object _lock = new object();

        public static SecureStorageController Controller { get; private set; }
        public static FlashObject Flash { get; private set; }

        public static void Initialize()
        {
            if (_initialized)
                return;

            Controller = new SecureStorageController(SecureStorage.Configuration);

            Flash = ReadFlash();

            _initialized = true;
        }

        public static bool IsErased()
        {
            var isBlank = true;

            for (uint block = 0; block < Controller.TotalSize / Controller.BlockSize; block++)
            {
                if (!Controller.IsBlank(block)) isBlank = false;
            }

            return isBlank;
        }

        public static void Write(FlashObject settings)
        {
            if (_initialized == false)
            {
                Initialize();
            }
            
            lock (_lock)
            {
                var buffer = new byte[1 * 1024];
                byte[] flashBuffer = Reflection.Serialize(settings, typeof(FlashObject));
             
                Array.Copy(BitConverter.GetBytes(flashBuffer.Length), buffer, 4);
                Array.Copy(flashBuffer, 0, buffer, 4, flashBuffer.Length);

                if (IsErased() == false)
                {
                    Controller.Erase();
                }

                var dataBlock = new byte[Controller.BlockSize];

                for (uint block = 0; block < buffer.Length / Controller.BlockSize; block++)
                {
                    Array.Copy(buffer, (int)(block * Controller.BlockSize), dataBlock, 0, (int)(Controller.BlockSize));
                    Controller.Write(block, dataBlock);
                }
            }
        }

        private static FlashObject ReadFlash()
        {
            lock (_lock)
            {
                if (IsErased() == false)
                {
                    var buffer = new byte[1 * 1024];
                    var dataBlock = new byte[Controller.BlockSize];

                    for (uint block = 0; block < buffer.Length / Controller.BlockSize; block++)
                    {
                        Controller.Read(block, dataBlock);
                        Array.Copy(dataBlock, 0, buffer, (int)(block * Controller.BlockSize), dataBlock.Length);
                    }
                    
                    var length = BitConverter.ToInt16(buffer, 0);
                    
                    byte[] flashBuffer = new byte[length];
                    Array.Copy(buffer, 4, flashBuffer, 0, length);

                    return (FlashObject)Reflection.Deserialize(flashBuffer, typeof(FlashObject));
                }
                else
                {
                    Write(new FlashObject());
                    return ReadFlash();
                }
            }
        }
    }
}