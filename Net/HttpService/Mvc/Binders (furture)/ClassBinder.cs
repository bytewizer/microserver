using System;

using MicroServer.Utilities;
using MicroServer.Net.Http.Mvc.Resolver;

namespace MicroServer.Net.Http.Binders
{
    /// <summary>
    /// Can bind classes which are not abstract or generic.
    /// </summary>
    public class ClassBinder : IModelBinder
    {
        /// <summary>
        /// Determines whether this instance can bind the specified model.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the model; otherwise <c>false</c>.
        /// </returns>
        public bool CanBind(IModelBinderContext context)
        {
            return context.ModelType.IsClass
                && !context.ModelType.IsAbstract; 
                //&& !context.ModelType.IsGenericType;
        }

        /// <summary>
        /// Bind the model
        /// </summary>
        /// <param name="context">Context information</param>
        /// <returns>
        /// An object of the specified type (<seealso cref="IModelBinderContext.ModelType)" />
        /// </returns>
        public object Bind(IModelBinderContext context)
        {
            if (context.ModelType.GetConstructor(new Type[0]) == null)
                throw new ModelBindingException("Do not have a default constructor.", context.ModelName, context.ModelType);

            //var model = Activator.CreateInstance(context.ModelType);
            var model = ServiceResolver.Current.Resolve(context.ModelType);
            var prefix = StringUtility.IsNullOrEmpty(context.Prefix) ? context.ModelName : context.Prefix + "." + context.ModelName;
            
            //foreach (Type property in context.ModelType.GetProperties())
            //{
            //    if (!property.CanWrite)
            //        continue;

            //    var value = context.Execute(property.PropertyType, prefix, property.Name);

            //    property.SetValue(model, value, null);

            //}

            return model;
        }
    }
}