using System;
using System.Collections;
using System.Reflection;
using System.Text;

using Microsoft.SPOT;

using MicroServer.Net.Http.Mvc.Resolver;
using MicroServer.Net.Http.Mvc.ActionResults;


namespace MicroServer.Net.Http.Mvc.Controllers
{
    class ControllerFactory
    {
        private readonly Hashtable _controllers = new Hashtable();

        public IEnumerable Controllers
        {
            get { return _controllers; }
        }

        public virtual void Register(string uri, ControllerMapping controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");

            _controllers.Add(controller.Uri.ToLower(), controller);
        }

        /// <summary>
        /// Loads Uri mappings for all controllers and their actions
        /// </summary>
        public virtual void Load()
        {
            ControllerIndexer indexer = new ControllerIndexer();
            indexer.Find();

            foreach (ControllerMapping controller in indexer.Controllers)
            {
                foreach (DictionaryEntry mapping in controller.Mappings)
                {
                    _controllers.Add(controller.Uri.ToLower() + "/" + ((MethodInfo)mapping.Value).Name.ToString().ToLower(), controller);
                }
            }
        }

        public virtual bool TryMapping(string uri, out ControllerMapping mapping)
        {
            mapping = null;
            if (_controllers.Contains(uri))
            {
                mapping = (ControllerMapping)_controllers[uri];
                return true;
            }
            return false;
        }


        public virtual object Invoke(Type controllerType, MethodInfo action, IHttpContext context)
        {

            ControllerContext controllerContext = new ControllerContext(context);

            var controller = (Controller) ServiceResolver.Current.Resolve(controllerType);
            var newController = controller as IController;

            try
            {
                controllerContext.Controller = controller;
                controllerContext.ControllerName = controllerType.Name;
                controllerContext.ControllerUri = "/" + controllerType.Name;
                controllerContext.ActionName = action.Name;

                controller.SetContext(controllerContext);

                
                if (newController != null)
                {
                    var actionContext =  new ActionExecutingContext (controllerContext);
                    newController.OnActionExecuting(actionContext);
                    if (actionContext.Result != null)
                        return actionContext.Result;
                }
                object[] args = { controllerContext };
                ActionResult result = (ActionResult)action.Invoke(controller, args);
                result.ExecuteResult(controllerContext);

                if (newController != null)
                {
                    var actionContext = new ActionExecutedContext(controllerContext, false, null);
                    newController.OnActionExecuted(actionContext);
                    if (actionContext.Result != null)
                        return actionContext.Result;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                if (newController != null)
                {
                    var exceptionContext = new ExceptionContext(ex);
                    newController.OnException(exceptionContext);
                    if (exceptionContext.Result != null)
                        return exceptionContext.Result;
                }

                ActionResult result = (ActionResult) controller.TriggerOnException(ex);
                
                return result;
            }
        }
    }
}
