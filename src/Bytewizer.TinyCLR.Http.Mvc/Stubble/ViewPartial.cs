using System;

namespace Bytewizer.TinyCLR.Stubble
{
    [Serializable]
    internal class ViewPartial
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Prefix { get; set; } //prefix used in html variable names after importing the partial
    }
}
