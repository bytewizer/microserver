using System;
using System.Collections;
using System.Reflection;

namespace Bytewizer.TinyCLR.Terminal
{
    public class EndpointProvider
    {
        private readonly Hashtable _endpoints = new Hashtable();
        private readonly Hashtable _commands = new Hashtable();

        private readonly CommandDelegateFactory _commandFactory;

        public EndpointProvider(Assembly[] assemblies)
        {
            _commandFactory = new CommandDelegateFactory();

            if (assemblies == null)
            {
                return;
            }

            foreach (var assembly in assemblies)
            {
                GetAssemblies(assembly);
            }

        }

        public Hashtable GetEndpoints()
        {
            return _endpoints;
        }

        public Hashtable GetCommands()
        {
            return _commands;
        }

        private void GetAssemblies(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface || type.IsNotPublic)
                    continue;

                if (type.IsSubclassOf(typeof(ServerCommand)))
                {
                    MapCommandActions(type);
                }
            }
        }

        public void MapCommandActions(Type type)
        {
            var command = type.Name.Replace("Command", string.Empty).ToLower();
            var instance = (ServerCommand)Activator.CreateInstance(type);

            if (!string.IsNullOrEmpty(instance?.Description))
            {
                _commands.Add(command, instance.Description);
            }

            foreach (MethodInfo method in type.GetMethods())
            {
                if (type.IsAbstract || type.IsNotPublic)
                    continue;

                if (method.ReturnType.Equals(typeof(IActionResult)))
                {
                    var action = method.Name.ToLower();
                    var pattern = $"{command}/{action}";

                    var descriptor = new CommandActionDescriptor()
                    {
                        MethodInfo = method,
                        CommandName = command,
                        ActionName = action,
                    };

                    var parameters = method.GetParameters();
                    object[] actionParameters = new object[parameters.Length];               
                    
                    foreach (ParameterInfo item in parameters)
                    {
                        if (ModelMapper.CanBind(item.ParameterType))
                        {
                            actionParameters[item.Position] = item.ParameterType;
                        }
                        else
                        {
                            throw new Exception($"Invalid {pattern} {item.ParameterType.Name}");
                        }
                    }
                    
                    var requestDelegate = _commandFactory.CreateRequestDelegate(instance, actionParameters, descriptor);
                    var endpoint = new Endpoint(requestDelegate, pattern, instance, null, command);

                    _endpoints[pattern] = endpoint;
                }
            }
        }
    }
}