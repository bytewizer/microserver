using System.Reflection;

namespace System
{
    /// <summary>
    /// Contains methods to create types of objects locally. This class cannot be inherited.
    /// </summary>
    public static class Activator
    {
        /// <summary>
        /// Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="typename">The fully qualified name of the type to create an instance of.</param>
        public static object CreateInstance(string typename)
        {
            Type type = Type.GetType(typename);

            if (type != null)
                return CreateInstance(type);
            else
                return null;
        }

        /// <summary>
        /// Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        public static object CreateInstance(Type type)
        {
            return CreateInstance(type, new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. 
        /// If args is an empty array or null, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
        public static object CreateInstance(Type type, params object[] args)
        {
            Type[] types = args != null ? new Type[args.Length] : new Type[] { };

            for (int i = types.Length - 1; i >= 0; i--)
            {
                types[i] = args[i]?.GetType();
            }

            return CreateInstance(type, types, args);
        }

        /// <summary>
        /// Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <param name="types">An array of Type objects representing the number, order, and type of the parameters for the desired constructor.
        /// If types is an empty array or null, to get constructor that takes no parameters.</param>
        /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. 
        /// If args is an empty array or null, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
        public static object CreateInstance(Type type, Type[] types, params object[] args)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (types == null)
            {
                types = new Type[] { };
            }

            if (args == null)
            {
                args = new object[] { };
            }

            return type.GetConstructor(types).Invoke(args);
        }

        ///// <summary>
        ///// Retrieves the default value for a given Type.
        ///// </summary>
        ///// <param name="type">The Type for which to get the default value.</param>
        ///// <returns>The default value for <paramref name="type"/></returns>
        //private static object GetDefaultValue(Type type)
        //{
        //    // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
        //    //if (type == null || !type.IsValueType || type == typeof(void))
        //    //    return null;

        //    if (type == typeof(string))
        //    {
        //        return default(string);
        //    }

        //    if (type == typeof(bool))
        //    {
        //        return default(bool);
        //    }

        //    if (type == typeof(byte))
        //    {
        //        return default(byte);
        //    }

        //    if (type == typeof(byte[]))
        //    {
        //        return default(byte[]);
        //    }

        //    if (type == typeof(sbyte))
        //    {
        //        return default(sbyte);
        //    }

        //    if (type == typeof(sbyte[]))
        //    {
        //        return default(sbyte[]);
        //    }

        //    if (type == typeof(short))
        //    {
        //        return default(short);
        //    }

        //    if (type == typeof(ushort))
        //    {
        //        return default(ushort);
        //    }

        //    if (type == typeof(int))
        //    {
        //        return default(int);
        //    }

        //    if (type == typeof(uint))
        //    {
        //        return default(uint);
        //    }

        //    if (type == typeof(long))
        //    {
        //        return default(long);
        //    }

        //    if (type == typeof(ulong))
        //    {
        //        return default(ulong);
        //    }

        //    if (type == typeof(double))
        //    {
        //        return default(double);
        //    }

        //    return null;
        //}

        //public static object GetDefaultValue2(Type parameter)
        //{
        //    return parameter.InvokeMember(
        //      "GetDefault",
        //      BindingFlags.Static |
        //      BindingFlags.Public |
        //      BindingFlags.InvokeMethod,
        //      null, null, new object[0]);
        //}
    }
}

