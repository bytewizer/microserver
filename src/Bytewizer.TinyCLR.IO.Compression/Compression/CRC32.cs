using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Bytewizer.TinyCLR.IO.Compression
{
    /// <summary>
    /// Computes a CRC-32. The CRC-32 algorithm is parameterized - you
    /// can set the polynomial and enable or disable bit
    /// reversal. This can be used for GZIP, BZip2, or ZIP.
    /// </summary>
    public class CRC32
    {
        private readonly uint _dwPolynomial;
        private readonly bool _reverseBits;
       
        private long _TotalBytesRead;
        private uint _register = 0xFFFFFFFFU;
        private uint[] _crc32Table;


        /// <summary>
        /// Create an instance of the CRC32 class using the default settings: no
        /// bit reversal, and a polynomial of 0xEDB88320.
        /// </summary>
        public CRC32() 
            : this(false)
        {
        }

        /// <summary>
        /// Create an instance of the CRC32 class, specifying whether to reverse
        /// data bits or not.
        /// </summary>
        /// <param name='reverseBits'>
        /// specify true if the instance should reverse data bits.
        /// </param>
        /// <remarks>
        /// <para>
        /// In the CRC-32 used by BZip2, the bits are reversed. Therefore if you
        /// want a CRC32 with compatibility with BZip2, you should pass true
        /// here. In the CRC-32 used by GZIP and PKZIP, the bits are not
        /// reversed; Therefore if you want a CRC32 with compatibility with
        /// those, you should pass false.
        /// </para>
        /// </remarks>
        public CRC32(bool reverseBits)
            : this(unchecked((int)0xEDB88320), reverseBits)
        {
        }

        /// <summary>
        /// Create an instance of the CRC32 class, specifying the polynomial and
        /// whether to reverse data bits or not.
        /// </summary>
        /// <param name='polynomial'>
        /// The polynomial to use for the CRC, expressed in the reversed (LSB)
        /// format: the highest ordered bit in the polynomial value is the
        /// coefficient of the 0th power; the second-highest order bit is the
        /// coefficient of the 1 power, and so on. Expressed this way, the
        /// polynomial for the CRC-32C used in IEEE 802.3, is 0xEDB88320.
        /// </param>
        /// <param name='reverseBits'>
        /// specify true if the instance should reverse data bits.
        /// </param>
        ///
        /// <remarks>
        /// <para>
        ///  In the CRC-32 used by BZip2, the bits are reversed. Therefore if you
        ///  want a CRC32 with compatibility with BZip2, you should pass true
        ///  here for the <c>reverseBits</c> parameter. In the CRC-32 used by
        ///  GZIP and PKZIP, the bits are not reversed; Therefore if you want a
        ///  CRC32 with compatibility with those, you should pass false for the
        ///  <c>reverseBits</c> parameter.
        ///  </para>
        /// </remarks>
        public CRC32(int polynomial, bool reverseBits)
        {
            _reverseBits = reverseBits;
            _dwPolynomial = (uint)polynomial;
            
            GenerateLookupTable();
        }

        /// <summary>
        ///   Indicates the total number of bytes applied to the CRC.
        /// </summary>
        public long TotalBytesRead
        {
            get
            {
                return _TotalBytesRead;
            }
        }

        /// <summary>
        /// Indicates the current CRC for all blocks slurped in.
        /// </summary>
        public int Crc32Result
        {
            get
            {
                return unchecked((int)(~_register));
            }
        }

        /// <summary>
        /// Update the value for the running CRC32 using the given block of bytes.
        /// This is useful when using the CRC32() class in a Stream.
        /// </summary>
        /// <param name="block">Block of bytes to slurp.</param>
        /// <param name="offset">Starting point in the block.</param>
        /// <param name="count">How many bytes within the block to slurp.</param>
        public void SlurpBlock(byte[] block, int offset, int count)
        {
            if (block == null)
                throw new Exception("The data buffer must not be null.");

            // bzip algorithm
            for (int i = 0; i < count; i++)
            {
                int x = offset + i;
                byte b = block[x];
                if (this._reverseBits)
                {
                    uint temp = (_register >> 24) ^ b;
                    _register = (_register << 8) ^ _crc32Table[temp];
                }
                else
                {
                    uint temp = (_register & 0x000000FF) ^ b;
                    _register = (_register >> 8) ^ _crc32Table[temp];
                }
            }
            _TotalBytesRead += count;
        }

        private static uint ReverseBits(uint data)
        {
            unchecked
            {
                uint ret = data;
                ret = (ret & 0x55555555) << 1 | (ret >> 1) & 0x55555555;
                ret = (ret & 0x33333333) << 2 | (ret >> 2) & 0x33333333;
                ret = (ret & 0x0F0F0F0F) << 4 | (ret >> 4) & 0x0F0F0F0F;
                ret = (ret << 24) | ((ret & 0xFF00) << 8) | ((ret >> 8) & 0xFF00) | (ret >> 24);
                return ret;
            }
        }

        private static byte ReverseBits(byte data)
        {
            unchecked
            {
                uint u = (uint)data * 0x00020202;
                uint m = 0x01044010;
                uint s = u & m;
                uint t = (u << 2) & (m << 1);
                return (byte)((0x01001001 * (s + t)) >> 24);
            }
        }

        private void GenerateLookupTable()
        {
            _crc32Table = new uint[256];
            unchecked
            {
                uint dwCrc;
                byte i = 0;
                do
                {
                    dwCrc = i;
                    for (byte j = 8; j > 0; j--)
                    {
                        if ((dwCrc & 1) == 1)
                        {
                            dwCrc = (dwCrc >> 1) ^ _dwPolynomial;
                        }
                        else
                        {
                            dwCrc >>= 1;
                        }
                    }
                    if (_reverseBits)
                    {
                        _crc32Table[ReverseBits(i)] = ReverseBits(dwCrc);
                    }
                    else
                    {
                        _crc32Table[i] = dwCrc;
                    }
                    i++;
                } while (i != 0);
            }
        }
    }
}
