using System;
using System.Reflection;
using System.Collections;

using Microsoft.SPOT;

using MicroServer.Extensions;

namespace MicroServer.Utilities
{
    public static class TypeUtility
    {

        /// <summary>
        /// High-level method that enumerates all Types in the loaded assembly,
        /// and places their class names and properties in a hashtable.  At deserialization
        /// time, this Hashtable (containing Property Names and Types).
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Hashtable GetProperties(Type[] types)
        {
            Hashtable properties = new Hashtable();

            foreach (Type type in types)
            {
                Hashtable entry = GetProperties(type);
                if (entry != null)
                {
                    properties.Add(type.FullName, entry);
                }
            }
            return properties;
        }
           
        public static Hashtable GetProperties(Type type)
        {
            Hashtable properties = new Hashtable();
            
            // Type dump
            //Debug.Print("Name: " + t.Name);
            //Debug.Print("    IsClass: " + t.IsClass);
            //Debug.Print("    IsArray: " + t.IsArray);
            //Debug.Print("    IsEnum: " + t.IsEnum);
            //Debug.Print("    IsAbstract: " + t.IsAbstract);
            //Debug.Print("");

            // If it's a class, then it's something we care about
            if (type.IsClass)
            {
                MethodInfo[] methods = type.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    //Debug.Print("            Name: " + method.Name);
                    //Debug.Print("            IsVirtual: " + method.IsVirtual);
                    //Debug.Print("            IsStatic: " + method.IsStatic);
                    //Debug.Print("            IsPublic: " + method.IsPublic);
                    //Debug.Print("            IsFinal: " + method.IsFinal);
                    //Debug.Print("            IsAbstract: " + method.IsAbstract);
                    //Debug.Print("            MemberType: " + method.MemberType);
                    //Debug.Print("            DeclaringType: " + method.DeclaringType);
                    //Debug.Print("            ReturnType: " + method.ReturnType);

                    // If the Name.StartsWith "get_" and/or "set_",
                    // and it's not Abstract && not Virtual
                    // then it's a Property to save
                    if ((method.Name.StartsWith("get_")) &&
                        (method.IsAbstract == false) &&
                        (method.IsVirtual == false))
                    {
                        // Ignore abstract and virtual objects
                        if ((method.IsAbstract ||
                            (method.IsVirtual) ||
                            (method.ReturnType.IsAbstract)))
                        {
                            continue;
                        }

                        // Ignore delegates and MethodInfos
                        if ((method.ReturnType == typeof(System.Delegate)) ||
                            (method.ReturnType == typeof(System.MulticastDelegate)) ||
                            (method.ReturnType == typeof(System.Reflection.MethodInfo)))
                        {
                            continue;
                        }

                        // Same for DeclaringType
                        if ((method.DeclaringType == typeof(System.Delegate)) ||
                            (method.DeclaringType == typeof(System.MulticastDelegate)))
                        {
                            continue;
                        }

                        // Don't need these types either
                        if ((method.Name.StartsWith("System.Globalization")))
                        {
                            continue;
                        }

                        properties.Add(method.Name.Substring(4), method.ReturnType);
                    }
                }
                return properties;
            }

            return null;
        }  
    }
}
