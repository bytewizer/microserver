using System;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class ModelBindingException : Exception
    {
        public string ModelName { get; private set; }
        public object Value { get; private set; }

        public ModelBindingException(string message, string modelName, object value)
            : base(message)
        {
            ModelName = modelName;
            Value = value;
        }

        public ModelBindingException(string message)
            : base(message)
        {
        }

        public ModelBindingException(string message, string modelName, object value, Exception inner)
            : base(message, inner)
        {
            ModelName = modelName;
            Value = value;
        }

        public override string Message
        {
            get { return $"Binding failure '{ModelName}' = '{Value}', Error: { base.Message}"; }
        }
    }
}
