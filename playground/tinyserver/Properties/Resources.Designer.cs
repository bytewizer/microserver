//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bytewizer.TinyCLR.TinyServer.Properties
{
    
    internal partial class Resources
    {
        private static System.Resources.ResourceManager manager;
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if ((Resources.manager == null))
                {
                    Resources.manager = new System.Resources.ResourceManager("Bytewizer.TinyCLR.TinyServer.Properties.Resources", typeof(Resources).Assembly);
                }
                return Resources.manager;
            }
        }
        internal static byte[] GetBytes(Resources.BinaryResources id)
        {
            return ((byte[])(ResourceManager.GetObject(((short)(id)))));
        }
        [System.SerializableAttribute()]
        internal enum BinaryResources : short
        {
            DeviceCert = 12948,
            DeviceKey = 20123,
        }
    }
}
