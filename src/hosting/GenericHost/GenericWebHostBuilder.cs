using System;

namespace Bytewizer.TinyCLR.Hosting
{
    internal class GenericWebHostBuilder : IWebHostBuilder
    {
        private readonly IHostBuilder _builder;
        private object _startupObject;

        public GenericWebHostBuilder(IHostBuilder builder)
        {
            _builder = builder;
        }

        public IWebHostBuilder UseStartup(Type startupType)
        {
            // UseStartup can be called multiple times. Only run the last one.
            _startupObject = startupType;

            if (ReferenceEquals(_startupObject, startupType))
            {
                UseStartup(startupType);
            }

            return this;
        }

        public IWebHost Build()
        {
            throw new NotImplementedException();
        }
    }
}
