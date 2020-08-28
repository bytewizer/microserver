using System;

using GHIElectronics.TinyCLR.Data.Json;

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
            ContentType = "application/json";
            StatusCode = StatusCodes.Status200OK;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the Content-Type header of the response.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

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
            var json = JsonConverter.Serialize(Value).ToString();
            context.HttpContext.Response.Write(json, "application/json");
        }
    }
}
