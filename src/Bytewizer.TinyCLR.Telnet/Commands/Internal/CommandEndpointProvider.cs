using System;
using System.Reflection;
using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{
    public class CommandEndpointProvider
    {
        private readonly Hashtable _endpoints;
        private readonly Hashtable _commands;
        private readonly CommandDelegateFactory _commandFactory;

        public CommandEndpointProvider(Assembly[] assemblies)
        {
            _commandFactory = new CommandDelegateFactory();
            _endpoints = new Hashtable();
            _commands = new Hashtable();
            
            GetAssemblies(assemblies);
        }

        public Hashtable GetEndpoints()
        {
            return _endpoints;
        }

        public Hashtable GetCommands()
        {
            return _commands;
        }

        private void GetAssemblies(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                GetAssemblies(assembly);
            }
        }

        private void GetAssemblies(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface || type.IsNotPublic)
                    continue;

                if (type.IsSubclassOf(typeof(Command)))
                {
                    MapCommandActions(type);
                }
            }
        }

        private void MapCommandActions(Type type)
        {
            var command = type.Name.Replace("Command", string.Empty).ToLower();
            //var instance = (Command)Activator.CreateInstance(type);
            //var description = instance?.Description ?? string.Empty;

            _commands.Add(command, string.Empty);

            foreach (MethodInfo method in type.GetMethods())
            {
                if (type.IsAbstract || type.IsNotPublic)
                    continue;

                if (method.ReturnType.Equals(typeof(IActionResult)))
                {
                    var action = method.Name.ToLower();
                    var route = $"{command}/{action}";

                    var requestDelegate = _commandFactory.CreateRequestDelegate(type, method);
                    var endpoint = new RouteEndpoint(requestDelegate, route, null, command);

                    _endpoints.Add(route, endpoint);
                }
            }
        }
    }
}
