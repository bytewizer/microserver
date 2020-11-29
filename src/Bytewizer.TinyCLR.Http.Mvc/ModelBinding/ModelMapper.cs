using System;
using System.Reflection;
using System.Collections;

using Bytewizer.TinyCLR.Http.Mvc.Resolver;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    class ModelMapper : IModelBinder
    {
        private readonly ArrayList _binders = new ArrayList();

        public ModelMapper()
        {
            // TODO: Additional Model Binders
            _binders.Add(new PrimitiveModelBinder());
        }

        public void Clear()
        {
            _binders.Clear();
        }

        public void AddBinder(IModelBinder binder)
        {
            _binders.Add(binder);
        }

        public object[] Bind(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var action = context.ActionDescriptor.MethodInfo;
            
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var parameters = action.GetParameters();

            if (parameters.Length == 0)
                return null;

            var provider = new RequestValueProvider(context.HttpContext.Request);
            
            var binders = provider.GetKeys();
            if (parameters.Length != binders.Count)
                return null;

            object[] results = new object[parameters.Length];

            var x = 0;
            foreach (string item in binders)
            {
                var modelContext = new ModelBinderContext(parameters[x].ParameterType, item, string.Empty, provider)
                {
                    RootBinder = this
                };
                foreach (IModelBinder modelBinder in _binders)
                {
                    if (modelBinder.CanBind(modelContext))
                    {
                        results[x] = modelBinder.Bind(modelContext);
                    }
                }
                x++;
            };

            return results;
        }

        bool IModelBinder.CanBind(IModelBinderContext context)
        {
            return true;
        }

        object IModelBinder.Bind(IModelBinderContext context)
        {
            foreach (IModelBinder modelBinder in _binders)
            {
                if (modelBinder.CanBind(context))
                {
                    return modelBinder.Bind(context);
                }
            }

            return context.ModelType.IsClass ? null : ServiceResolver.Current.Resolve(context.ModelType);
        }
    }
}