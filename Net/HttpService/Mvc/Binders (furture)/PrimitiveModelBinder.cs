using System;

using MicroServer.Utilities;

namespace MicroServer.Net.Http.Binders
{
    /// <summary>
    /// Can bind primitives and <c>string</c>.
    /// </summary>
    public class PrimitiveModelBinder : IModelBinder
    {
        /// <summary>
        /// Determines whether this instance can bind the specified model.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the model; otherwise <c>false</c>.
        /// </returns>
        public bool CanBind(IModelBinderContext context)
        {
            return context.ModelType == typeof(String)
                || context.ModelType == typeof(Boolean)
                || context.ModelType == typeof(Byte)
                || context.ModelType == typeof(SByte)
                || context.ModelType == typeof(Int16)
                || context.ModelType == typeof(UInt16)
                || context.ModelType == typeof(Int32)
                || context.ModelType == typeof(UInt32)
                || context.ModelType == typeof(Int64)
                || context.ModelType == typeof(UInt64)
                || context.ModelType == typeof(Double);
        }

        /// <summary>
        /// Bind the model
        /// </summary>
        /// <param name="context">Context information</param>
        /// <returns>
        /// An object of the specified type (<seealso cref="IModelBinderContext.ModelType)" />
        /// </returns>
        public object Bind(IModelBinderContext context)
        {
            var name = StringUtility.IsNullOrEmpty(context.Prefix)
                           ? context.ModelName
                           : context.Prefix + "." + context.ModelName;
            var parameter = context.ValueProvider.Get(name);
            if (parameter == null)
                return null;

            object value = parameter.Value;

            try
            {
                if (context.ModelType == typeof(String))
                {
                    return value;
                }
                
                if (context.ModelType == typeof(Int32))
                {
                    return value =  Int32.Parse((string)value);
                }

                if (context.ModelType == typeof(Boolean))
                {

                    if ((string)value == "1" || ((string)value).ToUpper() == bool.TrueString.ToUpper())
                    {
                        return true;
                    }
                    else if ((string)value == "0" || ((string)value).ToUpper() == bool.FalseString.ToUpper())
                    {
                        return false;
                    }

                    return null;
                }

                if (context.ModelType == typeof(Byte))
                {
                    return value = Byte.Parse((string)value);
                }

                if (context.ModelType == typeof(SByte))
                {
                    return value = SByte.Parse((string)value);
                }

                if (context.ModelType == typeof(Int16))
                {
                    return value = Int16.Parse((string)value);
                }

                if (context.ModelType == typeof(UInt16))
                {
                    return value = UInt16.Parse((string)value);
                }

                if (context.ModelType == typeof(Int32))
                {
                    return value = Int32.Parse((string)value);
                }

                if (context.ModelType == typeof(UInt32))
                {
                    return value = UInt32.Parse((string)value);
                }

                if (context.ModelType == typeof(Int64))
                {
                    return value = Int64.Parse((string)value);
                }

                if (context.ModelType == typeof(UInt64))
                {
                    return value = UInt64.Parse((string)value);
                }

                if (context.ModelType == typeof(Double))
                {
                    return value = Double.Parse((string)value);
                }
            
            }
            catch (Exception ex)
            {
                throw new ModelBindingException(ex.Message, context.ModelName, value);
            }

            return null;
        }
    }
}