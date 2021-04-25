using System;
using System.Runtime.InteropServices;

namespace Bytewizer.TinyCLR.IO.Compression
{
	/// <summary>
	/// Encoder and Decoder for ZLIB and DEFLATE (IETF RFC1950 and RFC1951).
	/// </summary>
	/// <remarks>
	/// This class compresses and decompresses data according to the Deflate algorithm
	/// and optionally, the ZLIB format, as documented in <see
	/// href="http://www.ietf.org/rfc/rfc1950.txt">RFC 1950 - ZLIB</see> and <see
	/// href="http://www.ietf.org/rfc/rfc1951.txt">RFC 1951 - DEFLATE</see>.
	/// </remarks>
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	public sealed class ZlibCodec
	{
		/// <summary>
		/// The buffer from which data is taken.
		/// </summary>
		public byte[] InputBuffer;

		/// <summary>
		/// An index into the InputBuffer array, indicating where to start reading. 
		/// </summary>
		public int NextIn;

		/// <summary>
		/// The number of bytes available in the InputBuffer, starting at NextIn. 
		/// </summary>
		public int AvailableBytesIn;

		/// <summary>
		/// Total number of bytes read so far, through all calls to Inflate()/Deflate().
		/// </summary>
		public long TotalBytesIn;

		/// <summary>
		/// Buffer to store output data.
		/// </summary>
		public byte[] OutputBuffer;

		/// <summary>
		/// An index into the OutputBuffer array, indicating where to start writing. 
		/// </summary>
		public int NextOut;

		/// <summary>
		/// The number of bytes available in the OutputBuffer, starting at NextOut. 
		/// </summary>
		public int AvailableBytesOut;

		/// <summary>
		/// Total number of bytes written to the output so far, through all calls to Inflate()/Deflate().
		/// </summary>
		public long TotalBytesOut;

		/// <summary>
		/// used for diagnostics, when something goes wrong!
		/// </summary>
		public string Message;


		internal DeflateManager dstate;

		internal InflateManager istate;

		internal uint _Adler32;

		/// <summary>
		/// The compression level to use in this codec.  Useful only in compression mode.
		/// </summary>
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		/// <summary>
		/// The number of Window Bits to use.  
		/// </summary>
		public int WindowBits = 15;

		/// <summary>
		/// The compression strategy to use.
		/// </summary>
		public CompressionStrategy Strategy = CompressionStrategy.Default;


		/// <summary>
		/// The Adler32 checksum on the data transferred through the codec so far.
		/// </summary>
		public int Adler32 => (int)_Adler32;

		/// <summary>
		/// Create a ZlibCodec.
		/// </summary>
		/// <remarks>
		/// If you use this default constructor, you will later have to explicitly call 
		/// InitializeInflate() or InitializeDeflate() before using the ZlibCodec to compress 
		/// or decompress. 
		/// </remarks>
		public ZlibCodec()
		{
		}

		/// <summary>
		/// Create a ZlibCodec that either compresses or decompresses.
		/// </summary>
		/// <param name="mode">
		/// Indicates whether the codec should compress (deflate) or decompress (inflate).
		/// </param>
		public ZlibCodec(CompressionMode mode)
		{
			switch (mode)
			{
			case CompressionMode.Compress:
				if (InitializeDeflate() != 0)
				{
					throw new Exception("Cannot initialize for deflate.");
				}
				break;
			case CompressionMode.Decompress:
				if (InitializeInflate() != 0)
				{
					throw new Exception("Cannot initialize for inflate.");
				}
				break;
			default:
				throw new Exception("Invalid ZlibStreamFlavor.");
			}
		}


		/// <summary>
		/// Initialize the inflation state. 
		/// </summary>
		/// <returns>Z_OK if everything goes well.</returns>
		public int InitializeInflate()
		{
			return InitializeInflate(WindowBits);
		}

		/// <summary>
		/// Initialize the inflation state with an explicit flag to
		/// govern the handling of RFC1950 header bytes.
		/// </summary>
		/// <remarks>
		/// By default, the ZLIB header defined in <see
		/// href="http://www.ietf.org/rfc/rfc1950.txt">RFC 1950</see> is expected.  If
		/// you want to read a zlib stream you should specify true for
		/// expectRfc1950Header.  If you have a deflate stream, you will want to specify
		/// false. It is only necessary to invoke this initializer explicitly if you
		/// want to specify false.
		/// </remarks>
		/// <param name="expectRfc1950Header">whether to expect an RFC1950 header byte
		/// pair when reading the stream of data to be inflated.</param>
		/// <returns>Z_OK if everything goes well.</returns>
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return InitializeInflate(WindowBits, expectRfc1950Header);
		}

		/// <summary>
		/// Initialize the ZlibCodec for inflation, with the specified number of window bits. 
		/// </summary>
		/// <param name="windowBits">The number of window bits to use. If you need to ask what that is, 
		/// then you shouldn't be calling this initializer.</param>
		/// <returns>Z_OK if all goes well.</returns>
		public int InitializeInflate(int windowBits)
		{
			WindowBits = windowBits;
			return InitializeInflate(windowBits, expectRfc1950Header: true);
		}

		/// <summary>
		/// Initialize the inflation state with an explicit flag to govern the handling of
		/// RFC1950 header bytes. 
		/// </summary>
		/// <remarks>
		/// If you want to read a zlib stream you should specify true for
		/// expectRfc1950Header. In this case, the library will expect to find a ZLIB
		/// header, as defined in <see href="http://www.ietf.org/rfc/rfc1950.txt">RFC
		/// 1950</see>, in the compressed stream.  If you will be reading a DEFLATE or
		/// GZIP stream, which does not have such a header, you will want to specify
		/// false.
		/// </remarks>
		/// <param name="expectRfc1950Header">whether to expect an RFC1950 header byte pair when reading 
		/// the stream of data to be inflated.</param>
		/// <param name="windowBits">The number of window bits to use. If you need to ask what that is, 
		/// then you shouldn't be calling this initializer.</param>
		/// <returns>Z_OK if everything goes well.</returns>
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			WindowBits = windowBits;
			if (dstate != null)
			{
				throw new Exception("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			istate = new InflateManager(expectRfc1950Header);
			return istate.Initialize(this, windowBits);
		}

		/// <summary>
		/// Inflate the data in the InputBuffer, placing the result in the OutputBuffer.
		/// </summary>
		/// <param name="flush">The flush to use when inflating.</param>
		/// <returns>Z_OK if everything goes well.</returns>
		public int Inflate(FlushType flush)
		{
			if (istate == null)
			{
				throw new Exception("No Inflate State!");
			}
			return istate.Inflate(flush);
		}

		/// <summary>
		/// Ends an inflation session. 
		/// </summary>
		/// <returns>Z_OK if everything goes well.</returns>
		public int EndInflate()
		{
			if (istate == null)
			{
				throw new Exception("No Inflate State!");
			}
			int result = istate.End();
			istate = null;
			return result;
		}

		/// <summary>
		/// Sync Inflate.
		/// </summary>
		public int SyncInflate()
		{
			if (istate == null)
			{
				throw new Exception("No Inflate State!");
			}
			return istate.Sync();
		}

		/// <summary>
		/// Initialize the ZlibCodec for deflation operation.
		/// </summary>
		public int InitializeDeflate()
		{
			return InternalInitializeDeflate(wantRfc1950Header: true);
		}

		/// <summary>
		/// Initialize the ZlibCodec for deflation operation, using the specified CompressionLevel.
		/// </summary>
		public int InitializeDeflate(CompressionLevel level)
		{
			CompressLevel = level;
			return InternalInitializeDeflate(wantRfc1950Header: true);
		}

		/// <summary>
		/// Initialize the ZlibCodec for deflation operation, using the specified CompressionLevel, 
		/// and the explicit flag governing whether to emit an RFC1950 header byte pair.
		/// </summary>
		/// <remarks>
		/// The codec will use the maximum window bits (15) and the specified CompressionLevel.
		/// If you want to generate a zlib stream, you should specify true for
		/// wantRfc1950Header. In this case, the library will emit a ZLIB
		/// header, as defined in <see href="http://www.ietf.org/rfc/rfc1950.txt">RFC
		/// 1950</see>, in the compressed stream.  
		/// </remarks>
		/// <param name="level">The compression level for the codec.</param>
		/// <param name="wantRfc1950Header">whether to emit an initial RFC1950 byte pair in the compressed stream.</param>
		/// <returns>Z_OK if all goes well.</returns>
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			CompressLevel = level;
			return InternalInitializeDeflate(wantRfc1950Header);
		}

		/// <summary>
		/// Initialize the ZlibCodec for deflation operation, using the specified CompressionLevel, 
		/// and the specified number of window bits. 
		/// </summary>
		/// <remarks>
		/// The codec will use the specified number of window bits and the specified CompressionLevel.
		/// </remarks>
		/// <param name="level">The compression level for the codec.</param>
		/// <param name="bits">the number of window bits to use.  If you don't know what this means, don't use this method.</param>
		/// <returns>Z_OK if all goes well.</returns>
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			CompressLevel = level;
			WindowBits = bits;
			return InternalInitializeDeflate(wantRfc1950Header: true);
		}

		/// <summary>
		/// Initialize the ZlibCodec for deflation operation, using the specified
		/// CompressionLevel, the specified number of window bits, and the explicit flag
		/// governing whether to emit an RFC1950 header byte pair.
		/// </summary>
		/// <param name="level">The compression level for the codec.</param>
		/// <param name="wantRfc1950Header">whether to emit an initial RFC1950 byte pair in the compressed stream.</param>
		/// <param name="bits">the number of window bits to use.  If you don't know what this means, don't use this method.</param>
		/// <returns>Z_OK if all goes well.</returns>
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			CompressLevel = level;
			WindowBits = bits;
			return InternalInitializeDeflate(wantRfc1950Header);
		}

		private int InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (istate != null)
			{
				throw new Exception("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
            dstate = new DeflateManager
            {
                WantRfc1950HeaderBytes = wantRfc1950Header
            };
            return dstate.Initialize(this, CompressLevel, WindowBits, Strategy);
		}

		/// <summary>
		/// Deflate one batch of data.
		/// </summary>
		public int Deflate(FlushType flush)
		{
			if (dstate == null)
			{
				throw new Exception("No Deflate State!");
			}
			return dstate.Deflate(flush);
		}

		/// <summary>
		/// End a deflation session.
		/// </summary>
		/// <remarks>
		/// Call this after making a series of one or more calls to Deflate(). All buffers are flushed.
		/// </remarks>
		/// <returns>Z_OK if all goes well.</returns>
		public int EndDeflate()
		{
			if (dstate == null)
			{
				throw new Exception("No Deflate State!");
			}
			dstate = null;
			return 0;
		}

		/// <summary>
		/// Reset a codec for another deflation session.
		/// </summary>
		/// <remarks>
		/// Call this to reset the deflation state.  For example if a thread is deflating
		/// non-consecutive blocks, you can call Reset() after the Deflate(Sync) of the first
		/// block and before the next Deflate(None) of the second block.
		/// </remarks>
		/// <returns>Z_OK if all goes well.</returns>
		public void ResetDeflate()
		{
			if (dstate == null)
			{
				throw new Exception("No Deflate State!");
			}
			dstate.Reset();
		}

		/// <summary>
		/// Set the CompressionStrategy and CompressionLevel for a deflation session.
		/// </summary>
		/// <param name="level">the level of compression to use.</param>
		/// <param name="strategy">the strategy to use for compression.</param>
		/// <returns>Z_OK if all goes well.</returns>
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (dstate == null)
			{
				throw new Exception("No Deflate State!");
			}
			return dstate.SetParams(level, strategy);
		}

		/// <summary>
		/// Set the dictionary to be used for either Inflation or Deflation.
		/// </summary>
		/// <param name="dictionary">The dictionary bytes to use.</param>
		/// <returns>Z_OK if all goes well.</returns>
		public int SetDictionary(byte[] dictionary)
		{
			if (istate != null)
			{
				return istate.SetDictionary(dictionary);
			}

			if (dstate != null)
			{
				return dstate.SetDictionary(dictionary);
			}

			throw new Exception("No inflate or deflate state!");
		}

		internal void FlushPending()
		{
			int num = dstate.pendingCount;
			if (num > AvailableBytesOut)
			{
				num = AvailableBytesOut;
			}
			if (num != 0)
			{
				if (dstate.pending.Length <= dstate.nextPending || OutputBuffer.Length <= NextOut || dstate.pending.Length < dstate.nextPending + num || OutputBuffer.Length < NextOut + num)
				{
					throw new Exception($"Invalid State. (pending.Length={dstate.pending.Length}, pendingCount={dstate.pendingCount})");
				}
				Array.Copy(dstate.pending, dstate.nextPending, OutputBuffer, NextOut, num);
				NextOut += num;
				dstate.nextPending += num;
				TotalBytesOut += num;
				AvailableBytesOut -= num;
				dstate.pendingCount -= num;
				if (dstate.pendingCount == 0)
				{
					dstate.nextPending = 0;
				}
			}
		}

		internal int ReadBuf(byte[] buf, int start, int size)
		{
			int num = AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			AvailableBytesIn -= num;
			if (dstate.WantRfc1950HeaderBytes)
			{
				_Adler32 = Adler.Adler32(_Adler32, InputBuffer, NextIn, num);
			}
			Array.Copy(InputBuffer, NextIn, buf, start, num);
			NextIn += num;
			TotalBytesIn += num;
			return num;
		}
	}
}
