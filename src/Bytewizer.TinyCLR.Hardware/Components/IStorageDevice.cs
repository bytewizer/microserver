using System;

namespace Bytewizer.TinyCLR.Hardware.Components
{
    public interface IStorageDevice : IDisposable
    {
        void Mount();
        void Unmount();
    }
}