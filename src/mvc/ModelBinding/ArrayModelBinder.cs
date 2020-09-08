using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class ArrayModelBinder : IModelBinder
    {
        public bool CanBind(IModelBinderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            return context.ModelType.IsArray;
        }

        public object Bind(IModelBinderContext context)
        {            
            throw new NotImplementedException();
        }

        //private static object BuildIndexedArray(IModelBinderContext context, string fieldName, Type arrayType)
        //{
        //    var fields = new ArrayList();
        //    var indexes = new ArrayList();

        //    foreach (var item in context.ValueProvider.Find(fieldName))
        //    {
        //        var pos = item.Name.IndexOf(']', fieldName.Length);
        //        if (pos == -1)
        //            throw new ModelBindingException("Expected to find ']' to mark end of array.", fieldName + "]", item.Name);

        //        var name = item.Name.Substring(0, pos + 1);
        //        if (!fields.Contains(name))
        //        {
        //            fields.Add(name);

        //            //var index = ExtractIndex(name, fieldName);
        //            indexes.Add(index);
        //        }
        //    }

        //    if (fields.Count == 0)
        //        return null;

        //   // ValidateIndexes(indexes, fieldName);

        //    Array array = Array.CreateInstance(arrayType, fields.Count);
        //    for (var i = 0; i < fields.Count; i++)
        //    {
        //        var index = (int)indexes[i];

        //        //prefix already includes the field
        //        var value = context.Execute(arrayType, "", (string)fields[i]);
               
        //        array.SetValue(value, index);
        //    }

        //    return array;
        //}

        //private static int ExtractIndex(string name, string fieldName)
        //{
        //    var pos = name.IndexOf('[');
        //    if (pos == -1)
        //        throw new ModelBindingException("Failed to find '['.", fieldName + "]", name);

        //    name = name.Remove(0, pos + 1);
        //    pos = name.IndexOf(']');
        //    if (pos == -1)
        //        throw new ModelBindingException("Failed to find ']'.", fieldName + "]", name);

        //    name = name.Remove(pos);
        //    var index = 0;
        //    if (!int.TryParse(name, out index))
        //        throw new ModelBindingException("Failed to get indexer value.", name + "]", index);
        //    return index;
        //}
    }
}