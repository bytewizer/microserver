using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    /// <summary>
    /// Represents an exception when attempting to bind to a model.
    /// </summary>
    public class ModelBindingException : Exception
    {
        /// <summary>
        /// Gets the model name which caused the exception.
        /// </summary>
        public string ModelName { get; private set; }

        /// <summary>
        /// Gets the model value which caused the exception.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBindingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ModelBindingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBindingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="modelName">The model name which caused the exception.</param>
        /// <param name="value">The model value which caused the exception.</param>
        public ModelBindingException(string message, string modelName, object value)
            : base(message)
        {
            ModelName = modelName;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBindingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="modelName">The model name which caused the exception.</param>
        /// <param name="value">The model value which caused the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public ModelBindingException(string message, string modelName, object value, Exception innerException)
            : base(message, innerException)
        {
            ModelName = modelName;
            Value = value;
        }

        /// <summary>
        /// The binding message.
        /// </summary>
        public override string Message
        {
            get { return $"Binding failure '{ModelName}' = '{Value}', Error: { base.Message}"; }
        }
    }
}
