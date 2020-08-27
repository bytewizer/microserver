namespace Bytewizer.TinyCLR.Hosting
{
    public static class Host
    {

        public static IHostBuilder CreateDefaultBuilder()
        {
            var builder = new HostBuilder();

            //TODO: Add .UseContentRoot()
            //TODO: Add .Logging()

            return builder;
        }
    }
}