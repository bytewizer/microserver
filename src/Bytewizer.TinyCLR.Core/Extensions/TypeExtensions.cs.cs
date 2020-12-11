using System;
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
    }
}