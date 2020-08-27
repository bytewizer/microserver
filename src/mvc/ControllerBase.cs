using System;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// A base class for an MVC controller.
    /// </summary>
    public abstract class ControllerBase
    {
        private ControllerContext _controllerContext;

        /// <summary>
        /// Gets the <see cref="Http.HttpContext"/> for the executing action.
        /// </summary>
        public HttpContext HttpContext => ControllerContext.HttpContext;

        /// <summary>
        /// Gets the <see cref="HttpRequest"/> for the executing action.
        /// </summary>
        public HttpRequest Request => HttpContext?.Request;

        /// <summary>
        /// Gets the <see cref="HttpResponse"/> for the executing action.
        /// </summary>
        public HttpResponse Response => HttpContext?.Response;

        /// <summary>
        /// Gets or sets the <see cref="Mvc.ControllerContext"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Controllers.IControllerActivator"/> activates this property while activating controllers.
        /// If user code directly instantiates a controller, the getter returns an empty
        /// <see cref="Mvc.ControllerContext"/>.
        /// </remarks>
        public ControllerContext ControllerContext
        {
            get
            {
                if (_controllerContext == null)
                {
                    _controllerContext = new ControllerContext();
                }

                return _controllerContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _controllerContext = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/> for user associated with the executing action.
        /// </summary>
        public string User => HttpContext?.User;

    }
}
