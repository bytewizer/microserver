using System;

using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.TinyCLR.Logging
{
    class Program
    {
        private static ILoggerFactory loggerFactory;
        
        static void Main()
        {
            //var filter = new LoggerFilterOptions()
            //{
            //    MinLevel = LogLevel.Trace
            //};
            //loggerFactory = new LoggerFactory(filter);

            loggerFactory = new LoggerFactory();
            //loggerFactory.AddDebug();

            TestLoggerExtensions();
            //TestLogger();
        }

        private static void TestLogger()
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(TestLogger));

            var id = new EventId(10, "event id name");
            logger.Log(LogLevel.Information, id, "event id message");

            try 
            {
                throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, id, ex, "message");
            }         
        }

        private static void TestLoggerExtensions()
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(TestLoggerExtensions));

            logger.LogTrace("Trace");
            logger.LogDebug("Debug");
            logger.LogInformation("Information");
            logger.LogWarning("Warning");
            logger.LogError("Error");
            logger.LogCritical("Critical");
        }
    }
}