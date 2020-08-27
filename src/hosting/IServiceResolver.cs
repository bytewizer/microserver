using System;

namespace Bytewizer.TinyCLR.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceResolver
    {
        object Resolve(Type type);
    }
}