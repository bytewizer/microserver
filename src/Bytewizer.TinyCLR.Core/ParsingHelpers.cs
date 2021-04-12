using System;
using System.Collections;

namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParsingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public static ArrayList SplitByComma(string source)
        {
            var result = new ArrayList();
            var values = source.Split(new char[] { ',' });

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value.Trim().ToUpper());
                }
            }

            return result;
        }

        /// <summary>Creates a random guid with a prefix</summary>
        /// <param name="Prefix">The prefix of the id; null = no prefix</param>
        /// <param name="Length">The length of the id to generate</param>
        /// <returns>The random guid. Ex. Prefix-XXXXXXXXXXXXXXXX</returns>
        public static string CreateGuid(string Prefix, int Length = 16)
        {
            string final = null;
            string ids = "0123456789abcdefghijklmnopqrstuvwxyz";

            Random random = new Random();

            // Loop and get a random index in the ids and append to id 
            for (short i = 0; i < Length; i++)
            {
                final += ids[random.Next(ids.Length)];
            }

            // Return the guid without a prefix
            if (Prefix == null)
            { 
                return final; 
            }

            // Return the guid with a prefix
            return string.Format("{0}-{1}", Prefix, final);
        }
    }
}
