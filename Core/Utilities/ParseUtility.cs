using System;
using Microsoft.SPOT;

using MicroServer.Extensions;

namespace MicroServer.Utilities
{
    /// <summary>
    /// Provides additional parsing operations
    /// </summary>
    public abstract class ParseUtility
    {
        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="i">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseInt(string value, out int i)
        {
            i = 0;
            try
            {
                i = int.Parse(value);
                return true;
            }
            catch 
            {
                return false;
            }    
        }

        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="i">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseShort(string value, out short i)
        {
            i = 0;
            try
            {
                i = short.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="i">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseLong(string value, out long i)
        {
            i = 0;
            try
            {
                i = long.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="i">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseDouble(string value, out double i)
        {
            i = 0;
            try
            {
                i = double.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="val">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseBool(string value, out bool val)
        {
            val = false;
            try
            {
                if (value == "1" || value.ToUpper() == bool.TrueString.ToUpper())
                {
                    val = true;

                    return true;
                }
                else if (value == "0" || value.ToUpper() == bool.FalseString.ToUpper())
                {
                    val = false;

                    return true;
                }

                return false;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="i">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseUInt(string value, out uint i)
        {
            i = 0;
            try
            {
                i = uint.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="i">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseUShort(string value, out ushort i)
        {
            i = 0;
            try
            {
                i = ushort.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse the provided string value.
        /// </summary>
        /// <param name="value">String value to be parsed</param>
        /// <param name="i">Variable to set successfully parsed value to</param>
        /// <returns>True if parsing was successful</returns>
        public static bool TryParseULong(string value, out ulong i)
        {
            i = 0;
            try
            {
                i = ulong.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse the provided datetime value.
        /// </summary>
        /// <param name="datetime">Datetime value to be parsed</param>
        /// <returns>Datetime if parsing was successful</returns>
        public static DateTime TryParseDateTime(string datetime)
        {
            // Converts an ISO 8601 time/date format string, which is used by JSON and others,
            // into a DateTime object. 
            
            //Check to see if format contains the timezone ID, or contains UTC reference
            // Neither means it's localtime
            bool utc = datetime.EndsWith("Z");

            string[] parts = datetime.Split(new char[] { 'T', 'Z', ':', '-', '.', '+', });

            // We now have the time string to parse, and we'll adjust
            // to UTC or timezone after parsing
            string year = parts[0];
            string month = (parts.Length > 1) ? parts[1] : "1";
            string day = (parts.Length > 2) ? parts[2] : "1";
            string hour = (parts.Length > 3) ? parts[3] : "0";
            string minute = (parts.Length > 4) ? parts[4] : "0";
            string second = (parts.Length > 5) ? parts[5] : "0";
            string ms = (parts.Length > 6) ? parts[6] : "0";

            DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day), Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(second), Convert.ToInt32(ms));

            // If a time offset was specified instead of the UTC marker,
            // add/subtract in the hours/minutes
            if ((utc == false) && (parts.Length >= 9))
            {
                // There better be a timezone offset
                string hourOffset = (parts.Length > 7) ? parts[7] : "";
                string minuteOffset = (parts.Length > 8) ? parts[8] : "";
                if (datetime.Contains("+"))
                {
                    dt = dt.AddHours(Convert.ToDouble(hourOffset));
                    dt = dt.AddMinutes(Convert.ToDouble(minuteOffset));
                }
                else
                {
                    dt = dt.AddHours(-(Convert.ToDouble(hourOffset)));
                    dt = dt.AddMinutes(-(Convert.ToDouble(minuteOffset)));
                }
            }

            if (utc)
            {
                // Convert the Kind to DateTimeKind.Utc if string Z present
                dt = new DateTime(0, DateTimeKind.Utc).AddTicks(dt.Ticks);
            }

            return dt;

            //string[] dt = datetime.Split(' ');
            //string[] date = dt[0].Split('/');
            //string[] time = dt[1].Split(':');
            //return new DateTime(int.Parse(date[2]), int.Parse(date[0]), int.Parse(date[1]), int.Parse(time[0]), int.Parse(time[1]), int.Parse (time[2]));
        }

        #region TryParseHex Method

        private static string hexValues = "0123456789ABCDEF";

        /// <summary>
        /// Converts a hex string to a byte value
        /// </summary>
        /// <param name="value">Byte value as output parameter</param>
        /// <returns>True if successful</returns>
        public static bool TryParseHex(string source, out byte value)
        {
            value = 0;

            if (source == null)
                return false;

            source = source.Trim();
            if (source.Length != 2)
                return false;

            source = source.ToUpper();
            int value0 = hexValues.IndexOf(source[0]);
            int value1 = hexValues.IndexOf(source[1]);
            if (value0 < 0 || value1 < 0)
                return false;

            value = (byte)((value0 << 4) + value1);

            return true;
        }

        /// <summary>
        /// Converts a hex string to an Int16 value
        /// </summary>
        /// <param name="value">Int16 value as output parameter</param>
        /// <returns>True if successful</returns>
        public static bool TryParseHex(string source, out Int16 value)
        {
            value = 0;

            if (source == null)
                return false;

            source = source.Trim();
            if (source.Length != 4)
                return false;

            source = source.ToUpper();
            int value0 = hexValues.IndexOf(source[0]);
            int value1 = hexValues.IndexOf(source[1]);
            int value2 = hexValues.IndexOf(source[2]);
            int value3 = hexValues.IndexOf(source[3]);
            if (value0 < 0 || value1 < 0 || value2 < 0 || value3 < 0)
                return false;

            value = (Int16)((value0 << 12) + (value1 << 8) + (value2 << 4) + value3);

            return true;
        }

        /// <summary>
        /// Converts a hex string to an Int32 value
        /// </summary>
        /// <param name="value">Int32 value as output parameter</param>
        /// <returns>True if successful</returns>
        public static bool TryParseHex(string source, out Int32 value)
        {
            value = 0;

            if (source == null)
                return false;

            source = source.Trim();
            if (source.Length != 8)
                return false;

            source = source.ToUpper();
            int value0 = hexValues.IndexOf(source[0]);
            int value1 = hexValues.IndexOf(source[1]);
            int value2 = hexValues.IndexOf(source[2]);
            int value3 = hexValues.IndexOf(source[3]);
            int value4 = hexValues.IndexOf(source[4]);
            int value5 = hexValues.IndexOf(source[5]);
            int value6 = hexValues.IndexOf(source[6]);
            int value7 = hexValues.IndexOf(source[7]);
            if (value0 < 0 || value1 < 0 || value2 < 0 || value3 < 0 || value4 < 0 || value5 < 0 || value6 < 0 || value7 < 0)
                return false;

            value = (Int32)((value0 << 28) + (value1 << 24) + (value2 << 20) + (value3 << 16) + (value4 << 12) + (value5 << 8) + (value6 << 4) + value7);

            return true;
        }

        /// <summary>
        /// Converts a hex string to an UInt16 value
        /// </summary>
        /// <param name="value">UInt16 value as output parameter</param>
        /// <returns>True if successful</returns>
        public static bool TryParseHex(string source, out UInt16 value)
        {
            value = 0;

            if (source == null)
                return false;

            source = source.Trim();
            if (source.Length != 4)
                return false;

            source = source.ToUpper();
            int value0 = hexValues.IndexOf(source[0]);
            int value1 = hexValues.IndexOf(source[1]);
            int value2 = hexValues.IndexOf(source[2]);
            int value3 = hexValues.IndexOf(source[3]);
            if (value0 < 0 || value1 < 0 || value2 < 0 || value3 < 0)
                return false;

            value = (UInt16)((value0 << 12) + (value1 << 8) + (value2 << 4) + value3);

            return true;
        }

        /// <summary>
        /// Converts a hex string to an UInt32 value
        /// </summary>
        /// <param name="value">UInt32 value as output parameter</param>
        /// <returns>True if successful</returns>
        public static bool TryParseHex(string source, out UInt32 value)
        {
            value = 0;

            if (source == null)
                return false;

            source = source.Trim();
            if (source.Length != 8)
                return false;

            source = source.ToUpper();
            int value0 = hexValues.IndexOf(source[0]);
            int value1 = hexValues.IndexOf(source[1]);
            int value2 = hexValues.IndexOf(source[2]);
            int value3 = hexValues.IndexOf(source[3]);
            int value4 = hexValues.IndexOf(source[4]);
            int value5 = hexValues.IndexOf(source[5]);
            int value6 = hexValues.IndexOf(source[6]);
            int value7 = hexValues.IndexOf(source[7]);
            if (value0 < 0 || value1 < 0 || value2 < 0 || value3 < 0 || value4 < 0 || value5 < 0 || value6 < 0 || value7 < 0)
                return false;

            value = (UInt32)((value0 << 28) + (value1 << 24) + (value2 << 20) + (value3 << 16) + (value4 << 12) + (value5 << 8) + (value6 << 4) + value7);

            return true;
        }

        #endregion TryParseHex Functions
    }
}
