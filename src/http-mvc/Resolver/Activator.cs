using System;
using System.Reflection;

namespace Bytewizer.TinyCLR.Http.Mvc.Resolver
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

        public static object CreateInstance(this Type type, params object[] args)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type[] argTypes = args != null ? new Type[args.Length] : new Type[0];

            for (int t = argTypes.Length - 1; t >= 0; t--)
            {
                argTypes[t] = args[t].GetType();
            }

            return type.GetConstructor(argTypes).Invoke(args);
        }
    }
}