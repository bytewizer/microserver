using System;

namespace MicroServer.Net.Http.Binders
{
    public class ModelBindingException : Exception
    {
        public string FullModelName { get; private set; }
        public object Value { get; private set; }

        public ModelBindingException(string message, string fullModelName, object value)
            :base(message)
        {
            FullModelName = fullModelName;
            Value = value;
        }

        public ModelBindingException(string message)
            : base(message)
        {
        }

        public ModelBindingException(string message, string fullModelName, object value, Exception inner)
            : base(message, inner)
        {
            FullModelName = fullModelName;
            Value = value;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get { return string.Concat("Binding failure '", FullModelName, "' = '", Value, "', Error: ", base.Message); }
        }
    }
}