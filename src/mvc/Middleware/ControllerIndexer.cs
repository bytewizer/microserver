using System;
using System.Collections;
using System.Reflection;

using Bytewizer.TinyCLR.Http.Extensions;

namespace Bytewizer.TinyCLR.Http.Mvc.Middleware
{
    public class ControllerIndexer
    {
        private readonly ArrayList _controllers = new ArrayList();

        public IEnumerable Controllers
        {
            get { return _controllers; }
        }

        public void Find()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Find(assembly);
            }
        }

        public void Find(Assembly assembly)
        {
            Type controllerType = typeof(Controller);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface || type.IsNotPublic)
                    continue;

                if (type.IsSubclassOf(controllerType))
                    MapController(type);
            }
        }

        private void MapController(Type type)
        {
            ControllerMapping mapping = new ControllerMapping(type)
            {
                Uri = type.Name.Replace("Controller", string.Empty)
            };

            foreach (MethodInfo method in type.GetMethods())
            {
                if (type.IsAbstract || type.IsNotPublic)
                    continue;

                if (method.ReturnType.Equals(typeof(IActionResult)))
                    mapping.Add(method);
            }
            _controllers.Add(mapping);
        }
    }
}
