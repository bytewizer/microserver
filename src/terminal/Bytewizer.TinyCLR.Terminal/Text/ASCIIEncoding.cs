using System;
using System.Text;

namespace Bytewizer.TinyCLR.Text
{
    public class ASCIIEncoding : Encoding
    {
        private readonly char _fallbackChar;
        private static readonly char[] ByteToChar;

        static ASCIIEncoding()
        {
            if (ByteToChar == null)
            {
                ByteToChar = new char[128];
                var ch = '\0';
                for (byte i = 0; i < 128; i++)
                {
                    ByteToChar[i] = ch++;
                }
            }
        }

        public ASCIIEncoding()
        {
            _fallbackChar = '?';
        }

        public override byte[] GetBytes(string s)
        {
            byte[] retval = new byte[s.Length];
            for (int ix = 0; ix < s.Length; ++ix)
            {
                char ch = s[ix];
                if (ch <= 0x7f)
                {
                    retval[ix] = (byte)ch;
                }
                else
                { 
                    retval[ix] = (byte)_fallbackChar; 
                }
            }
            return retval;
        }

        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            for (int i = 0; i < charCount && i < s.Length; i++)
            {
                var b = (byte)s[i + charIndex];

                if (b > 127)
                    b = (byte)_fallbackChar;

                bytes[i + byteIndex] = b;
            }
            return charCount;
        }

        public override char[] GetChars(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override char[] GetChars(byte[] bytes, int byteIndex, int byteCount)
        {
            char[] chars = new char[bytes.Length];

            for (int i = 0; i < byteCount; i++)
            {
                var b = bytes[i + byteIndex];
                char ch;

                if (b > 127)
                {
                    ch = _fallbackChar;
                }
                else
                {
                    ch = ByteToChar[b];
                }

                chars[i] = ch;
            }

            return chars;
        }

        public override string GetString(byte[] bytes, int index, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int ix = index; ix < count; ++ix)
            {
                char ch = (char)bytes[ix];
                if (ch <= 0x7f)
                {
                    sb.Append(ch);
                }
                else
                {
                    sb.Append(_fallbackChar);
                }
            }
            return sb.ToString();
        }

        public override Decoder GetDecoder()
        {
            throw new NotImplementedException();
        }
    }
}