using MicroServer.Net.Http.Mvc.Resolver;
using MicroServer.Utilities;
using System;
using System.Collections;

namespace MicroServer.Net.Http.Binders
{
    public class ModelMapper : IModelBinder
    {
        private ArrayList  _binders = new ArrayList();

        public ModelMapper()
        {
            _binders.Add(new PrimitiveModelBinder());
            //_binders.Add(new EnumModelBinder());
            //_binders.Add(new ArrayModelBinder());
            //_binders.Add(new DictionaryModelBinder());
            //_binders.Add(new ClassBinder());
        }

        /// <summary>
        /// Remove all binders
        /// </summary>
        public void Clear()
        {
            _binders.Clear();
        }

        public void AddBinder(IModelBinder binder)
        {
            _binders.Add(binder);
        }


        //public Type Bind(Type typeBinder, IHttpRequest request, string name)
        //{
        //    var provider = new RequestValueProvider(request as IHttpMessage);

        //    if (!StringUtility.IsNullOrEmpty(name))
        //    {
        //        var context = new ModelBinderContext(typeBinder, name, "", provider);
        //        context.RootBinder = this;

        //        foreach (IModelBinder modelBinder in _binders)
        //        {
        //            if (modelBinder.CanBind(context))
        //            {
        //                return (typeBinder)modelBinder.Bind(context);
        //            }
        //        }

        //        return default(typeBinder);
        //    }
        //    //if (IEnumerable).IsInstanceOfTypetypeBinder)
        //    //    throw new InvalidOperationException("did not expect IEnumerable implementations without a name in the binder.");

        //    //var model = Activator.CreateInstance(typeof(typeBinder));
        //    var model = ServiceResolver.Current.Resolve(typeBinder);

        //    foreach (var property in model.GetType().GetProperties())
        //    {
        //        var context = new ModelBinderContext(property.PropertyType, property.Name, "", provider);
        //        context.RootBinder = this;
        //        foreach (var modelBinder in _binders)
        //        {
        //            if (modelBinder.CanBind(context))
        //            {
        //                var value = modelBinder.Bind(context);
        //                property.SetValue(model, value, null);
        //                break;
        //            }
        //        }
        //    }
        //    return (typeBinder)model;
        //}
        
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

            return context.ModelType.IsClass ? null : ServiceResolver.Current.Resolve(context.ModelType); //Activator.CreateInstance(context.ModelType);
        }
    }
}