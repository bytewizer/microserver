using System;
using System.Collections;
using System.Reflection;

using Bytewizer.TinyCLR.Http.Mvc.Resolver;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    class ModelMapper : IModelBinder
    {
        private readonly ArrayList _binders = new ArrayList();

        public ModelMapper()
        {
            _binders.Add(new PrimitiveModelBinder());
            _binders.Add(new ArrayModelBinder());
            _binders.Add(new EnumModelBinder());
            _binders.Add(new ClassModelBinder());
        }

        public void Clear()
        {
            _binders.Clear();
        }

        public void AddBinder(IModelBinder binder)
        {
            _binders.Add(binder);
        }

        public object[] Bind(HttpRequest request, ParameterInfo[] parameters)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (parameters.Length == 0)
                return null;

            var provider = new RequestValueProvider(request);
            var binders = provider.GetValues();
            if (parameters.Length != binders.Length)
                return null;

            var x = 0;
            object[] results = new object[parameters.Length];
            
            foreach (DictionaryEntry item in binders)
            {
                var context = new ModelBinderContext(parameters[x].ParameterType, (string)item.Key, provider);
                foreach (IModelBinder modelBinder in _binders)
                {
                    if (modelBinder.CanBind(context))
                    {
                        results[x] = modelBinder.Bind(context);
                    }
                }
                x++;
            };

            return results;
        }

        public object Bind(HttpRequest request, Type type, string name)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var provider = new RequestValueProvider(request);

            if (!string.IsNullOrEmpty(name))
            {
                var context = new ModelBinderContext(type, name, provider);
                foreach (IModelBinder modelBinder in _binders)
                {
                    if (modelBinder.CanBind(context))
                    {
                        return modelBinder.Bind(context);
                    }
                }
            }

            return null;
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