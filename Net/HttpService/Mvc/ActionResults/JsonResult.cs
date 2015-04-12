using System;
using System.IO;
using System.Text;

using MicroServer.Utilities;
using MicroServer.Serializers.Json;
using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc.ActionResults
{
    /// <summary>
    /// Send JSON formated content to the client. 
    /// </summary>
    public class JsonResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResult"/> class.
        /// </summary>
        /// <param name="jdom">The JSON object body content.</param>
        public JsonResult(JObject jdom)
        {
            Data = jdom;
            ContentType = "application/json";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResult"/> class.
        /// </summary>
        /// <param name="jdom">The JSON object body content.</param>
        /// <param name="contentType">The content type header.</param>
        public JsonResult(JObject jdom, string contentType)
        {
            Data = jdom;
            ContentType = contentType;
        }

        /// <summary>
        /// Gets content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets JSON data object.
        /// </summary>
        public JObject Data { get; set; }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!StringUtility.IsNullOrEmpty(ContentType))
            {
                context.HttpContext.Response.ContentType = ContentType;
            }

            if (Data != null)
            {
                byte[] jbytes = Encoding.UTF8.GetBytes(JsonHelpers.Serialize(Data));

                context.HttpContext.Response.ContentType = ContentType;
                context.HttpContext.Response.Body = new MemoryStream(jbytes);
            }
        }
    }
}
