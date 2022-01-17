using System;
using System.Text;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// An action result which sends response content to the client. 
    /// </summary>
    public class ResponseResult : ActionResult
    {
        /// <summary>
        /// Initializes a new default instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        public ResponseResult()
        {
            Response = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        public ResponseResult(StringBuilder content)
        {
            Response = content;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        public ResponseResult(string content)
        {
            Response = new StringBuilder(content);
        }

        /// <summary>
        /// Gets or set the content representing this response.
        /// </summary>
        public StringBuilder Response { get; set; }

        /// <summary>
        /// Indicates if the last line should be appended with crlf.
        /// </summary>
        public bool AppendLastLineFeed { get; set; } = true;

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
       
            if(AppendLastLineFeed)
            {
                Response.AppendLine();
                context.TerminalContext.Response.WriteLine(Response.ToString());
            }
            else
            {
                context.TerminalContext.Response.Write(Response.ToString());
            }         
        }
    }
}