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
            Response = content;
        }

        /// <summary>
        /// Gets or set the content representing this response.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or set the whether to include a new line in this response.
        /// </summary>
        public bool NewLine { get; set; } = true;


        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if(NewLine)
            {
                context.TelnetContext.Response.WriteLine(Response);
            }
            else
            {
                context.TelnetContext.Response.Write(Response);
            }         
        }
    }
}