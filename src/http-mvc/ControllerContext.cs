using System;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// The context associated with the current request for a controller.
    /// </summary>
    public class ControllerContext : ActionContext
    {
        /// <summary>
        /// Creates a new <see cref="ControllerContext"/>.
        /// </summary>
        /// <remarks>
        /// The default constructor is provided for unit test purposes only.
        /// </remarks>
        public ControllerContext()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ControllerContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="ActionContext"/> associated with the current request.</param>
        public ControllerContext(ActionContext context)
            : base(context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                throw new ArgumentException(nameof(context));
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ControllerActionDescriptor"/> associated with the current request.
        /// </summary>
        public new ControllerActionDescriptor ActionDescriptor
        {
            get { return (ControllerActionDescriptor)base.ActionDescriptor; }
            set { base.ActionDescriptor = value; }
        }
    }
}