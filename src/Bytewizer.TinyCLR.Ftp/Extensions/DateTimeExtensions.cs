using System;

namespace Bytewizer.TinyCLR.Ftp
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class DateTimeExtensions
    {
        public static string ToTimeString( this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyyMMddHHmmss.fff");
        }
    }
}
