using System;
using System.Text;

namespace Bytewizer.TinyCLR.Terminal
{
    internal static class ModelMapper
    {
        public static bool CanBind(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            return modelType == typeof(string)
                || modelType == typeof(bool)
                || modelType == typeof(byte)
                || modelType == typeof(byte[])
                || modelType == typeof(sbyte)
                || modelType == typeof(sbyte[])
                || modelType == typeof(short)
                || modelType == typeof(ushort)
                || modelType == typeof(int)
                || modelType == typeof(uint)
                || modelType == typeof(long)
                || modelType == typeof(ulong)
                || modelType == typeof(double);
        }

        public static object[] Bind(object[] actionParameters, string[] parameters)
        {
            object[] results = new object[actionParameters.Length];
            for (int i = 0; i < actionParameters.Length; i++)
            {
                if (i < parameters.Length)
                {
                    results[i] = Bind((Type)actionParameters[i], parameters[i]);
                }
                else
                {
                    results[i] = Bind((Type)actionParameters[i], null);
                }
            }

            return results;
        }

        public static object Bind(Type modelType, object parameter)
        {
            string value = (string)parameter;

            try
            {
                if (modelType == typeof(string))
                {
                    return value;
                }

                if (modelType == typeof(int))
                {
                    return parameter is null ? new int() : int.Parse(value);
                }

                if (modelType == typeof(bool))
                {

                    if (value == "1" || (value).ToUpper() == bool.TrueString.ToUpper())
                    {
                        return true;
                    }
                    else if (value == "0" || (value).ToUpper() == bool.FalseString.ToUpper())
                    {
                        return false;
                    }

                    return new bool();
                }

                if (modelType == typeof(byte))
                {
                    return parameter is null ? new byte() : byte.Parse(value);
                }

                if (modelType == typeof(byte[]))
                {
                    return parameter is null ? new byte[] { } : Encoding.UTF8.GetBytes(value);
                }

                if (modelType == typeof(sbyte))
                {
                    return parameter is null ? new sbyte() : sbyte.Parse(value);
                }

                if (modelType == typeof(sbyte[]))
                {
                    return parameter is null ? new sbyte[] { } : Encoding.UTF8.GetBytes(value);
                }

                if (modelType == typeof(short))
                {
                    return parameter is null ?  new short() : short.Parse(value);
                }

                if (modelType == typeof(ushort))
                {
                    return parameter is null ? new ushort() : ushort.Parse(value);
                }

                if (modelType == typeof(uint))
                {
                    return parameter is null ? new uint() : uint.Parse(value);
                }

                if (modelType == typeof(long))
                {
                    return parameter is null ? new long() : long.Parse(value);
                }

                if (modelType == typeof(ulong))
                {
                    return parameter is null ? new ulong() : ulong.Parse(value);
                }

                if (modelType == typeof(double))
                {
                    return parameter is null ? new double(): double.Parse(value);
                }
            }
            catch
            {
                throw new Exception($"The specified parameter '{value}' failed to parse as '{modelType}' type.");
            }

            return null;
        }
    }
}
