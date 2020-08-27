namespace Bytewizer.TinyCLR.Hosting
{
    class HostBuilder : IHostBuilder
    {       
        public IHost Build()
        {
            return new Internal.Host();
        }
    }
}
