using System;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Bytewizer.Playground.Cryptography
{
    class Program
    {
        static void Main()
        {
            string text = "The quick brown fox jumps over the lazy dog";

            using (var algorithm = SHA1.Create())
            {
                byte[] buffer = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text), 0, text.Length);

                char[] output = new char[buffer.Length * 2];
                for (int i = 0; i < buffer.Length; i++)
                {
                    output[(i << 1) + 0] = (buffer[i] >> 4) <= 9 ? (char)((buffer[i] >> 4) + '0') : (char)((buffer[i] >> 4) - 10 + 'A');
                    output[(i << 1) + 1] = (buffer[i] & 15) <= 9 ? (char)((buffer[i] & 15) + '0') : (char)((buffer[i] & 15) - 10 + 'A');
                }

                var hashString = new string(output);
                Debug.WriteLine($"SHA1 Hash: {new string(output)}");
                Debug.WriteLine($"SHA1 Matching Hash: {hashString.Equals("2FD4E1C67A2D28FCED849EE1BB76E7391B93EB12")}");              
            }

            using (var algorithm = SHA256.Create())
            {
                byte[] buffer = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text), 0, text.Length);

                char[] output = new char[buffer.Length * 2];
                for (int i = 0; i < buffer.Length; i++)
                {
                    output[(i << 1) + 0] = (buffer[i] >> 4) <= 9 ? (char)((buffer[i] >> 4) + '0') : (char)((buffer[i] >> 4) - 10 + 'A');
                    output[(i << 1) + 1] = (buffer[i] & 15) <= 9 ? (char)((buffer[i] & 15) + '0') : (char)((buffer[i] & 15) - 10 + 'A');
                }

                var hashString = new string(output);
                Debug.WriteLine($"SHA256 Hash: {new string(output)}");
                Debug.WriteLine($"SHA256 Matching Hash: {hashString.Equals("D7A8FBB307D7809469CA9ABCB0082E4F8D5651E46D3CDB762D02D0BF37C9E592")}");
            }
            
            using (var algorithm = SHA384.Create())
            {
                byte[] buffer = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text), 0, text.Length);

                char[] output = new char[buffer.Length * 2];
                for (int i = 0; i < buffer.Length; i++)
                {
                    output[(i << 1) + 0] = (buffer[i] >> 4) <= 9 ? (char)((buffer[i] >> 4) + '0') : (char)((buffer[i] >> 4) - 10 + 'A');
                    output[(i << 1) + 1] = (buffer[i] & 15) <= 9 ? (char)((buffer[i] & 15) + '0') : (char)((buffer[i] & 15) - 10 + 'A');
                }

                var hashString = new string(output);
                Debug.WriteLine($"SHA384 Hash: {new string(output)}");
                Debug.WriteLine($"SHA384 Matching Hash: {hashString.Equals("CA737F1014A48F4C0B6DD43CB177B0AFD9E5169367544C494011E3317DBF9A509CB1E5DC1E85A941BBEE3D7F2AFBC9B1")}");
            }

            using (var algorithm = SHA512.Create())
            {
                byte[] buffer = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text), 0, text.Length);

                char[] output = new char[buffer.Length * 2];
                for (int i = 0; i < buffer.Length; i++)
                {
                    output[(i << 1) + 0] = (buffer[i] >> 4) <= 9 ? (char)((buffer[i] >> 4) + '0') : (char)((buffer[i] >> 4) - 10 + 'A');
                    output[(i << 1) + 1] = (buffer[i] & 15) <= 9 ? (char)((buffer[i] & 15) + '0') : (char)((buffer[i] & 15) - 10 + 'A');
                }

                var hashString = new string(output);
                Debug.WriteLine($"SHA512 Hash: {new string(output)}");
                Debug.WriteLine($"SHA512 Matching Hash: {hashString.Equals("07E547D9586F6A73F73FBAC0435ED76951218FB7D0C8D788A309D785436BBB642E93A252A954F23912547D1E8A3B5ED6E1BFD7097821233FA0538F3DB854FEE6")}");
            }
        }
    }
}