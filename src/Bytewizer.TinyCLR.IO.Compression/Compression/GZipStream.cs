using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.IO.Compression
{
	/// <summary>
	///   A class for compressing and decompressing GZIP streams.
	/// </summary>
	public class GZipStream : Stream
	{
		private int _headerByteCount;
		private bool _disposed;
		private bool _firstReadDone;
		private string _FileName;
		private string _Comment;
		private int _Crc32;

		internal ZlibBaseStream _baseStream;
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
		internal static readonly Encoding iso8859dash1 = Encoding.UTF8;

		/// <summary>
		/// The last modified time for the GZIP stream.
		/// </summary>
		public DateTime LastModified;

		/// <summary>
		/// The comment on the GZIP stream.
		/// </summary>
		public string Comment
		{
			get
			{
				return _Comment;
			}
			set
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				_Comment = value;
			}
		}

		/// <summary>
		/// The FileName for the GZIP stream.
		/// </summary>
		public string FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				_FileName = value;
				if (_FileName != null)
				{
					if (_FileName.IndexOf("/") != -1)
					{
						_FileName = _FileName.Replace("/", "\\");
					}
					if (_FileName.EndsWith("\\"))
					{
						throw new Exception("Illegal filename");
					}
					if (_FileName.IndexOf("\\") != -1)
					{
						_FileName = Path.GetFileName(_FileName);
					}
				}
			}
		}

		/// <summary>
		/// The CRC on the GZIP stream.
		/// </summary>
		public int Crc32 => _Crc32;

		/// <summary>
		/// This property sets the flush behavior on the stream.
		/// </summary>
		public virtual FlushType FlushMode
		{
			get
			{
				return _baseStream._flushMode;
			}
			set
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				_baseStream._flushMode = value;
			}
		}

		/// <summary>
		/// The size of the working buffer for the compression codec.
		/// </summary>
		public int BufferSize
		{
			get
			{
				return _baseStream._bufferSize;
			}
			set
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}

				if (_baseStream._workingBuffer != null)
				{
					throw new Exception("The working buffer is already set.");
				}

				if (value < 1024)
				{
					throw new Exception($"Invalid {value} buffer size. Use a larger buffer of at least 1024.");
				}

				_baseStream._bufferSize = value;
			}
		}

		/// <summary>
		/// Returns the total number of bytes input so far.
		/// </summary>
		public virtual long TotalIn => _baseStream._z.TotalBytesIn;

		/// <summary>
		/// Returns the total number of bytes output so far.
		/// </summary>
		public virtual long TotalOut => _baseStream._z.TotalBytesOut;

		/// <summary>
		/// Indicates whether the stream can be read.
		/// </summary>
		public override bool CanRead
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return _baseStream._stream.CanRead;
			}
		}

		/// <summary>
		/// Indicates whether the stream supports Seek operations.
		/// </summary>
		public override bool CanSeek => false;

		/// <summary>
		/// Indicates whether the stream can be written.
		/// </summary>
		public override bool CanWrite
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return _baseStream._stream.CanWrite;
			}
		}

		/// <summary>
		/// Reading this property always throws a <see cref="NotImplementedException"/>.
		/// </summary>
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}


		/// <summary>
		/// The position of the stream pointer.
		/// </summary>
		public override long Position
		{
			get
			{
				if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return _baseStream._z.TotalBytesOut + _headerByteCount;
				}
				if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return _baseStream._z.TotalBytesIn + _baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Create a <c>GZipStream</c> using the specified <c>CompressionMode</c>.
		/// </summary>
		/// <param name="stream">The stream which will be read or written.</param>
		/// <param name="mode">Indicates whether the GZipStream will compress or decompress.</param>
		public GZipStream(Stream stream, CompressionMode mode)
			: this(stream, mode, CompressionLevel.Default, false)
		{
		}

		/// <summary>
		/// Create a <c>GZipStream</c> using the specified <c>CompressionMode</c> and
		/// the specified <c>CompressionLevel</c>.
		/// </summary>
		/// <param name="stream">The stream to be read or written while deflating or inflating.</param>
		/// <param name="mode">Indicates whether the <c>GZipStream</c> will compress or decompress.</param>
		/// <param name="level">A tuning knob to trade speed for effectiveness.</param>
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level)
			: this(stream, mode, level, false)
		{
		}

		/// <summary>
		/// Create a <c>GZipStream</c> using the specified <c>CompressionMode</c>, and
		/// explicitly specify whether the stream should be left open after Deflation
		/// or Inflation.
		/// </summary>
		/// <param name="stream">The stream to be read or written while deflating or inflating.</param>
		/// <param name="mode">Indicates whether the <c>GZipStream</c> will compress or decompress.</param>
		/// <param name="leaveOpen">true if the application would like the base stream to remain open after inflation/deflation.</param>
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		/// <summary>
		/// Create a <c>GZipStream</c> using the specified <c>CompressionMode</c> and the
		/// specified <c>CompressionLevel</c>, and explicitly specify whether the
		///  stream should be left open after Deflation or Inflation.
		/// </summary>
		/// <param name="stream">The stream which will be read or written.</param>
		/// <param name="mode">Indicates whether the GZipStream will compress or decompress.</param>
		/// <param name="leaveOpen">true if the application would like the stream to remain open after inflation/deflation.</param>
		/// <param name="level">A tuning knob to trade speed for effectiveness.</param>
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			_baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		/// <summary>
		/// Dispose the stream.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!_disposed)
				{
					if (disposing && _baseStream != null)
					{
						_baseStream.Close();
						_Crc32 = _baseStream.Crc32;
					}
					_disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		/// <summary>
		/// This property sets the flush behavior on the stream.
		/// </summary>
		public override void Flush()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			_baseStream.Flush();
		}

		/// <summary>
		///  Read and decompress data from the source stream.
		/// </summary>
		/// <param name="buffer">The buffer into which the decompressed data should be placed.</param>
		/// <param name="offset">the offset within that data array to put the first byte read.</param>
		/// <param name="count">the number of bytes to read.</param>
		/// <returns>the number of bytes actually read</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = _baseStream.Read(buffer, offset, count);
			if (!_firstReadDone)
			{
				_firstReadDone = true;
				FileName = _baseStream._GzipFileName;
				Comment = _baseStream._GzipComment;
			}
			return result;
		}

		/// <summary>
		/// Calling this method always throws a <see cref="NotImplementedException"/>.
		/// </summary>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calling this method always throws a <see cref="NotImplementedException"/>.
		/// </summary>
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Write data to the stream.
		/// </summary>
		/// <param name="buffer">The buffer holding data to write to the stream.</param>
		/// <param name="offset">the offset within that data array to find the first byte to write.</param>
		/// <param name="count">the number of bytes to write.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!_baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				_headerByteCount = EmitHeader();
			}
			_baseStream.Write(buffer, offset, count);
		}

		private int EmitHeader()
		{
			byte[] array = ((Comment == null) ? null : iso8859dash1.GetBytes(Comment));
			byte[] array2 = ((FileName == null) ? null : iso8859dash1.GetBytes(FileName));
			int num = ((Comment != null) ? (array.Length + 1) : 0);
			int num2 = ((FileName != null) ? (array2.Length + 1) : 0);
			int num3 = 10 + num + num2;
			byte[] array3 = new byte[num3];
			int num4 = 0;
			array3[num4++] = 31;
			array3[num4++] = 139;
			array3[num4++] = 8;
			byte b = 0;
			if (Comment != null)
			{
				b = (byte)(b ^ 0x10u);
			}
			if (FileName != null)
			{
				b = (byte)(b ^ 8u);
			}
			array3[num4++] = b;
			if (LastModified == DateTime.MinValue)
			{
				LastModified = DateTime.Now;
			}
			int value = (int)(LastModified - _unixEpoch).TotalSeconds;
			Array.Copy(BitConverter.GetBytes(value), 0, array3, num4, 4);
			num4 += 4;
			array3[num4++] = 0;
			array3[num4++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num4, num2 - 1);
				num4 += num2 - 1;
				array3[num4++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num4, num - 1);
				num4 += num - 1;
				array3[num4++] = 0;
			}
			_baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		/// <summary>
		///   Compress a string into a byte array using GZip.
		/// </summary>
		///
		/// <remarks>
		///   Uncompress it with <see cref="GZipStream.UncompressString(byte[])"/>.
		/// </remarks>
		///
		/// <seealso cref="GZipStream.UncompressString(byte[])"/>
		/// <seealso cref="GZipStream.CompressBuffer(byte[])"/>
		///
		/// <param name="s">
		///   A string to compress. The string will first be encoded
		///   using UTF8, then compressed.
		/// </param>
		///
		/// <returns>The string in compressed form</returns>

		public static byte[] CompressString(string s)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Compress a byte array into a new byte array using GZip.
		/// </summary>
		///
		/// <remarks>
		///   Uncompress it with <see cref="GZipStream.UncompressBuffer(byte[])"/>.
		/// </remarks>
		///
		/// <seealso cref="GZipStream.CompressString(string)"/>
		/// <seealso cref="GZipStream.UncompressBuffer(byte[])"/>
		///
		/// <param name="b">
		///   A buffer to compress.
		/// </param>
		///
		/// <returns>The data in compressed form</returns>
		public static byte[] CompressBuffer(byte[] b)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Uncompress a GZip'ed byte array into a single string.
		/// </summary>
		///
		/// <seealso cref="GZipStream.CompressString(String)"/>
		/// <seealso cref="GZipStream.UncompressBuffer(byte[])"/>
		///
		/// <param name="compressed">
		///   A buffer containing GZIP-compressed data.
		/// </param>
		///
		/// <returns>The uncompressed string</returns>
		public static string UncompressString(byte[] compressed)
		{
			using (MemoryStream stream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(stream, CompressionMode.Decompress);
				return ZlibBaseStream.UncompressString(compressed, decompressor);
			}
		}

		/// <summary>
		///   Uncompress a GZip'ed byte array into a byte array.
		/// </summary>
		///
		/// <seealso cref="GZipStream.CompressBuffer(byte[])"/>
		/// <seealso cref="GZipStream.UncompressString(byte[])"/>
		///
		/// <param name="compressed">
		///   A buffer containing data that has been compressed with GZip.
		/// </param>
		///
		/// <returns>The data in uncompressed form</returns>
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			using (MemoryStream stream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(stream, CompressionMode.Decompress);
				return ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
		}
	}
}
