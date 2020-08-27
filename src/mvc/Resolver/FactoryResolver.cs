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
    }
}