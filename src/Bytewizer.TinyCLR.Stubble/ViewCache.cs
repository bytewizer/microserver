﻿using System.Collections;

namespace Bytewizer.TinyCLR.Stubble
{
    public static class ViewCache
    {
        public static IDictionary Cache { get; set; } = new Hashtable(); //public static Dictionary<string, SerializedView> cache { get; set; } = new Dictionary<string, SerializedView>();
    }
}
