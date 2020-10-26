using System;
using System.Collections;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    internal class SingletonCache
    {
        static readonly SingletonCache instance = null;
        static readonly Hashtable objectPool = new Hashtable();

        static SingletonCache()
        {
            instance = new SingletonCache();
        }

        private SingletonCache()
        { 
        }

        public static SingletonCache Instance()
        {
            return instance;
        }

        public object GetSingleton(Type type, params object[] parameters)
        {
            object obj = null;

            try
            {
                if (objectPool.Contains(type.Name) == false)
                {
                    obj = Activator.CreateInstance(type, parameters);
                    objectPool.Add(type.Name, obj);
                }
                else
                {
                    obj = objectPool[type.Name];
                }
            }
            catch
            {
                // log it maybe
            }

            return obj;
        }
    }
}
