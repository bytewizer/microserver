using System;
using System.Collections;
using System.Reflection;
using System.Text;


namespace MicroServer.Utilities
{
    public static class ByteUtility
    {
        private const string HEX_INDEX = "0123456789abcdef          ABCDEF";
        private const string HEX_CHARS = "0123456789ABCDEF";
        
        #region Public Members

        // byte
        public static bool GetBit(byte value, int position)
        {
            return (GetBits(value, position, 1) == 1);
        }

        public static byte SetBit(ref byte value, int position, bool flag)
        {
            return SetBits(ref value, position, 1, (flag ? (byte)1 : (byte)0));
        }

        public static byte GetBits(byte value, int position, int length)
        {
            if (length <= 0 || position >= 8)
                return 0;

            int mask = (2 << (length - 1)) - 1;

            return (byte)((value >> position) & mask);
        }
        
        public static byte SetBits(ref byte value, int position, int length, byte bits)
        {
            if (length <= 0 || position >= 8)
                return value;

            int mask = (2 << (length - 1)) - 1;

            value &= (byte)~(mask << position);
            value |= (byte)((bits & mask) << position);

            return value;
        }

        // ushort
        public static bool GetBit(ushort value, int position)
        {
            return (GetBits(value, position, 1) == 1);
        }

        public static ushort SetBit(ref ushort value, int position, bool flag)
        {
            return SetBits(ref value, position, 1, (flag ? (ushort)1 : (ushort)0));
        }

        public static ushort GetBits(ushort value, int position, int length)
        {
            if (length <= 0 || position >= 16)
                return 0;

            int mask = (2 << (length - 1)) - 1;

            return (ushort)((value >> position) & mask);
        }

        public static ushort SetBits(ref ushort value, int position, int length, ushort bits)
        {
            if (length <= 0 || position >= 16)
                return value;

            int mask = (2 << (length - 1)) - 1;

            value &= (ushort)~(mask << position);
            value |= (ushort)((bits & mask) << position);

            return value;
        }

        // string
        public static string GetSafeString(byte[] bytes)
        {
            if (bytes != null)
            {

                return GetString(bytes, 0, bytes.Length);
            }
            else
            {
                return "Null";
            }
        }

        public static string GetString(byte[] bytes)
        {
            if (bytes != null)
            {

                return GetString(bytes, 0, bytes.Length);
            }
            else
            {
                return String.Empty;
            }
        }

        public static string GetString(byte[] bytes, int offset, int length)
        {
            string s = String.Empty;

            if (bytes != null)
            {             
                for (int i = offset; i < length && i < bytes.Length; i++)
                    s += (char)bytes[i];
            }

            return s;
        }

        public static string PrintByte(byte b)
        {
            return ByteToHex(b);
        }

        public static string BytesToHex(byte[] b)
        {
            string res = "";

            for (int i = 0; i < b.Length; i++)
                res += ByteToHex(b[i]);

            return res;
        }

        public static string ByteToHex(byte b)
        {
            int lowByte = b & 0x0F;
            int highByte = (b & 0xF0) >> 4;

            return new string(
                new char[] { HEX_CHARS[highByte], HEX_CHARS[lowByte] }
            );
        }

        public static byte[] HexToByte(string s)
        {
            int l = s.Length / 2;
            byte[] data = new byte[l];
            int j = 0;

            for (int i = 0; i < l; i++)
            {
                char c = s[j++];
                int n, b;

                n = HEX_INDEX.IndexOf(c);
                b = (n & 0xf) << 4;
                c = s[j++];
                n = HEX_INDEX.IndexOf(c);
                b += (n & 0xf);
                data[i] = (byte)b;
            }

            return data;
        }

        public static string PrintBytes(byte[] bytes)
        {
            return PrintBytes(bytes, bytes.Length);
        }

        public static string PrintSafeBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                return PrintBytes(bytes, bytes.Length);
            }
            else
            {
                return "Null";
            }
        }

        public static string PrintBytes(byte[] bytes, bool wrapLines)
        {
            return PrintBytes(bytes, bytes.Length, wrapLines);
        }

        public static string PrintBytes(byte[] bytes, int length)
        {
            return PrintBytes(bytes, length, true);
        }

        public static string PrintBytes(byte[] bytes, int length, bool wrapLines)
        {
            string s = string.Empty;

            int c = 0;

            for (int i = 0; i < length && i < bytes.Length; i++)
            {
                s += PrintByte(bytes[i]);

                if (++c == 24 && wrapLines)
                {
                    s += "\r\n";
                    c = 0;
                }
                else
                    if (i < length - 1)
                        s += "-";
            }

            return s;
        }

        public static byte[] Combine(byte[] value1, byte[] value2)
        {
            byte[] value = new byte[value1.Length + value2.Length];
            Array.Copy(value1, value, value1.Length);
            Array.Copy(value2, 0, value, value1.Length, value2.Length);

            return value;
        }

        public static ushort ReverseByteOrder(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] reverse = new byte[bytes.Length];

            int j = 0;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                reverse[j++] = bytes[i];
            }

            return BitConverter.ToUInt16(reverse, 0);
        }

        public static byte[] ReverseByteOrder(byte[] bytes)
        {
            byte[] reverse = new byte[bytes.Length];

            int j = 0;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                reverse[j++] = bytes[i];
            }

            return reverse;
        }

        public static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            int found = -1;
            bool matched = false;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            matched = true;
                            for (int y = 1; y <= searchBytes.Length - 1; y++)
                            {
                                if (searchIn[i + y] != searchBytes[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }

                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }

            }
            return found;
        }

        public static bool Equality(byte[] a1, byte[] b1)
        {
            // If not same length, done
            if (a1.Length != b1.Length)
            {
                return false;
            }

            // If they are the same object, done
            if (object.ReferenceEquals(a1, b1))
            {
                return true;
            }

            // Loop all values and compare
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != b1[i])
                {
                    return false;
                }
            }

            // If we got here, equal
            return true;
        }

        #endregion Public Members
    }
}
