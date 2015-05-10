using System;
using System.Collections;
using System.Reflection;
using System.Text;

using Microsoft.SPOT;

using MicroServer.Logging;
using MicroServer.Net.Http.Modules;
using MicroServer.Net.Http.Messages;
using MicroServer.Net.Http.Mvc.ActionResults;

namespace MicroServer.Net.Http.Mvc.Controllers
{
    public class ControllerModule : IWorkerModule
    {
        ControllerFactory _controllerFactory = new ControllerFactory();

        public ControllerModule()
        {
            _controllerFactory.Load();

            Logger.WriteInfo(this, "Controller Url(s) Loaded:");
            foreach (DictionaryEntry controller in _controllerFactory.Controllers)
            {
                Logger.WriteInfo("  route url: /" + controller.Key.ToString());
            }
        }

        /// <summary>
        /// Key is complete uri to action
        /// </summary>
        private Hashtable _controllers = new Hashtable();

        public Hashtable Controllers
        {
            get { return _controllers; }
        }

        #region IWorkerModule Members

        /// <summary>
        /// Invoked before anything else
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The first method that is executed in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.
        /// <para>If you are going to handle the request, implement <see cref="IWorkerModule"/> and do it in the <see cref="IWorkerModule.HandleRequest"/> method.</para>
        /// </remarks>
        public void BeginRequest(IHttpContext context)
        {
            //Logger.WriteDebug(this, "Pipeline => BeginRequest");
        }

        /// <summary>
        /// Handle the request asynchronously.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="callback">callback</param>
        /// <returns><see cref="ModuleResult.Stop"/> will stop all processing except <see cref="IHttpModule.EndRequest"/>.</returns>
        /// <remarks>Invoked in turn for all modules unless you return <see cref="ModuleResult.Stop"/>.</remarks>
        public void HandleRequestAsync(IHttpContext context, AsyncModuleHandler callback)
        {
            //Logger.WriteDebug(this, "Pipeline => HandleRequestAsync");
            callback(new AsyncModuleResult(context, HandleRequest(context)));
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns><see cref="ModuleResult.Stop"/> will stop all processing except <see cref="IHttpModule.EndRequest"/>.</returns>
        /// <remarks>Invoked in turn for all modules unless you return <see cref="ModuleResult.Stop"/>.</remarks>
        public ModuleResult HandleRequest(IHttpContext context)
        {
            //Logger.WriteDebug(this, "Pipeline => HandleRequest");

            ControllerMapping mapping;
            string uri = context.Request.UriRewrite.AbsolutePath.TrimStart('/').TrimEnd('/').ToLower();

            if (_controllerFactory.TryMapping(uri, out mapping))
            {
                string actionName = uri.Substring(uri.LastIndexOf('/') + 1);

                MethodInfo action = mapping.FindAction(actionName);
                ActionResult result = (ActionResult)_controllerFactory.Invoke(mapping.ControllerType, action, context);

                return ModuleResult.Continue;
            }

            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "Page not found";
            return ModuleResult.Stop;
        }

        /// <summary>
        /// End request is typically used for post processing. The response should already contain everything required.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The last method that is executed in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.</remarks>
        public void EndRequest(IHttpContext context)
        {
            //Logger.WriteDebug(this, "Pipeline => EndRequest");
        }

        #endregion IWorkerModule Members
    }
}
