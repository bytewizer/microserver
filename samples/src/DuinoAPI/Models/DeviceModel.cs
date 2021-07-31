using GHIElectronics.TinyCLR.Native;

namespace Bytewizer.TinyCLR.DuinoAPI
{
    public class DeviceModel
    {
        public DeviceModel()
        {
            UniqueId = DeviceInformation.GetUniqueId();
            ManufacturerName = DeviceInformation.ManufacturerName;
            DeviceName = DeviceInformation.DeviceName;
            
            var major = (ushort)((DeviceInformation.Version >> 48) & 0xFFFF);
            var minor = (ushort)((DeviceInformation.Version >> 32) & 0xFFFF);
            var build = (ushort)((DeviceInformation.Version >> 16) & 0xFFFF);
            var revision = (ushort)((DeviceInformation.Version >> 0) & 0xFFFF);
            Version = $"{major}.{minor}.{build}.{revision}";
        }

        public byte[] UniqueId { get; private set; }
        public string ManufacturerName { get; private set; }
        public string DeviceName { get; private set; }
        public string Version { get; private set; }
    }
}