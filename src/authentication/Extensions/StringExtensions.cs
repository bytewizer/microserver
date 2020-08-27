using System;
using System.Text;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http
{
    public static class StringExtensions
    {
        /// <summary>
        /// Compute the SHA1 hash of a string.
        /// </summary>
        /// <param name="source"></param>
        public static string ToSha1(this string source)
        {
            if (source == null)
            {
                return string.Empty;
            }
            else
            {
                var crypto = MD5.Create();
                var encoded = Encoding.UTF8.GetBytes(source);
                var hash = crypto.ComputeHash(encoded);

                return BitConverter.ToString(hash);
            }
        }
    }
}
