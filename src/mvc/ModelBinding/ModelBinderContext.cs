using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class ModelBinderContext : IModelBinderContext
    {
        public ModelBinderContext(Type modelType, string modelName, IValueProvider provider)
        {
            ModelName = modelName;
            ModelType = modelType;
            ValueProvider = provider;
        }

        public Type ModelType { get; private set; }

        public string ModelName { get; private set; }

        public IValueProvider ValueProvider { get; set; }

        public object Execute(Type modelType, string modelName)
        {
            throw new NotImplementedException();
        }
    }
}