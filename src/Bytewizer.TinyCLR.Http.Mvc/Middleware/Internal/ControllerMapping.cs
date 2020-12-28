using System;
using System.Reflection;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.Middleware
{
    /// <summary>
    /// Contains uri to action method mappings for a controller
    /// </summary>
    /// <remarks>
    /// Mappings are made on action name in combination with argument count. This means that
    /// the same action can have multiple methods as long as they got different number of arguments.
    /// </remarks>
    internal class ControllerMapping
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerMapping"/> class.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        public ControllerMapping(Type controllerType)
        {
            ControllerType = controllerType;
        }

        /// <summary>
        /// Gets or sets URI to reach this route.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets type of controller to invoke
        /// </summary>
        public Type ControllerType { get; set; }

        public Hashtable Mappings { get; } = new Hashtable();

        /// <summary>
        /// Invoke an action method.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action.</param>
        /// <param name="arguments">Action arguments.</param>
        internal ActionResult Invoke(Controller instance, string action, object[] arguments)
        {
            MethodBase methodAction = (MethodBase)Mappings[action];
            return (ActionResult)methodAction.Invoke(instance, arguments);
        }

        /// <summary>
        /// Add a action method.
        /// </summary>
        /// <param name="method">The method.</param>
        public void Add(MethodInfo method)
        {
            Mappings.Add(method.Name.ToLower(), method);
        }

        /// <summary>
        /// Find a action method.
        /// </summary>
        /// <param name="actionName">The action name.</param>
        public virtual MethodInfo FindAction(string actionName)
        {
            if (Mappings.Contains(actionName.ToLower()))
            {
                return (MethodInfo)Mappings[actionName.ToLower()];
            }

            return null;
        }
    }
}