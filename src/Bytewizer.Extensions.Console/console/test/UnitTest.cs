using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bytewizer.Extensions.Console.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void CreateConsoleBuilder()
        {
            string[] args = null;
            CreateConsoleBuilder(args).Build().RunAsync();
        }

        public static IApplicationBuilder CreateConsoleBuilder(string[] args) =>
            ConsoleApplication.CreateDefaultBuilder(args)
                .UseStartup<Application>()
                .ConfigureServices((context, services) =>
                {

                });
    }
}
