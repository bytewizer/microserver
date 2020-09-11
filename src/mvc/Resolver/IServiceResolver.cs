using System;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    public interface IServiceResolver
    {
        object Resolve(Type type);
        object Resolve(Type type, params object[] args);
    }
}