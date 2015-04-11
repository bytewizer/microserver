
using System;
using System.Collections;
using System.Reflection;
using System.Text;

using MicroServer.Extensions;
using Microsoft.SPOT;


namespace MicroServer.Net.Http.Mvc.Controllers
{
    public class ControllerIndexer
    {
        private ArrayList _controllers = new ArrayList();

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
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (type.IsSubclassOf(controllerType))
                    MapController(type);
            }
        }

        private void MapController(Type type)
        {
            ControllerMapping mapping = new ControllerMapping(type);
            mapping.Uri = type.Name.Replace("Controller", string.Empty);

            Type resultType = typeof(ActionResult);
            foreach (MethodInfo method in type.GetMethods())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (method.ReturnType.IsSubclassOf(resultType)  || method.ReturnType.Equals(resultType))
                    mapping.Add(method);
            }
            _controllers.Add(mapping);
        }
    }
}
