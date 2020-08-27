using System;

namespace Bytewizer.TinyCLR.Hosting.Resolver
{
    public static class Activator
    {
        public static object CreateInstance(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }

        public static object CreateInstance(Type type, object[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetConstructor(new Type[] { }).Invoke(parameters);
        }
    }
}