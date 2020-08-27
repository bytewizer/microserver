using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ActionResults
{
    /// <summary>
    /// An action result which redirect to another url or controller/action.
    /// </summary>
    public class RedirectResult : ActionResult
    {
        private string _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectResult"/> class with the values
        /// provided.
        /// </summary>
        /// <param name="url">The local URL to redirect to.</param>
        public RedirectResult(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectResult"/> class with the values
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public RedirectResult(string controllerName, string actionName)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException("controllerName");
            }

            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentException("actionName");
            }

            _url = "/" + controllerName + "/" + actionName;
        }

        /// <summary>
        /// Gets or sets the URL to redirect to.
        /// </summary>
        public string Url
        {
            get => _url;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(value));
                }

                _url = value;
            }
        }

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!string.IsNullOrEmpty(_url))
            {
                string content = $"<!DOCTYPE html><html><head><META http-equiv='refresh' content='0;URL= {_url}'</head><body></body></html>";
                context.HttpContext.Response.Write(content, "text/html");
            }
        }
    }
}