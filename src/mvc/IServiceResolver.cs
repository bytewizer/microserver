using System;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceResolver
    {
        object Resolve(Type type);
    }
}