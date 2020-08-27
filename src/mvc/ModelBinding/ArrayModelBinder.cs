using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class ArrayModelBinder : IModelBinder
    {
        public bool CanBind(IModelBinderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            return context.ModelType.IsArray;
        }

        public object Bind(IModelBinderContext context)
        {
            throw new NotImplementedException();
        }
    }
}