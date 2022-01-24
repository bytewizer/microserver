//using System;
//using System.Collections;
//using System.Text;
//using System.Threading;

//namespace Bytewizer.TinyCLR
//{
//    public static class StringExtensions
//    {
//        internal static void SkipWhiteSpace(this string str, ref int pos)
//        {
//            if (pos >= str.Length)
//            {
//                return;
//            }

//            while (pos < str.Length && char.IsWhiteSpace(str[pos]))
//            {
//                pos++;
//            }
//        }

//        internal static string TrimAndLower(this string str)
//        {
//            if (str == null)
//                return null;

//            char[] buffer = new char[str.Length];
//            int length = 0;

//            for (int i = 0; i < str.Length; ++i)
//            {
//                char ch = str[i];
//                if (!char.IsWhiteSpace(ch) && !char.IsControl(ch))
//                    buffer[length++] = char.ToLowerInvariant(ch);
//            }

//            return new string(buffer, 0, length);
//        }

//        internal static char Peek(this string str, int pos)
//        {
//            if (pos < 0 || pos >= str.Length)
//            {
//                return null;
//            }

//            return str[pos];
//        }
//    }
//}
