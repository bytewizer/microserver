using System;
using System.Collections;
using System.Reflection;

namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// Contains extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns all the public constructors defined for the current <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The current <see cref="Type"/></param>
        public static ConstructorInfo[] GetConstructors(this Type type)
        {
            BindingFlags bindingAttr = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public;
            return type.GetConstructors(bindingAttr);
        }

        /// <summary>
        /// searches for the constructors defined for the current <see cref="Type"/> using the specified <see cref="BindingFlags"/>.
        /// Specify <see cref="BindingFlags.CreateInstance"/> and <see cref="BindingFlags.Instance"/> along with one or both of <see cref="BindingFlags.Public"/> 
        /// and <see cref="BindingFlags.NonPublic"/> to retrieve instance constructors.
        /// </summary>
        /// <param name="type">The current <see cref="Type"/></param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        public static ConstructorInfo[] GetConstructors(this Type type, BindingFlags bindingAttr)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            MethodInfo[] methods = type.GetMethods(bindingAttr);
            ConstructorInfo[] constructors = new ConstructorInfo[methods.Length];

            for (int i = 0; i < constructors.Length; i++)
            {
                ParameterInfo[] constructorParameters = methods[i].GetParameters();

                Type[] parameters = new Type[constructorParameters.Length];
                for (int n = 0; n < parameters.Length; n++)
                {
                    parameters[n] = constructorParameters[n].ParameterType;
                }

                constructors[i] = methods[i].DeclaringType.GetConstructor(parameters);
            }

            return constructors;
        }

        ///// <summary>
        ///// Gets the properties of the current Type.
        ///// </summary>
        ///// <param name="type">The current <see cref="Type"/></param>
        //public static Hashtable GetProperties(this Type type)
        //{
        //    Hashtable properties = new Hashtable();

        //    // Type dump
        //    //Debug.WriteLine("Name: " + type.Name);
        //    //Debug.WriteLine("    IsClass: " + type.IsClass);
        //    //Debug.WriteLine("    IsArray: " + type.IsArray);
        //    //Debug.WriteLine("    IsEnum: " + type.IsEnum);
        //    //Debug.WriteLine("    IsAbstract: " + type.IsAbstract);
        //    //Debug.WriteLine("");

        //    // If it's a class then it's something we care about
        //    if (type.IsClass)
        //    {
        //        MethodInfo[] methods = type.GetMethods();
        //        foreach (MethodInfo method in methods)
        //        {
        //            //Debug.WriteLine("            Name: " + method.Name);
        //            //Debug.WriteLine("            IsVirtual: " + method.IsVirtual);
        //            //Debug.WriteLine("            IsStatic: " + method.IsStatic);
        //            //Debug.WriteLine("            IsPublic: " + method.IsPublic);
        //            //Debug.WriteLine("            IsFinal: " + method.IsFinal);
        //            //Debug.WriteLine("            IsAbstract: " + method.IsAbstract);
        //            ////Debug.WriteLine("            MemberType: " + method.MemberType);
        //            //Debug.WriteLine("            DeclaringType: " + method.DeclaringType);
        //            //Debug.WriteLine("            ReturnType: " + method.ReturnType);

        //            // If the Name.StartsWith "get_" and/or "set_",
        //            // and it's not Abstract && not Virtual
        //            // then it's a Property to save
        //            if ((method.Name.StartsWith("get_")) &&
        //                (method.IsAbstract == false) &&
        //                (method.IsVirtual == false))
        //            {
        //                // Ignore abstract and virtual objects
        //                if ((method.IsAbstract ||
        //                    (method.IsVirtual) ||
        //                    (method.ReturnType.IsAbstract)))
        //                {
        //                    continue;
        //                }

        //                // Ignore delegates and MethodInfos
        //                if ((method.ReturnType == typeof(Delegate)) ||
        //                    (method.ReturnType == typeof(MulticastDelegate)) ||
        //                    (method.ReturnType == typeof(MethodInfo)))
        //                {
        //                    continue;
        //                }

        //                // Same for DeclaringType
        //                if ((method.DeclaringType == typeof(Delegate)) ||
        //                    (method.DeclaringType == typeof(MulticastDelegate)))
        //                {
        //                    continue;
        //                }

        //                // Don't need these types either
        //                if ((method.Name.StartsWith("System.Globalization")))
        //                {
        //                    continue;
        //                }

        //                properties.Add(method.Name.Substring(4), method.ReturnType);
        //            }
        //        }
        //        return properties;
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// Determines whether an instance of a specified type can be assigned to a variable of the current type.
        ///// </summary>
        ///// <param name="type">The current <see cref="Type"/></param>
        ///// <param name="c">The type to compare with the current type.</param>
        //public static bool IsAssignableFrom(this Type type, Type c)
        //{
        //    return true;
        //}
    }
}