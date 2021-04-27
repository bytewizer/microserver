using GHIElectronics.TinyCLR.Data.Json;
using System;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// An action result which formats the given object as JSON and sends to the client.
    /// </summary>
    public class JsonResult : ActionResult
    {
        /// <summary>
        /// Creates a new <see cref="JsonResult"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to format as JSON.</param>
        public JsonResult(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the Content-Type header of the response.
        /// </summary>
        public string ContentType { get; set; } = "application/json; charset=UTF-8";

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        /// <summary>
        /// Gets or sets a value that defines whether JSON should use pretty printing.
        /// </summary>
        public bool Indented { get; set; } = false;

        /// <summary>
        /// Gets or sets the value to be formatted.
        /// </summary>
        public object Value { get; set; }

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var options = new JsonSerializationOptions() { Indented = Indented };

            context.HttpContext.Response.WriteJson(Value, ContentType, StatusCode, Encoding.UTF8, options);
        }
    }
}