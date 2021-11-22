using System;
using System.Reflection;
using System.Collections;
using Bytewizer.TinyCLR.Http.Routing;

namespace Bytewizer.TinyCLR.Http.WebSockets.Middleware
{
    internal class HubEndpointProvider
    {
        private readonly Hashtable _endpoints;
        private readonly HubDelegateFactory _controllerFactory;

        public HubEndpointProvider()
        {
            _controllerFactory = new HubDelegateFactory();
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

                if (type.IsSubclassOf(typeof(Hub)))
                {
                    var hub = type.Name.Replace("Hub", string.Empty).ToLower();
                    var requestDelegate = _controllerFactory.CreateRequestDelegate(type);
                    var endpoint = new RouteEndpoint(requestDelegate, hub, null, $"/{hub}");

                    _endpoints.Add(hub, endpoint);
                }
            }
        }
    }
}
