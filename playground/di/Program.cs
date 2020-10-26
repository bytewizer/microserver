using System;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.DependencyInjection;
using System.Reflection;
using System.Collections;
using Bytewizer.TinyCLR.Logging.Debug;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.DI
{
    class Program
    {     
        static void Main()
        {

            //IoC.Register(typeof(ILoggerFactory), typeof(LoggerFactory));
            //ILoggerFactory service = (ILoggerFactory)IoC.Resolve(typeof(ILoggerFactory));

            // Type classType = Type.GetType(typeof(LoggerFactory).FullName);
            //MethodInfo tom = classType.GetMethod(".ctor", BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //LoggerFactory classInstance = (LoggerFactory)Activator.CreateInstance(classType, new object[] { });
            //var ctor = typeof(LoggerFactory).GetConstructor(new Type[] { });
            //var currentCount = ctor.GetParameters();
            //var serivce = ctor.Invoke(new object[] { });

            //Type classType = Type.GetType(typeof(LoggerFactory).FullName);
            //var loggerFactory = (LoggerFactory)Activator.CreateInstance(classType);

            //var loggerFactory = (LoggerFactory)Activator.CreateInstance(typeof(LoggerFactory));
            //loggerFactory.AddDebug();
            //var logger = loggerFactory.CreateLogger(typeof(Program));
            //logger.LogInformation("Starting application");


            var serviceProvider = new ServiceCollection()
                .AddSingleton(typeof(ILoggerFactory), typeof(LoggerFactory))
                .AddSingleton(typeof(IFooService), typeof(FooService))
                .AddSingleton(typeof(IBarService), typeof(BarService))
                .BuildServiceProvider();

            var loggerFactory = (LoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
            loggerFactory.AddDebug();

            var logger = loggerFactory.CreateLogger(typeof(Program));
            logger.LogInformation("Starting application");

            //do the actual work here
            var bar = (BarService)serviceProvider.GetService(typeof(IBarService));
            bar.DoSomeRealWork();

            logger.LogInformation("All done!");

            //var tom = GetConstructors(typeof(BarService));

            //InDebug();
        }

        private static void InDebug()
        {
            Type pinEnumType = typeof(BarService);

            BindingFlags bindingFlags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo[] constructors = pinEnumType.GetMethods(bindingFlags);
            Debug.WriteLine("Number of methods: " + constructors.Length);

            foreach (var constructor in constructors)
            {
                Debug.WriteLine("Constructor: " + constructor.Name);

                ParameterInfo[] parameters = constructor.GetParameters();
                foreach (ParameterInfo parameter in parameters)
                {
                    Debug.WriteLine($"Constructor Parameter: {parameter.Position}::{parameter.ParameterType.Name}");  
                }
            }

            FieldInfo[] fields = pinEnumType.GetFields();
            Debug.WriteLine("Number of fields: " + fields.Length);

            foreach (FieldInfo field in fields)
            {
                Debug.WriteLine("Field: " + field.Name + " = "); //+ field.GetValue(new BarService));
            }

            MethodInfo[] methods = pinEnumType.GetMethods();
            Debug.WriteLine("Number of methods: " + methods.Length);

            foreach (var method in methods)
            {
                Debug.WriteLine("Method: " + method.Name);
            }
        }
    }

    static class IoC
    {
        static readonly Hashtable types = new Hashtable();

        public static void Register(Type contract, Type implementation)
        {
            types[contract] = implementation;
        }

        public static object Resolve(Type contract)
        {
            Type implementation = (Type)types[contract];
            ConstructorInfo constructor = implementation.GetConstructor(new Type[] { });
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementation);
            }

            ArrayList parameters = new ArrayList();
            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                parameters.Add(Resolve(parameterInfo.ParameterType));
            }

            return constructor.Invoke(parameters.ToArray());
        }
    }


    public interface IFooService
    {
        void DoThing(int number);
    }

    public interface IBarService
    {
        void DoSomeRealWork();
    }

    public class BarService : IBarService
    {
        private readonly IFooService _fooService;
        
        public BarService(IFooService fooService)
        {
            _fooService = fooService;
        }

        public void DoSomeRealWork()
        {
            for (int i = 0; i < 10; i++)
            {
                _fooService.DoThing(i);
            }
        }
    }

    public class FooService : IFooService
    {
        private readonly ILogger _logger;
        
        public FooService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(FooService));
        }

        public void DoThing(int number)
        {
            _logger.LogInformation($"Doing the thing {number}");
        }
    }
}