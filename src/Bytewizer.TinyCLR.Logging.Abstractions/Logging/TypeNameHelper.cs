using System;
using System.Text;

namespace Bytewizer.TinyCLR.Logging
{
    internal static class TypeNameHelper
    {
        public static string GetTypeDisplayName(object item, bool fullName = true)
        {
            return item == null ? null : GetTypeDisplayName(item.GetType(), fullName);
        }

        /// <summary>
        /// Pretty print a type name.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="fullName"><c>true</c> to print a fully qualified name.</param>
        public static string GetTypeDisplayName(Type type, bool fullName = true)
        {
            var builder = new StringBuilder();
            
            var name = fullName ? type.FullName : type.Name;
            builder.Append(name);
            
            return builder.ToString();
        }
    }
}
