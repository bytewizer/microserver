using System;
using System.Text;
using System.Collections;

namespace MicroServer.Core.Extensions
{
    /// <summary>
    /// Extension methods for ArrayList
    /// </summary>
    public static class ArrayListExtensions
    {
        /// <summary>
        /// Add Range to ArrayList
        /// </summary>
        /// <param name="list">Aray List to add to</param>
        /// <param name="arr">Items to be added to the collection</param>
        public static void AddRange(this ArrayList list, Array arr)
        {
            foreach (object b in arr)
            {
                list.Add(b);
            }
        }

        /// <summary>
        /// Removes a range of elements from the ArrayList
        /// </summary>
        /// <param name="list">List to operate on</param>
        /// <param name="index">starting index</param>
        /// <param name="count"></param>
        public static void RemoveRange(this ArrayList list, int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Adds a range of elements from the Array
        /// </summary>
        /// <param name="list">List to operate on</param>
        /// <param name="separator">starting index</param>
        /// <param name="count"></param>
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
