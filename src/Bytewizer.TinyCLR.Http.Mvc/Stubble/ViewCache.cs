using System.Collections;

namespace Bytewizer.TinyCLR.Stubble
{
    internal static class ViewCache
    {
        public static IDictionary Cache { get; set; } = new Hashtable(); //public static Dictionary<string, SerializedView> cache { get; set; } = new Dictionary<string, SerializedView>();
    }
}
