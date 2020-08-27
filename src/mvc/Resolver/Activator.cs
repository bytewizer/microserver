using System;

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
    }
}