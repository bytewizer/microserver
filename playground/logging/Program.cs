using System;
using System.Collections;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.Playground.Logging
{
    class Program
    {
        private static ILoggerFactory loggerFactory;
        
        static void Main()
        {
            var filter = new LoggerFilterOptions()
            {
                MinLevel = LogLevel.Information
            };

            var provider = new ArrayList
            {
                new DebugLoggerProvider(LogLevel.Information)
            };

            loggerFactory = new LoggerFactory(provider, filter);

            TestLoggerExtensions();
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


//private static void TestLogger()
//{
//    ILogger logger = loggerFactory.CreateLogger(nameof(TestLogger));

//    var id = new EventId(10, "event id name");
//    logger.Log(LogLevel.Information, id, "event id message");

//    try 
//    {
//        throw new NullReferenceException();
//    }
//    catch (Exception ex)
//    {
//        logger.Log(LogLevel.Error, id, ex, "message");
//    }         
//}