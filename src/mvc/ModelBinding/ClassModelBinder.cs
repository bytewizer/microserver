using Bytewizer.TinyCLR.Http.Mvc.Resolver;
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

            var model = Activator.CreateInstance(context.ModelType);
            var prefix = string.IsNullOrEmpty(context.Prefix) ? context.ModelName : context.Prefix + "." + context.ModelName;


            //foreach (var property in context.ModelType.GetProperties())
            //{
            //    if (!property.CanWrite)
            //        continue;

            //    var value = context.Execute(property.PropertyType, prefix, property.Name);

            //    property.SetValue(model, value, null);
            //}

            throw new NotImplementedException();
        }
    }
}