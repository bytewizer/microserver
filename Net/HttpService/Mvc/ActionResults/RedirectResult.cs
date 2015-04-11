using System;
using System.IO;
using System.Text;

using Microsoft.SPOT;

using MicroServer.Utilities;
using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc
{
    /// <summary>
    /// Redirect to another url or controller/action.
    /// </summary>
    public class RedirectResult : ActionResult
    {
        private string _path;
        private string _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectResult"/> class.
        /// </summary>
        /// <param name="url">Uri to redirect to.</param>
        /// <remarks>
        /// Include "http://" in Uri to redirect to another site.
        /// </remarks>
        public RedirectResult(string url)
        {
            if (StringUtility.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url");
            }

            _url = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectResult"/> class.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public RedirectResult(string controllerName, string actionName)
        {
            if (StringUtility.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException("controllerName");
            }

            if (StringUtility.IsNullOrEmpty(actionName))
            {
                throw new ArgumentException("actionName");
            }

            _path = "/" + controllerName + "/" + actionName;
        }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {
            // Local controller/action redirect
            if (!StringUtility.IsNullOrEmpty(_path))
            {
                Uri url = context.Uri;
                string port = string.Empty;

                if (url.Port != 80)
                    port = string.Concat(";", url.Port);

                context.HttpContext.Request.UriRewrite = new Uri(
                    string.Concat(url.Scheme, "://", url.Host, port, _path));

                //Debug.Print(string.Concat(url.Scheme, "://", url.Host, port, _path));
            }

            // External rediect
            if (!StringUtility.IsNullOrEmpty(_url))
            {
                byte[] body = Encoding.UTF8.GetBytes(
                        @"<!DOCTYPE html><html><head><META http-equiv='refresh' content='0;URL=" + _url + @"'</head><body></body></html>");
                context.HttpContext.Response.Body = new MemoryStream(body);
            }
        }
    }
}