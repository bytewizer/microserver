using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public interface IValueProvider
    {
        string Get(string name);
        DictionaryEntry[] GetValues();
    }
}
