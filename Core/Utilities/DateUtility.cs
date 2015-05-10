using System;
namespace MicroServer.Utilities
{

    //All HTTP date/time stamps MUST be represented in Greenwich Mean Time (GMT), without exception. For the purposes 
    //of HTTP, GMT is exactly equal to UTC (Coordinated Universal Time). This is indicated in the first two formats by
    //the inclusion of "GMT" as the three-letter abbreviation for time zone, and MUST be assumed when reading the asctime
    //format. HTTP-date is case sensitive and MUST NOT include additional LWS beyond that specifically included as SP in the grammar.

    //   HTTP-date    = rfc1123-date | rfc850-date | asctime-date
    //   rfc1123-date = wkday "," SP date1 SP time SP "GMT"
    //   rfc850-date  = weekday "," SP date2 SP time SP "GMT"
    //   asctime-date = wkday SP date3 SP time SP 4DIGIT
    //
    //   date1        = 2DIGIT SP month SP 4DIGIT
    //                  ; day month year (e.g., 02 Jun 1982)
    //   date2        = 2DIGIT "-" month "-" 2DIGIT
    //                  ; day-month-year (e.g., 02-Jun-82)
    //   date3        = month SP ( 2DIGIT | ( SP 1DIGIT ))
    //                  ; month day (e.g., Jun  2)
    //   time         = 2DIGIT ":" 2DIGIT ":" 2DIGIT
    //                  ; 00:00:00 - 23:59:59
    //   wkday        = "Mon" | "Tue" | "Wed"
    //                | "Thu" | "Fri" | "Sat" | "Sun"
    //   weekday      = "Monday" | "Tuesday" | "Wednesday"
    //                | "Thursday" | "Friday" | "Saturday" | "Sunday"
    //   month        = "Jan" | "Feb" | "Mar" | "Apr"
    //                | "May" | "Jun" | "Jul" | "Aug"
    //                | "Sep" | "Oct" | "Nov" | "Dec"

    
    /// <summary>
    /// Provides additional datetime operations
    /// </summary>
    public static class DateUtility
    {
        /// <summary>
        /// Attempt to parse the provided datetime value using RFC1123, RFC1036, or  ANSI C asctime() format.
        /// </summary>
        /// <param name="datetime">Datetime value to be parsed</param>
        /// <returns>Datetime if parsing was successful or <see cref="DateTime.MinValue"/> if parse fails</returns>
        public static DateTime Parse(string datetime)
        {
            string[] vals = Tokenize(datetime);
            string[] secs = vals[4].Split(':');

            // Long
            switch (vals[0].ToLower())
            {
                case "mon,":
                case "tue,":
                case "wed,":
                case "thu,":
                case "fri,":
                case "sat,":
                case "sun,":
                case "mon":
                case "tue":
                case "wed":
                case "thu":
                case "fri":
                case "sat":
                case "sun":
                    return new DateTime(int.Parse(vals[3]),
                                        MonthFromString(vals[2]), 
                                        int.Parse(vals[1]), 
                                        int.Parse(secs[0]), 
                                        int.Parse(secs[1]),
                                        int.Parse(secs[2]));
            }

            // Short
            if (datetime.IndexOf("/") > 0)
            {
                vals = datetime.Split('/');
                return new DateTime(int.Parse(vals[2]),
                                    int.Parse(vals[0]),
                                    int.Parse(vals[1]),
                                    int.Parse(secs[0]), 
                                    int.Parse(secs[1]),
                                    int.Parse(secs[2]));
            }

            return DateTime.MinValue;
        }

        private static int MonthFromString(string month)
        {
            switch (month.ToLower())
            {
                case "jan":
                case "january":
                    return 1;
                case "feb":
                case "february":
                    return 2;
                case "mar":
                case "march":
                    return 3;
                case "apr":
                case "april":
                    return 4;
                case "may":
                    return 5;
                case "jun":
                case "june":
                    return 6;
                case "jul":
                case "july":
                    return 7;
                case "aug":
                case "august":
                    return 8;
                case "sep":
                case "september":
                    return 9;
                case "oct":
                case "october":
                    return 10;
                case "nov":
                case "november":
                    return 11;
                default:
                    return 12;
            }
        }

        private static string[] SplitComponents(string value, char deliminator)
        {
            int iStart = 0;
            string[] ret = null;
            string[] tmp;
            int i;
            string s;

            while (true)
            {
                // Find deliminator
                i = value.IndexOf(deliminator, iStart);

                if (InQuotes(value, i))
                    iStart = i + 1;
                else
                {
                    // Separate value
                    if (i < 0)
                        s = value;
                    else
                    {
                        s = value.Substring(0, i).Trim();
                        value = value.Substring(i + 1);
                    }

                    // Add value
                    if (ret == null)
                        ret = new string[] { s };
                    else
                    {
                        tmp = new string[ret.Length + 1];
                        Array.Copy(ret, tmp, ret.Length);
                        tmp[tmp.Length - 1] = s;
                        ret = tmp;
                    }

                    iStart = 0;
                }

                // Break on last value
                if (i < 0 || value == string.Empty)
                    break;
            }

            return ret;
        }

        private static string[] Tokenize(string command)
        {
            string[] res = SplitComponents(command, ' ');
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = res[i].Trim();
                if (res[i].Substring(0, 1) == "\"" && res[i].Substring(res[i].Length - 1) == "\"")
                    res[i] = res[i].Substring(1, res[i].Length - 2);
            }
            return res;
        }

        private static bool InQuotes(string value, int position)
        {
            int qcount = 0;
            int i;
            int iStart = 0;

            while (true)
            {
                // Find next instance of a quote
                i = value.IndexOf('"', iStart);

                // If not return our value
                if (i < 0 || i >= position)
                    return qcount % 2 != 0;

                // Check if it's a qualified quote
                if (i > 0 && value.Substring(i, 1) != "\\" || i == 0)
                    qcount++;

                iStart = i + 1;
            }
        }
    }
}
