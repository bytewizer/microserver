using System;
using System.Reflection;
using System.Collections;

using Bytewizer.TinyCLR.Http.Mvc.Filters;
using Bytewizer.TinyCLR.Http.Mvc.Resolver;
using Bytewizer.TinyCLR.Http.Mvc.ModelBinding;

namespace Bytewizer.TinyCLR.Http.Mvc.Middleware
{
    class ControllerFactory
    {
        private readonly Hashtable _controllers = new Hashtable();
        private readonly ModelMapper _modelMapper = new ModelMapper();


        public IEnumerable Controllers
        {
            get { return _controllers; }
        }

        public virtual void Register(string uri, ControllerMapping controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");

            _controllers.Add(controller.Uri.ToLower(), controller);
        }

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

        public virtual object Invoke(Type controllerType, MethodInfo action, HttpContext context)
        {        
            ControllerContext controllerContext = new ControllerContext(context);

            var controller = (Controller)ServiceResolver.Current.Resolve(controllerType);
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
                    var actionContext = new ActionExecutingContext(controllerContext);
                    newController.OnActionExecuting(actionContext);
                    if (actionContext.Result != null)
                        return actionContext.Result;
                }

                object[] args = null;
                // TODO: ModelBinding
                //object[] args = new object[] { };
                //var parameters = action.GetParameters();
                //if (parameters.Length > 0)
                //{
                //    var httpRequest = controllerContext.HttpContext.Request;
                //    args = _modelMapper.Bind(httpRequest, parameters);
                //}
                ActionResult result = (ActionResult)action.Invoke(controller, args);
                result.ExecuteResult(controllerContext);

                if (newController != null)
                {
                    var actionContext = new ActionExecutedContext(controllerContext);
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
                    var exceptionContext = new ExceptionContext(controllerContext, ex);
                    newController.OnException(exceptionContext);
                    if (exceptionContext.Result != null)
                        return exceptionContext.Result;
                }

                ActionResult result = controller.TriggerOnException(ex);

                return result;
            }
        }

        private object[] MapParameters(MethodInfo action, string[] parameters)
        {
            if (parameters == null)
                return null;

            var methodParams = action.GetParameters();

            if (methodParams.Length != parameters.Length)
                return null;

            if (methodParams.Length == 0)
                return null;

            var paramVals = new object[methodParams.Length];
            for (var x = 0; x < methodParams.Length; x++)
            {
                var methodParamName = methodParams[x].ParameterType.Name;

                switch (methodParamName)
                {
                    case "String":
                        paramVals[x] = parameters[x];
                        break;
                    case "Int32":
                        if (int.TryParse(parameters[x], out int value))
                        { 
                            paramVals[x] = value;
                        }
                        else { paramVals[x] = null; }
                        break;
                    default:
                        break;
                }
            }
            return paramVals;



            //        for (var x = 0; x < methodParams.Length; x++)
            //        {
            //            //find correct key/value pair
            //            var param = "";
            //            var methodParamName = methodParams[x].Name.ToLower();
            //            var paramType = methodParams[x].ParameterType;

            //            foreach (var item in parameters)
            //            {
            //                if (item.Key == methodParamName)
            //                {
            //                    param = item.Value;
            //                    break;
            //                }
            //            }

            //            if (param == "")
            //            {
            //                //set default value for empty parameter
            //                if (paramType == typeof(Int32))
            //                {
            //                    param = "0";
            //                }
            //            }

            //            //cast params to correct (supported) types
            //            if (paramType.Name != "String")
            //            {
            //                if (int.TryParse(param, out int i) == true)
            //                {
            //                    if (paramType.IsEnum == true)
            //                    {
            //                        //convert param value to enum
            //                        paramVals[x] = Enum.Parse(paramType, param);
            //                    }
            //                    else
            //                    {
            //                        //convert param value to matching method parameter number type
            //                        paramVals[x] = Convert.ChangeType(i, paramType);
            //                    }

            //                }
            //                else if (paramType.FullName.Contains("DateTime"))
            //                {
            //                    //convert param value to DateTime
            //                    if (param == "")
            //                    {
            //                        paramVals[x] = null;
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            paramVals[x] = DateTime.Parse(param);
            //                        }
            //                        catch (Exception) { }
            //                    }
            //                }
            //                else if (paramType.IsArray)
            //                {
            //                    //convert param value to array (of T)
            //                    var arr = param.Replace("[", "").Replace("]", "").Replace("\r", "").Replace("\n", "").Split(",").Select(a => { return a.Trim(); }).ToList();
            //                    if (paramType.FullName == "System.Int32[]")
            //                    {
            //                        //convert param values to int array
            //                        paramVals[x] = arr.Select(a => { return int.Parse(a); }).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //convert param values to array (of matching method parameter type)
            //                        paramVals[x] = Convert.ChangeType(arr, paramType);
            //                    }


            //                }
            //                else if (paramType.Name.IndexOf("Dictionary") == 0)
            //                {
            //                    //convert param value (JSON) to Dictionary
            //                    paramVals[x] = JsonSerializer.Deserialize<Dictionary<string, string>>(param);
            //                }
            //                else if (paramType.Name == "Boolean")
            //                {
            //                    paramVals[x] = param.ToLower() == "true";
            //                }
            //                else
            //                {
            //                    //convert param value to matching method parameter type
            //                    paramVals[x] = JsonSerializer.Deserialize(param, paramType);
            //                }
            //            }
            //            else
            //            {
            //                //matching method parameter type is string
            //                paramVals[x] = param;
            //            }
            //        }
            //        return paramVals;
        }
    }
}
