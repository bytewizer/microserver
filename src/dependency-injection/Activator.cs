using System;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    public static class Activator
    {
        public static object CreateInstance(string typename)
        {
            Type type = Type.GetType(typename);
            
            if (type != null)
                return CreateInstance(type);
            else
                return null;
        }

        public static object CreateInstance(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }

        public static object CreateInstance(Type type, params object[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return type.GetConstructor(new Type[] { }).Invoke(parameters);
        }

        public static object CreateInstance(Type type, Type[] types, params object[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return type.GetConstructor(types).Invoke(parameters);
        }
    }
}