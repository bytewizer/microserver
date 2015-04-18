using System;
using System.Text;
using System.Collections;

namespace MicroServer.Utilities
{
    /// <summary>
    /// Provides additional string operations
    /// </summary>
    public static class StringUtility
    {          
        /// <summary>
        /// Check if the provided string is either null or empty
        /// </summary>
        /// <param name="source">String to validate</param>
        /// <returns>True if the string is null or empty</returns>
        public static bool IsNullOrEmpty(string source)
        {
            if (source == null || source == string.Empty)
                return true;

            return false;
        }

        /// <summary>
        /// Check if the provided string is either null or white space
        /// </summary>
        /// <param name="source">String to validate</param>
        /// <returns>True if the string is null or white space</returns>
        public static bool IsNullOrWhiteSpace(string source)
        {
            if (source == null || source == string.Empty)
                return true;

            return IsNullOrEmpty(source) || source.Trim().Length == 0;
        }

        /// <summary>
        /// Split a string by deliminator
        /// </summary>
        /// <param name="source"></param>
        /// <param name="deliminator"></param>
        /// <returns></returns>
        public static string[] SplitComponents(string source, char deliminator)
        {
            int iStart = 0;
            string[] ret = null;
            string[] tmp;
            int i;
            string s;

            while (true)
            {
                // Find deliminator
                i = source.IndexOf(deliminator, iStart);

                if (InQuotes(source, i))
                    iStart = i + 1;
                else
                {
                    // Separate value
                    if (i < 0)
                        s = source;
                    else
                    {
                        s = source.Substring(0, i).Trim();
                        source = source.Substring(i + 1);
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
                if (i < 0 || source == string.Empty)
                    break;
            }

            return ret;
        }

        /// <summary>
        /// Determine if a specific character is inside of a quote string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool InQuotes(string source, int position)
        {
            int qcount = 0;
            int i;
            int iStart = 0;

            while (true)
            {
                // Find next instance of a quote
                i = source.IndexOf('"', iStart);

                // If not return our value
                if (i < 0 || i >= position)
                    return qcount % 2 != 0;

                // Check if it's a qualified quote
                if (i > 0 && source.Substring(i, 1) != "\\" || i == 0)
                    qcount++;

                iStart = i + 1;
            }
        }

        /// <summary>
        /// Determine if a string includes a pattern using "*" and "?" as wild cards 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="caseSensitive"></param>
        /// <returns>True if pattern wild card matches</returns>
        public static bool MatchWildCard(string source, string pattern, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                pattern = pattern.ToLower();
                source = source.ToLower();
            }

            int nText = 0;
            int nPattern = 0;
            //const char *cp = NULL, *mp = NULL;
            int mp = 0;
            int cp = 0;

            while (nText < source.Length && nPattern < pattern.Length && (pattern[nPattern] != '*'))
            {
                if ((pattern[nPattern] != source[nText]) && (pattern[nPattern] != '?'))
                {
                    return false;
                }
                nPattern++;
                nText++;
            }

            while (nText < source.Length)
            {
                if (pattern[nPattern] == '*')
                {
                    nPattern++;
                    if (nPattern >= pattern.Length)
                    {
                        return true;
                    }
                    mp = nPattern;
                    cp = nText + 1;
                }
                else if ((pattern[nPattern] == source[nText]) || (pattern[nPattern] == '?'))
                {
                    nPattern++;
                    nText++;
                }
                else
                {
                    nPattern = mp;
                    nText = cp++;
                }
            }

            while (nPattern < pattern.Length && pattern[nPattern] == '*')
            {
                nPattern++;
            }
            return nPattern >= pattern.Length;
        }

        /// <summary>
        /// Encodes a string according to the BASE64 standard
        /// </summary>
        /// <param name="value">The input string</param>
        /// <returns>The output string</returns>
        public static string Base64Encode(string value)
        {
            // Pairs of 3 8-bit bytes will become pairs of 4 6-bit bytes
            // That's the whole trick of base64 encoding :-)

            int Blocks = value.Length / 3;           // The amount of original pairs
            if (Blocks * 3 < value.Length) ++Blocks; // Fixes rounding issues; always round up
            int Bytes = Blocks * 4;                  // The length of the base64 output

            // These characters will be used to represent the 6-bit bytes in ASCII
            char[] Base64_Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=".ToCharArray();

            // Converts the input string to characters and creates the output array
            char[] InputChars = value.ToCharArray();
            char[] OutputChars = new char[Bytes];

            // Converts the blocks of bytes
            for (int Block = 0; Block < Blocks; ++Block)
            {
                // Fetches the input pairs
                byte Input0 = (byte)(InputChars.Length > Block * 3 ? InputChars[Block * 3] : 0);
                byte Input1 = (byte)(InputChars.Length > Block * 3 + 1 ? InputChars[Block * 3 + 1] : 0);
                byte Input2 = (byte)(InputChars.Length > Block * 3 + 2 ? InputChars[Block * 3 + 2] : 0);

                // Generates the output pairs
                byte Output0 = (byte)(Input0 >> 2);                           // The first 6 bits of the 1st byte
                byte Output1 = (byte)(((Input0 & 0x3) << 4) + (Input1 >> 4)); // The last 2 bits of the 1st byte followed by the first 4 bits of the 2nd byte
                byte Output2 = (byte)(((Input1 & 0xf) << 2) + (Input2 >> 6)); // The last 4 bits of the 2nd byte followed by the first 2 bits of the 3rd byte
                byte Output3 = (byte)(Input2 & 0x3f);                         // The last 6 bits of the 3rd byte

                // This prevents 0-bytes at the end
                if (InputChars.Length < Block * 3 + 2) Output2 = 64;
                if (InputChars.Length < Block * 3 + 3) Output3 = 64;

                // Converts the output pairs to base64 characters
                OutputChars[Block * 4] = Base64_Characters[Output0];
                OutputChars[Block * 4 + 1] = Base64_Characters[Output1];
                OutputChars[Block * 4 + 2] = Base64_Characters[Output2];
                OutputChars[Block * 4 + 3] = Base64_Characters[Output3];
            }

            return new string(OutputChars);
        }

        /// <summary>
        /// Return X.X Byte/KB/MB/GB/TB
        /// </summary>
        /// <param name="value">Size</param>
        /// <returns></returns>
        public static string FormatDiskSize(long value)
        {
            double cur = (double)value;
            string[] size = new string[] { "bytes", "kb", "mb", "gb", "tb" };
            int i = 0;

            while (cur > 1024 && i < 4)
            {
                cur /= 1024;
                i++;
            }

            return System.Math.Round(cur) + " " + size[i];
        }

        #region ZeroFill Method

        /// <summary>
        /// Changes a number into a string and add zeros in front of it, if required
        /// </summary>
        /// <param name="number">The input number</param>
        /// <param name="digits">The amount of digits it should be</param>
        /// <param name="character">The character to repeat in front (default: 0)</param>
        /// <returns>A string with the right amount of digits</returns>
        public static string ZeroFill(string number, int digits, char character = '0')
        {
            bool Negative = false;
            if (number.Substring(0, 1) == "-")
            {
                Negative = true;
                number = number.Substring(1);
            }

            for (int Counter = number.Length; Counter < digits; ++Counter)
            {
                number = character + number;
            }
            if (Negative) number = "-" + number;
            return number;
        }

        /// <summary>
        /// Changes a number into a string and add zeros in front of it, if required
        /// </summary>
        /// <param name="Number">The input number</param>
        /// <param name="MinLength">The amount of digits it should be</param>
        /// <param name="Character">The character to repeat in front (default: 0)</param>
        /// <returns>A string with the right amount of digits</returns>
        public static string ZeroFill(int Number, int MinLength, char Character = '0')
        {
            return ZeroFill(Number.ToString(), MinLength, Character);
        }

        #endregion ZeroFill Method

        #region Replace Method

        /// <summary>
        /// Replace all occurances of the 'find' string with the 'replace' string.
        /// </summary>
        /// <param name="source">Original string</param>
        /// <param name="find">String to find within the original string</param>
        /// <param name="replace">String to be used in place of the find string</param>
        /// <returns>Final string after all instances have been replaced.</returns>
        public static string Replace(string source, string find, string replace)
        {
            int i;
            int iStart = 0;

            if (source == string.Empty || source == null || find == string.Empty || find == null)
                return source;

            while (true)
            {
                i = source.IndexOf(find, iStart);
                if (i < 0) break;

                if (i > 0)
                    source = source.Substring(0, i) + replace + source.Substring(i + find.Length);
                else
                    source = replace + source.Substring(i + find.Length);

                iStart = i + replace.Length;
            }
            return source;
        }

        /// <summary>
        /// Finds and replaces empty or null within a string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="replaceWith"></param>
        /// <returns>source</returns>
        public static string ReplaceEmptyOrNull(string source, string replaceWith)
        {
            if (source == string.Empty || source == null)
                return replaceWith;
            return source;
        }

        /// <summary>
        /// Finds and replaces empty or null within a string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="replaceWith"></param>
        /// <returns>Value</returns>
        public static string ReplaceEmptyOrNull(object source, string replaceWith)
        {
            if (source == null || source.ToString() == string.Empty)
                return replaceWith;
            return source.ToString();
        }

        #endregion Replace method

        #region Sort Method

        /// <summary>
        /// Sorts an array of strings.
        /// </summary>
        /// <remarks>
        /// Original code by user "Jay Jay"
        /// http://www.tinyclr.com/codeshare/entry/475
        /// Modified to be specifically suites to sorting arrays of strings.    
        /// </remarks>
        /// <param name="array">Array of string to be sorted.</param>
        public static void Sort(string[] array)
        {
            Sort(array, 0, array.Length - 1);
        }

        /// <summary>
        /// This is a generic version of C.A.R Hoare's Quick Sort 
        /// algorithm.  This will handle arrays that are already
        /// sorted, and arrays with duplicate keys.
        /// </summary>
        /// <remarks>
        /// If you think of a one dimensional array as going from
        /// the lowest index on the left to the highest index on the right
        /// then the parameters to this function are lowest index or
        /// left and highest index or right.  The first time you call
        /// this function it will be with the parameters 0, a.length - 1.
        /// </remarks>
        /// <param name="array">Array of string to be sorted.</param>
        /// <param name="l">Left boundary of array partition</param>
        /// <param name="r">Right boundary of array partition</param>
        private static void Sort(string[] array, int l, int r)
        {
            int M = 4;
            int i;
            int j;
            string v;

            if ((r - l) <= M)
            {
                InsertionSort(array, l, r);
            }
            else
            {
                i = (r + l) / 2;

                if (string.Compare(array[l], array[i]) > 0)
                    Swap(array, l, i);

                if (string.Compare(array[l], array[r]) > 0)
                    Swap(array, l, r);

                if (string.Compare(array[i], array[r]) > 0)
                    Swap(array, i, r);

                j = r - 1;
                Swap(array, i, j);

                i = l;
                v = array[j];
                for (; ; )
                {
                    while (string.Compare(array[++i], v) < 0)
                    { }

                    while (string.Compare(array[--j], v) > 0)
                    { }

                    if (j < i)
                        break;
                    Swap(array, i, j);

                }
                Swap(array, i, r - 1);

                Sort(array, l, j);
                Sort(array, i + 1, r);
            }
        }

        private static void InsertionSort(string[] array, int lo, int hi)
        {
            int i;
            int j;
            string v;

            for (i = lo + 1; i <= hi; i++)
            {
                v = array[i];
                j = i;
                while ((j > lo) && (string.Compare(array[j - 1], v) > 0))
                {

                    array[j] = array[j - 1];
                    --j;
                }
                array[j] = v;
            }
        }

        private static void Swap(IList list, int left, int right)
        {
            object swap = list[left];
            list[left] = list[right];
            list[right] = swap;
        }

        #endregion Sort Method

        #region Format Method

        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">The object to format.</param>
        /// <returns>A copy of format in which any format items are replaced by the string representation of arg0.</returns>
        /// <exception cref="MicroServer.Utilities.FormatException">format is invalid, or the index of a format item is less than zero, or greater than or equal to the length of the args array.</exception>
        /// <exception cref="System.ArgumentNullException">format or args is null</exception>
        public static string Format(string format, object arg)
        {
            return Format(format, new object[] { arg });
        }

        /// <summary>
        /// Format the given string using the provided collection of objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        /// <exception cref="MicroServer.Utilities.FormatException">format is invalid, or the index of a format item is less than zero, or greater than or equal to the length of the args array.</exception>
        /// <exception cref="System.ArgumentNullException">format or args is null</exception>
        /// <example>
        /// x = StringUtility.Format("Quick brown {0}","fox");
        /// </example>
        public static string Format(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            if (args == null)
                throw new ArgumentNullException("args");

            // Validate the structure of the format string.
            ValidateFormatString(format);

            StringBuilder bld = new StringBuilder();

            int endOfLastMatch = 0;
            int starting = 0;

            while (starting >= 0)
            {
                starting = format.IndexOf('{', starting);

                if (starting >= 0)
                {
                    if (starting != format.Length - 1)
                    {
                        if (format[starting + 1] == '{')
                        {
                            // escaped starting bracket.
                            starting = starting + 2;
                            continue;
                        }
                        else
                        {
                            bool found = false;
                            int endsearch = format.IndexOf('}', starting);

                            while (endsearch > starting)
                            {
                                if (endsearch != (format.Length - 1) && format[endsearch + 1] == '}')
                                {
                                    // escaped ending bracket
                                    endsearch = endsearch + 2;
                                }
                                else
                                {
                                    if (starting != endOfLastMatch)
                                    {
                                        string t = format.Substring(endOfLastMatch, starting - endOfLastMatch);
                                        t = Replace(t, "{{", "{"); // get rid of the escaped brace
                                        t = Replace(t, "}}", "}"); // get rid of the escaped brace
                                        bld.Append(t);
                                    }

                                    // we have a winner
                                    string fmt = format.Substring(starting, endsearch - starting + 1);

                                    if (fmt.Length >= 3)
                                    {
                                        fmt = fmt.Substring(1, fmt.Length - 2);

                                        string[] indexFormat = fmt.Split(new char[] { ':' });

                                        string formatString = string.Empty;

                                        if (indexFormat.Length == 2)
                                        {
                                            formatString = indexFormat[1];
                                        }

                                        int index = 0;

                                        // no format, just number
                                        if (ParseUtility.TryParseInt(indexFormat[0], out index))
                                        {
                                            bld.Append(FormatParameter(args[index], formatString));
                                        }
                                        else
                                        {
                                            //throw new FormatException(FormatException.ERROR_MESSAGE);
                                        }
                                    }

                                    endOfLastMatch = endsearch + 1;

                                    found = true;
                                    starting = endsearch + 1;
                                    break;
                                }


                                endsearch = format.IndexOf('}', endsearch);
                            }
                            // need to find the ending point

                            if (!found)
                            {
                                throw new FormatException(FormatException.ERROR_MESSAGE);
                            }
                        }
                    }
                    else
                    {
                        // invalid
                        throw new FormatException(FormatException.ERROR_MESSAGE);
                    }

                }

            }

            // copy any additional remaining part of the format string.
            if (endOfLastMatch != format.Length)
            {
                bld.Append(format.Substring(endOfLastMatch, format.Length - endOfLastMatch));
            }

            return bld.ToString();
        }

        private static void ValidateFormatString(string format)
        {
            char expected = '{';

            int i = 0;

            while ((i = format.IndexOfAny(new char[] { '{', '}' }, i)) >= 0)
            {
                if (i < (format.Length - 1) && format[i] == format[i + 1])
                {
                    // escaped brace. continue looking.
                    i = i + 2;
                    continue;
                }
                else if (format[i] != expected)
                {
                    // badly formed string.
                    //throw new FormatException(FormatException.ERROR_MESSAGE);
                }
                else
                {
                    // move it along.
                    i++;

                    // expected it.
                    if (expected == '{')
                        expected = '}';
                    else
                        expected = '{';
                }
            }

            if (expected == '}')
            {
                // orpaned opening brace. Bad format.
                //throw new FormatException(FormatException.ERROR_MESSAGE);
            }

        }

        private static string FormatParameter(object p, string formatString)
        {
            if (formatString == string.Empty)
                return p.ToString();

            if (p as IFormattable != null)
            {
                return ((IFormattable)p).ToString(formatString, null);
            }
            else if (p is DateTime)
            {
                return ((DateTime)p).ToString(formatString);
            }
            else if (p is Double)
            {
                return ((Double)p).ToString(formatString);
            }
            else if (p is Int16)
            {
                return ((Int16)p).ToString(formatString);
            }
            else if (p is Int32)
            {
                return ((Int32)p).ToString(formatString);
            }
            else if (p is Int64)
            {
                return ((Int64)p).ToString(formatString);
            }
            else if (p is SByte)
            {
                return ((SByte)p).ToString(formatString);
            }
            else if (p is Single)
            {
                return ((Single)p).ToString(formatString);
            }
            else if (p is UInt16)
            {
                return ((UInt16)p).ToString(formatString);
            }
            else if (p is UInt32)
            {
                return ((UInt32)p).ToString(formatString);
            }
            else if (p is UInt64)
            {
                return ((UInt64)p).ToString(formatString);
            }
            else
            {
                return p.ToString();
            }
        }

        #endregion Format method

    }

    /// <summary>
    /// The exception that is thrown when the format of an argument does not meet the parameter specifications of the invoked method.
    /// </summary>
    public class FormatException : Exception
    {
        internal static string ERROR_MESSAGE = "String format is not valid";

        /// <summary>
        /// Initializes a new instance of the FormatException class.
        /// </summary>
        public FormatException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the FormatException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public FormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FormatException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="ex">The exception that is the cause of the current exception. If the innerException parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
        public FormatException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
