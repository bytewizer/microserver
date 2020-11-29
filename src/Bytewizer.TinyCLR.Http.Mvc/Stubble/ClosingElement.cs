using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.Stubble
{
    public class ClosingElement
    {
        public string Name;
        public int Start;
        public int End;
        public ArrayList Show { get; set; } = new ArrayList(); //public List<bool> Show { get; set; } = new List<bool>();
    }
}
