using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class EnumModelBinder : IModelBinder
    {
        public bool CanBind(IModelBinderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            return context.ModelType.IsEnum;
        }

        public object Bind(IModelBinderContext context)
        {    
            throw new NotImplementedException();
        }
    }
}