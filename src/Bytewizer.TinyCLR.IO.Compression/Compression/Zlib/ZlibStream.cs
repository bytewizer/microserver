using System;
using System.IO;

namespace Bytewizer.TinyCLR.IO.Compression
{
	/// <summary>
	/// Represents a Zlib stream for compression or decompression.
	/// </summary>
	public class ZlibStream : Stream
	{
		internal ZlibBaseStream _baseStream;

		private bool _disposed;

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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
				}
				return _baseStream._stream.CanWrite;
			}
		}

		/// <summary>
		/// Reading this property always throws a <see cref="NotSupportedException"/>.
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
					return _baseStream._z.TotalBytesOut;
				}
				if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return _baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Create a <c>ZlibStream</c> using the specified <c>CompressionMode</c>.
		/// </summary>
		/// <param name="stream">The stream which will be read or written.</param>
		/// <param name="mode">Indicates whether the ZlibStream will compress or decompress.</param>
		public ZlibStream(Stream stream, CompressionMode mode)
			: this(stream, mode, CompressionLevel.Default, leaveOpen: false)
		{
		}

		/// <summary>
		///  Create a <c>ZlibStream</c> using the specified <c>CompressionMode</c> and
		///  the specified <c>CompressionLevel</c>.
		/// </summary>
		/// <param name="stream">The stream to be read or written while deflating or inflating.</param>
		/// <param name="mode">Indicates whether the ZlibStream will compress or decompress.</param>
		/// <param name="level">A tuning knob to trade speed for effectiveness.</param>
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level)
			: this(stream, mode, level, leaveOpen: false)
		{
		}

		/// <summary>
		/// Create a <c>ZlibStream</c> using the specified <c>CompressionMode</c>, and
		/// explicitly specify whether the captive stream should be left open after Deflation or Inflation.
		/// </summary>
		/// <param name="stream">The stream which will be read or written. This is called the "captive" stream in other places in this documentation.</param>
		/// <param name="mode">Indicates whether the ZlibStream will compress or decompress.</param>
		/// <param name="leaveOpen">true if the application would like the stream to remain open after inflation/deflation.</param>
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		/// <summary>
		/// Create a <c>ZlibStream</c> using the specified <c>CompressionMode</c>
		/// and the specified <c>CompressionLevel</c>, and explicitly specify
		/// whether the stream should be left open after Deflation or Inflation.
		/// </summary>
		/// <param name="stream">The stream which will be read or written.</param>
		/// <param name="mode">Indicates whether the ZlibStream will compress or decompress.</param>
		/// <param name="leaveOpen">true if the application would like the stream to remain open after inflation/deflation.</param>
		/// <param name="level">A tuning knob to trade speed for effectiveness. This parameter is effective only when mode is <c>CompressionMode.Compress</c>.</param>
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			_baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		/// <summary>
		/// Dispose the stream.
		/// </summary>
		/// <param name="disposing">Indicates whether the Dispose method was invoked by user code.</param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!_disposed)
				{
					if (disposing && _baseStream != null)
					{
						_baseStream.Close();
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
		/// Flush the stream.
		/// </summary>
		public override void Flush()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			_baseStream.Flush();
		}

		/// <summary>
		/// Indicates whether the stream can be written.
		/// </summary>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return _baseStream.Read(buffer, offset, count);
		}

		/// <summary>
		/// Calling this method always throws a <see cref="NotSupportedException"/>.
		/// </summary>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calling this method always throws a <see cref="NotSupportedException"/>.
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
				throw new ObjectDisposedException("ZlibStream");
			}
			_baseStream.Write(buffer, offset, count);
		}

		/// <summary>
		/// Compress a string into a byte array using ZLIB.
		/// </summary>
		/// <param name="s">A string to compress.  The string will first be encoded using UTF8, then compressed.</param>
		/// <returns>The string in compressed form</returns>
		public static byte[] CompressString(string s)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Compress a byte array into a new byte array using ZLIB.
		/// </summary>
		/// <param name="b">A buffer to compress.</param>
		/// <returns>The data in compressed form</returns>
		public static byte[] CompressBuffer(byte[] b)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Uncompress a ZLIB-compressed byte array into a single string.
		/// </summary>
		/// <param name="compressed">A buffer containing ZLIB-compressed data.</param>
		/// <returns>The uncompressed string</returns>
		public static string UncompressString(byte[] compressed)
		{
			using (MemoryStream stream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(stream, CompressionMode.Decompress);
				return ZlibBaseStream.UncompressString(compressed, decompressor);
			}
		}

		/// <summary>
		/// Uncompress a ZLIB-compressed byte array into a byte array.
		/// </summary>
		/// <param name="compressed">A buffer containing ZLIB-compressed data.</param>
		/// <returns>The uncompressed string</returns>
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			using (MemoryStream stream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(stream, CompressionMode.Decompress);
				return ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
		}
	}
}
