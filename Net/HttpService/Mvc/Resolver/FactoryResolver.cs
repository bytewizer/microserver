using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace MicroServer.Net.Http.Mvc.Resolver
{
    /// <summary>
    /// Use this get constructor info.
    /// </summary>
    public class FactoryResolver : IServiceResolver
    {
        public object Resolve(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { });
            return constructorInfo.Invoke(new object[] { });
        }
    }
}
