using System;
using System.Text;

namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// Contains extension methods for array.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Concatenates the elements of a specified array or the members 
        /// of a collection, using the specified separator between each element or member.
        /// </summary>
        /// <param name="list">The list to operate on</param>
        /// <param name="separator">Separator used to split the values.</param>
        public static string Join(this Array list, string separator)
        {
            if (list == null)
            {
                return null;
            }

            if (list.Length == 0)
            {
                return string.Empty;
            }

            if (list.Length == 1)
            {
                return list.GetValue(0).ToString();
            }

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