using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class DateTimeModelBinder : IModelBinder
    {
        public bool CanBind(IModelBinderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modelType = context.ModelType;
            return modelType == typeof(DateTime)
                || modelType == typeof(TimeSpan);
        }

        public object Bind(IModelBinderContext context)
        {
            var name = context.ModelName;
            var parameter = context.ValueProvider.Get(name);
            if (parameter == null)
                return null;

            object value = parameter;

            try
            {
                //TODO:  Convert datetime and timespan
                if (context.ModelType == typeof(DateTime))
                {
                    
                    return value = null;
                }
                if (context.ModelType == typeof(TimeSpan))
                {
                    return value = null;
                }
            }
            catch (Exception ex)
            {
                throw new ModelBindingException(ex.Message, context.ModelName, value);
            }

            return null;
        }
    }
}