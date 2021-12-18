using System;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// An action result which sends response content to the client. 
    /// </summary>
    public class ResponseResult : ActionResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        public ResponseResult(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Gets or set the response content representing.
        /// </summary>
        public string Content { get; set; }


        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.TelnetContext.Response.WriteLine(Content);
        }
    }
}