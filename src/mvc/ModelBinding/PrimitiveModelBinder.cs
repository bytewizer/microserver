using System;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    class PrimitiveModelBinder : IModelBinder
    {
        public bool CanBind(IModelBinderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modelType = context.ModelType;
            return modelType == typeof(string)
                || modelType == typeof(bool)
                || modelType == typeof(byte)
                || modelType == typeof(byte[])
                || modelType == typeof(sbyte)
                || modelType == typeof(short)
                || modelType == typeof(ushort)
                || modelType == typeof(int)
                || modelType == typeof(uint)
                || modelType == typeof(long)
                || modelType == typeof(ulong)
                || modelType == typeof(double);
        }

        public object Bind(IModelBinderContext context)
        {
            var name = string.IsNullOrEmpty(context.Prefix)
                           ? context.ModelName
                           : context.Prefix + "." + context.ModelName;

            var parameter = context.ValueProvider.Get(name);
            if (parameter == null)
                return null;

            object value = parameter;

            try
            {
                if (context.ModelType == typeof(string))
                {
                    return value;
                }

                if (context.ModelType == typeof(int))
                {
                    return value = int.Parse((string)value);
                }

                if (context.ModelType == typeof(bool))
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

                if (context.ModelType == typeof(byte))
                {
                    return value = byte.Parse((string)value);
                }

                if (context.ModelType == typeof(byte[]))
                {
                    return value =  Encoding.UTF8.GetBytes((string)value);
                }

                if (context.ModelType == typeof(sbyte))
                {
                    return value = sbyte.Parse((string)value);
                }

                if (context.ModelType == typeof(short))
                {
                    return value = short.Parse((string)value);
                }

                if (context.ModelType == typeof(ushort))
                {
                    return value = ushort.Parse((string)value);
                }

                if (context.ModelType == typeof(int))
                {
                    return value = int.Parse((string)value);
                }

                if (context.ModelType == typeof(uint))
                {
                    return value = uint.Parse((string)value);
                }

                if (context.ModelType == typeof(long))
                {
                    return value = long.Parse((string)value);
                }

                if (context.ModelType == typeof(ulong))
                {
                    return value = ulong.Parse((string)value);
                }

                if (context.ModelType == typeof(double))
                {
                    return value = double.Parse((string)value);
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