using System;
using System.Collections;
using System.Reflection;

namespace Bytewizer.TinyCLR.Stubble
{
    public class ViewChild
    {
        public ViewDictionary Data { get; set; }
        public Hashtable Fields = new Hashtable();

        public ViewChild(ViewData parent, string id)
        {
            Data = new ViewDictionary(parent, id);
            
            //load related fields    
            foreach (DictionaryEntry item in (parent.Fields))
            {
                if (((string)item.Key).IndexOf(id + "-") == 0)
                {
                    Fields.Add(((string)item.Key).Replace(id + "-", ""), item.Value);
                }
            }
        }

        public string this[string key]
        {
            get
            {
                return Data[key];
            }
            set
            {
                Data[key] = value;
            }
        }

        public void Show(string blockKey)
        {
            Data[blockKey] = "True";
        }

        public void Bind(object obj, string root = "")
        {
            if (obj != null)
            {
                foreach (MethodInfo method in obj.GetType().GetMethods())
                {
                    if (!method.IsPublic)
                        continue;

                    if (method.Name.IndexOf("get_") == 0)
                    {
                        object val = method.Invoke(obj, null);

                        var name = (root != "" ? root + "." : "") + method.Name.ToLower();
                        if (val == null)
                        {
                            Data[name] = "";
                        }
                        else if (val is string || val is int || val is long || val is double || val is decimal || val is short)
                        {
                            //add property value to dictionary
                            Data[name] = val.ToString();
                        }
                        else if (val is bool)
                        {
                            Data[name] = (bool)val == true ? "1" : "0";
                        }
                        else if (val is DateTime)
                        {
                            Data[name] = ((DateTime)val).ToString() + " " + ((DateTime)val).ToString();
                        }
                        else if (val is object)
                        {
                            //recurse child object for properties
                            Bind(val, name);
                        }
                    }
                }
            }
        }
    }
}
