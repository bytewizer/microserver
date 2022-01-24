using Bytewizer.TinyCLR.Terminal.Features;
using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Context object for execution of action which has been selected as part of a terminal command.
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
                actionContext.TerminalContext,
                actionContext.ActionDescriptor)
            {
        }

        /// <summary>
        /// Creates a new <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="terminalContext">The <see cref="TerminalContext"/> for the current terminal command.</param>
        /// /// <param name="actionDescriptor">The <see cref="ActionDescriptor"/> for the selected action.</param>
        public ActionContext(
            TerminalContext terminalContext, 
            ActionDescriptor actionDescriptor)
        {
            if (terminalContext == null)
            {
                throw new ArgumentNullException(nameof(terminalContext));
            }

            if (actionDescriptor == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptor));
            }

            TerminalContext = terminalContext;
            ActionDescriptor = actionDescriptor;
        }

        /// <summary>
        /// Gets or sets the <see cref="ActionDescriptor"/> for the selected action.
        /// </summary>
        public ActionDescriptor ActionDescriptor { get; internal set; }

        /// <summary>
        /// Gets the <see cref="Arguments"/> for the executing action.
        /// </summary>
        public Hashtable Arguments => TerminalContext.Request.Command.Arguments;

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool Authenticated => ((SessionFeature)TerminalContext.Features.Get(typeof(SessionFeature))).Authenticated;

        /// <summary>
        /// Gets or sets the <see cref="TerminalContext"/> for the current request.
        /// </summary>
        public TerminalContext TerminalContext { get; }
    }
}