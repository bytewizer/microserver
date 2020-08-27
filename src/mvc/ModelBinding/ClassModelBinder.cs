using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class ClassModelBinder : IModelBinder
    {
        public bool CanBind(IModelBinderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.ModelType.IsClass
                && !context.ModelType.IsAbstract;
        }

        public object Bind(IModelBinderContext context)
        {
            if (context.ModelType.GetConstructor(new Type[0]) == null)
                throw new ModelBindingException("A default constructor is required.", context.ModelName, context.ModelType);

            throw new NotImplementedException();
        }
    }
}