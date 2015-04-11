
using System;
using System.IO;

namespace MicroServer.Storage
{
	/// <summary>
	/// File storage class
	/// </summary>
	/// <remarks></remarks>
	public class FileStorage : IStorage
	{
        private object SyncLock = null;
        private string _filePath = string.Empty;
        private StreamWriter _streamWriter = null;
        private StreamReader _streamReader = null;
        
        #region Constructors / Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorage"/> class.
        /// </summary>
        public FileStorage(string filePath)
		{
			SyncLock = new object();
            _filePath = filePath;
			_streamWriter = null;
            _streamReader = null;
		}

        /// <summary>
        /// Handles object cleanup for GC finalization.
        /// </summary>
        ~FileStorage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Handles object cleanup.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles object cleanup
        /// </summary>
        /// <param name="disposing">True if called from Dispose(); false if called from GC finalization.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_streamWriter != null)
				{
					_streamWriter.Flush();
					_streamWriter.Close();
					_streamWriter.Dispose();
				}
            }
        }

        #endregion  Constructors / Deconstructors

        #region Methods

        public void Write(string data)
        {
            _streamWriter = new StreamWriter(File.Create(_filePath));

            lock (SyncLock)
            {
                if (_streamWriter != null)
                {                
                    _streamWriter.Write(data);
                    _streamWriter.Flush();
                }
            }
        }

        public string Read()
        {
            _streamReader = new StreamReader(File.Open(_filePath, FileMode.Open));

            lock (SyncLock)
            {
             
                if (_streamReader != null)
                {
                    return _streamReader.ReadToEnd();
                }

            }
            return string.Empty;
        }

        #endregion Methods
	}
}
