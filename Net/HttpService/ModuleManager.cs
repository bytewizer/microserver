using System;
using System.Collections;

using MicroServer.Net.Http.Modules;

namespace MicroServer.Net.Http
{
    /// <summary>
    /// Takes care of the module execution.
    /// </summary>
    /// <remarks><para>Important! The module manager will catch all exceptions, the last one is always attached to <see cref="IHttpContext.LastException"/>.</para>
    /// <para>
    /// It will however not do anything with the exception. You either have to have an error module which checks <see cref="IHttpContext.LastException"/>
    /// in <c>EndRequest()</c> or override the server to handle the error in it.
    /// </para>
    /// <para>Modules are invoked in the following order
    /// <list type="number">
    /// <item><see cref="IHttpModule.BeginRequest"/></item>
    /// <item><see cref="IRoutingModule"/></item>
    /// <item><see cref="IAuthenticationModule"/></item>
    /// <item><see cref="IAuthorizationModule"/></item>
    /// <item><see cref="IWorkerModule"/></item>
    /// <item><see cref="IHttpModule.EndRequest"/></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ModuleManager : IModuleManager
    {

        private readonly ArrayList _authenticationModules = new ArrayList();
        private readonly ArrayList _authorizationModules = new ArrayList();
        private readonly ArrayList _modules = new ArrayList();
        private readonly ArrayList _routingModules = new ArrayList();
        private readonly ArrayList _workerModules = new ArrayList();

        #region IModuleManager Members

        /// <summary>
        /// Add a HTTP module
        /// </summary>
        /// <param name="module">Module to include</param>
        /// <remarks>Modules are executed in the order they are added.</remarks>
        public void Add(IHttpModule module)
        {
            if (module == null) throw new ArgumentNullException("module");
            var worker = module as IWorkerModule;
            if (worker != null)
                _workerModules.Add(worker);

            var auth = module as IAuthenticationModule;
            if (auth != null)
                _authenticationModules.Add(auth);

            var auth2 = module as IAuthorizationModule;
            if (auth2 != null)
                _authorizationModules.Add(auth2);

            var routing = module as IRoutingModule;
            if (routing != null)
                _routingModules.Add(routing);

            _modules.Add(module);
        }

        /// <summary>
        /// Invoke all modules
        /// </summary>
        /// <param name="context"></param>
        /// <returns><c>true</c> if no modules have aborted the handling. Any module throwing an exception is also considered to be abort.</returns>
        public void InvokeAsync(IHttpContext context, AsyncModuleHandler callback)
        {
            
            var canContinue = HandleBeginRequest(context);
            if (canContinue)
                canContinue = InvokeModules(context, _authenticationModules, InvokeAuthenticate);
            if (canContinue)
                canContinue = InvokeModules(context, _routingModules, InvokeRouting);
            if (canContinue)
                canContinue = InvokeModules(context, _authorizationModules, InvokeAuthorize);
            if (canContinue)
            {
                var modules = _workerModules.GetEnumerator();        
                if (modules.MoveNext())
                {
                    ((IWorkerModule)modules.Current).HandleRequestAsync(context,
                                                       result => InvokeNextWorker(result, modules, innerResult =>
                                                           {
                                                               HandleEndRequest(context);
                                                               callback(innerResult);
                                                           }));
                }
                else
                {
                    callback(new AsyncModuleResult(context, ModuleResult.Continue));
                }
            }
            else
            {
                HandleEndRequest(context);
                callback(new AsyncModuleResult(context, ModuleResult.Stop));
            }
        }

        private void InvokeNextWorker(IAsyncModuleResult result, IEnumerator modules, AsyncModuleHandler callback)
        {
            if (result.Result == ModuleResult.Stop)
            {
                callback(result);
                return;
            }

            if (modules.MoveNext())
            {
                try
                {
                    ((IWorkerModule)modules.Current).HandleRequestAsync(result.Context, x => InvokeNextWorker(x, modules, callback));
                }
                catch (Exception err)
                {
                    result.Exception = err;
                    result.Result = ModuleResult.Stop;
                    callback(result);
                }
            }
            else
            {
                callback(result);
            }
        }

        #endregion

        private bool HandleBeginRequest(IHttpContext context)
        {
            var faulted = false;
            foreach (var httpModule in _modules)
            {
                try
                {
                    ((IHttpModule)httpModule).BeginRequest(context);
                }
                catch (Exception err)
                {
                    context.LastException = err;
                    faulted = true;
                }
            }

            return !faulted;
        }

        private void HandleEndRequest(IHttpContext context)
        {
            foreach (var httpModule in _modules)
            {
                try
                {
                    ((IHttpModule)httpModule).EndRequest(context);
                }
                catch (Exception err)
                {
                    context.LastException = err;
                }
            }
        }

        private ModuleResult InvokeAuthorize(IAuthorizationModule module, IHttpContext context)
        {
            return module.Authorize(context);
        }

        private ModuleResult InvokeRouting(IRoutingModule module, IHttpContext context)
        {
            return module.Route(context);
        }

        private ModuleResult InvokeAuthenticate(IAuthenticationModule arg1, IHttpContext arg2)
        {
            return arg1.Authenticate(arg2);
        }

        private bool InvokeModules(IHttpContext context, IEnumerable modules, InvokeAuthorizeHandler action)
        {
            try
            {
                foreach (IAuthorizationModule module in modules)
                {
                    if (action(module, context) == ModuleResult.Stop)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception err)
            {
                context.LastException = err;
                return false;
            }
        }

        private bool InvokeModules(IHttpContext context, IEnumerable modules, InvokeAuthenticateHandler action)
        {
            try
            {
                foreach (IAuthenticationModule module in modules)
                {
                    if (action(module, context) == ModuleResult.Stop)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception err)
            {
                context.LastException = err;
                return false;
            }
        }

        private bool InvokeModules(IHttpContext context, IEnumerable modules, InvokeRoutingHandler action)
        {
            try
            {
                foreach (IRoutingModule module in modules)
                {
                    if (action(module, context) == ModuleResult.Stop)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception err)
            {
                context.LastException = err;
                return false;
            }
        }

        #region Nested type: InvokeModuleHandler

        /// <summary>
        ///     Invoke the module handler
        /// </summary>
        private delegate ModuleResult InvokeAuthorizeHandler(IAuthorizationModule module, IHttpContext context);
        private delegate ModuleResult InvokeAuthenticateHandler(IAuthenticationModule module, IHttpContext context);
        private delegate ModuleResult InvokeRoutingHandler(IRoutingModule module, IHttpContext context);

        #endregion
    }
}