using System;

namespace Bytewizer.TinyCLR.Http.Mvc.Resolver
{
    /// <summary>
    /// Use this get constructor info.
    /// </summary>
    public class FactoryResolver : IServiceResolver
    {
        public object Resolve(Type type)
        {
            return type.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }

        public object Resolve(Type type, params object[] args)
        {
            Type[] argTypes = args != null ? new Type[args.Length] : new Type[0];

            for (int t = argTypes.Length - 1; t >= 0; t--)
            {
                argTypes[t] = args[t].GetType();
            }

            return type.GetConstructor(argTypes).Invoke(args);
        }
    }
}