using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParsingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public static ArrayList SplitByComma(string source)
        {
            var result = new ArrayList();
            var values = source.Split(new char[] { ',' });

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value.Trim().ToUpper());
                }
            }

            return result;
        }
    }
}
