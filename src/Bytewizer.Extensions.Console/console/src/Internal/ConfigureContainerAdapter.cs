using System;

namespace Bytewizer.Extensions.Console.Internal
{
    internal class ConfigureContainerAdapter<TContainerBuilder> : IConfigureContainerAdapter
    {
        private Action<ApplicationBuilderContext, TContainerBuilder> _action;

        public ConfigureContainerAdapter(Action<ApplicationBuilderContext, TContainerBuilder> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(ApplicationBuilderContext hostContext, object containerBuilder)
        {
            _action(hostContext, (TContainerBuilder)containerBuilder);
        }
    }
}