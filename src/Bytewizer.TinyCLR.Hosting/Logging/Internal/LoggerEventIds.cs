using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Hosting
{
    internal static class LoggerEventIds
    {
        public static readonly EventId Starting = new EventId(1, "Starting");
        public static readonly EventId Started = new EventId(2, "Started");
        public static readonly EventId Stopping = new EventId(3, "Stopping");
        public static readonly EventId Stopped = new EventId(4, "Stopped");
        public static readonly EventId StoppedWithException = new EventId(5, "StoppedWithException");
        public static readonly EventId ApplicationStartupException = new EventId(6, "ApplicationStartupException");
        public static readonly EventId ApplicationStoppingException = new EventId(7, "ApplicationStoppingException");
        public static readonly EventId ApplicationStoppedException = new EventId(8, "ApplicationStoppedException");
        public static readonly EventId BackgroundServiceFaulted = new EventId(9, "BackgroundServiceFaulted");
    }
}
