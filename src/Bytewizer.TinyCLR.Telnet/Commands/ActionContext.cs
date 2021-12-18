using System;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Context object for execution of action which has been selected as part of a telnet command.
    /// </summary>
    public class ActionContext
    {
        /// <summary>
        /// Creates an empty <see cref="ActionContext"/>.
        /// </summary>
        public ActionContext()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/> to copy.</param>
        public ActionContext(ActionContext actionContext)
            : this(
                actionContext.TelnetContext,
                actionContext.ActionDescriptor)
            {
        }

        /// <summary>
        /// Creates a new <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="telnetContext">The <see cref="TelnetContext"/> for the current telnet command.</param>
        /// /// <param name="actionDescriptor">The <see cref="ActionDescriptor"/> for the selected action.</param>
        public ActionContext(
            TelnetContext telnetContext, 
            ActionDescriptor actionDescriptor)
        {
            if (telnetContext == null)
            {
                throw new ArgumentNullException(nameof(telnetContext));
            }

            if (actionDescriptor == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptor));
            }

            TelnetContext = telnetContext;
            ActionDescriptor = actionDescriptor;
        }

        /// <summary>
        /// Gets or sets the <see cref="ActionDescriptor"/> for the selected action.
        /// </summary>
        public ActionDescriptor ActionDescriptor { get; internal set; }

        /// <summary>
        /// Gets the <see cref="Arguments"/> for the executing action.
        /// </summary>
        public ArgumentCollection Arguments => TelnetContext.Request.Command.Arguments;

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool Authenticated => TelnetContext.Request.Authenticated;

        /// <summary>
        /// Gets or sets the <see cref="TelnetContext"/> for the current request.
        /// </summary>
        public TelnetContext TelnetContext { get; }
    }
}