using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Ftp
{
    internal class CommandHelper
    {
        public static string ConvertPathToLocal(string RootPath, string Directory)
        {
            if (string.IsNullOrEmpty(Directory))
            {
                return RootPath;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(RootPath);

            char[] chars = Directory.ToCharArray();
            char c;
            for (int i = 0; i < chars.Length; i++)
            {
                c = chars[i];

                if (c == '/')
                {
                    sb.Append('\\');
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string ConvertPathToRemote(string Directory)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('/');

            char[] chars = Directory.ToCharArray(3, Directory.Length - 3);  // without leading A|B:\
            char c;
            for (int i = 0; i < chars.Length; i++)
            {
                c = chars[i];

                if (c == '\\')
                {
                    sb.Append('/');
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

    }
}