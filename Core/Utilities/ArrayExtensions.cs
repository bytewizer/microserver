using System;
using System.Text;
using System.Collections;

namespace MicroServer.Core.Extensions
{
    /// <summary>
    /// Extension methods for Array
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Adds a range of elements from the Array
        /// </summary>
        /// <param name="list">List to operate on</param>
        /// <param name="separator">Separator used to split the values</param>
        public static string Join(this Array list, string separator)
        {
            if (list.Length == 0)
                return String.Empty;

            if (list.Length == 1)
                return list.GetValue(0).ToString();

            var result = new StringBuilder(list.GetValue(0).ToString());
            for (var i = 1; i < list.Length; i++)
            {
                result.Append(separator);
                result.Append(list.GetValue(i));
            }

            return result.ToString();
        }
    }
}
