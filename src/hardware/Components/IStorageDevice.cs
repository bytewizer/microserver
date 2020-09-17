namespace Bytewizer.TinyCLR.Hardware.Components
{
    public interface IStorageDevice
    {
        void Mount();
        void Unmount();
        void Dispose();
    }
}