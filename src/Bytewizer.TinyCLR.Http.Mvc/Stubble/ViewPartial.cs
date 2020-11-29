using System;

namespace Bytewizer.TinyCLR.Http.Mvc.Stubble
{
    [Serializable]
    public class ViewPartial
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Prefix { get; set; } //prefix used in html variable names after importing the partial
    }
}
