using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public interface IModelBinderContext
    {
        Type ModelType { get; }

        string ModelName { get; }

        IValueProvider ValueProvider { get; }

        object Execute(Type modelType, string modelName);
    }
}
