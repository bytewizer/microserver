using System;
using System.Text;

using MicroServer.Logging;
using MicroServer.Utilities;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Mvc.Controllers;
using System.IO;
using MicroServer.Serializers.Token;
using Microsoft.SPOT;

namespace MicroServer.Net.Http.Mvc
{ 
    /// <summary>
    /// Send binary content to the client. 
    /// </summary>
    public class TokenResult : ActionResult
    {
 
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResult"/> class.
        /// </summary>
        /// <param name="content">The body content.</param>
        /// <param name="contentType">The content type header.</param>
        public TokenResult(string content, string contentType)
        {
            if (StringUtility.IsNullOrEmpty(content))
            {
                throw new ArgumentException("content");
            }

            if (StringUtility.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType");
            }

            Tokens = new TokenCollection();
            Content = content;
            ContentType = contentType;
        }

        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        public TokenCollection Tokens { get; private set; }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {

            Logger.WriteDebug(this, "Pipeline => ExecuteResult");

            TokenParser _parser = new TokenParser(Content, Tokens);
            
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!StringUtility.IsNullOrEmpty(ContentType))
            {
                context.HttpContext.Response.ContentType = ContentType;
            }

            if (!StringUtility.IsNullOrEmpty(Content))
            {
                //Debug.Print(_parser.ToString());
                
                MemoryStream body = new MemoryStream(Encoding.UTF8.GetBytes(_parser.ToString()));   
                context.HttpContext.Response.Body = body;
            }
        }
    }
}