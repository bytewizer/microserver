using System;
using System.Reflection;
using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{
    internal class CommandEndpointProvider
    {
        private readonly Hashtable _endpoints;
        private readonly CommandDelegateFactory _commandFactory;

        public CommandEndpointProvider()
        {
            _commandFactory = new CommandDelegateFactory();
            _endpoints = new Hashtable();
            
            GetAssemblies();
        }

        public Hashtable GetEndpoints()
        {
            return _endpoints;
        }

        public bool TryGetEndpoint(string pattern, out RouteEndpoint endpoint)
        {
            endpoint = default;

            if (_endpoints == null)
            {
                return false;
            }

            if (_endpoints.Contains(pattern))
            {
                endpoint = (RouteEndpoint)_endpoints[pattern];
                return true;
            }

            return false;
        }

        private void GetAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
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
            foreach (MethodInfo method in type.GetMethods())
            {
                if (type.IsAbstract || type.IsNotPublic)
                    continue;

                if (method.ReturnType.Equals(typeof(IActionResult)))
                {
                    var controller = type.Name.Replace("Command", string.Empty).ToLower();
                    var action = method.Name.ToLower();
                    var uri = $"{controller}/{action}";

                    var requestDelegate = _commandFactory.CreateRequestDelegate(type, method);
                    var endpoint = new RouteEndpoint(requestDelegate, uri, null, $"/{uri}");

                    _endpoints.Add(uri, endpoint);
                }
            }
        }
    }
}
