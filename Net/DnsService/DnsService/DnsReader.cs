/* 
 * DnsReader.cs
 * 
 * Copyright (c) 2009, Michael Schwarz (http://www.schwarz-interactive.de)
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR
 * ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * MS   09-03-26    fixed set position to int instead of long (.NET MF does only support arrays with int.MaxValue)
 * 
 */
using System;
using System.Text;
using System.IO;
using MicroServer.IO;
//using Naidar.Common.Utilities;

namespace MicroServer.Net.Dns
{
    internal class DnsReader : ByteReader
    {
        public DnsReader(byte[] message)
            : base(message)
        {
            _byteOrder = ByteOrder.Network;
        }

        public DnsReader(byte[] message, ByteOrder byteOrder, Encoding encoding, int position)
            : base(message, byteOrder, encoding, position)
        {
        }

        public DnsReader Copy()
        {
            return new DnsReader(_message, _byteOrder, _encoding, _position);
        }

        /// <summary>
        /// Reads a string from the byte array. RFC 1035 strings are prefixed with a 8-bit length indicator.
        /// </summary>
        /// <returns></returns>
        public override string ReadString()
        {
            short length = this.ReadByte();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(ReadChar());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reads a domain name from the byte array. (RFC 1035 - 4.1.4.)
        /// </summary>
        public string ReadDomain()
        {
            StringBuilder domain = new StringBuilder();
            int length = 0;

            while ((length = ReadByte()) != 0)
            {
                // top 2 bits set denotes domain name compression and to reference elsewhere
                if ((length & 0xc0) == 0xc0)
                {
                    // work out the existing domain name, copy this pointer
                    DnsReader newPointer = Copy();

                    // and move it to where specified here
                    newPointer.Position = (length & 0x3f) << 8 | ReadByte();

                    // repeat call recursively
                    domain.Append(newPointer.ReadDomain());
                    return domain.ToString();
                }

                // if not using compression, copy a char at a time to the domain name
                while (length > 0)
                {
                    domain.Append(ReadChar());
                    length--;
                }

                // if size of next label isn't null (end of domain name) add a period ready for next label
                if (Peek() != 0) domain.Append('.');
            }

            return domain.ToString();
        }

        public static DnsReader operator +(DnsReader br, int offset)
        {
            return new DnsReader(br._message, br._byteOrder, br._encoding, br._position + offset);
        }
    }
}