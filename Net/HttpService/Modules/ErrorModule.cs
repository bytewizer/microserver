using System;
using System.IO;

namespace MicroServer.Net.Http.Modules
{
    /// <summary>
    /// Reports errors to different sources.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var module = new ErrorModule();
    /// ]]>
    /// </code>
    /// </example>
    public class ErrorModule : IHttpModule
    {
        #region IHttpModule Members

        /// <summary>
        /// Invoked before anything else
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The first method that is exeucted in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.</remarks>
        public void BeginRequest(IHttpContext context)
        {
        }

        /// <summary>
        /// End request is typically used for post processing. The response should already contain everything required.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The last method that is executed in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.</remarks>
        public void EndRequest(IHttpContext context)
        {
            if (context.LastException == null)
                return;

            string errorPage = "<html><body>Opps, fail with exception: " + context.LastException + "</html></body>";

            var bytes = context.Response.ContentCharset.GetBytes(errorPage);
            context.Response.Body = new MemoryStream();
            context.Response.Body.Write(bytes, 0, bytes.Length);
            context.Response.ContentType = "text/html";
        }

        #endregion
    }
}