
using System;
using Microsoft.SPOT;

namespace MicroServer.Storage
{
	/// <summary>
	/// Flash storage class
	/// </summary>
	/// <remarks></remarks>
	public class FlashStorage : IStorage
	{

        private object SyncLock = null;

        #region Constructors / Deconstructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorage"/> class.
        /// </summary>
        public FlashStorage()
		{
			SyncLock = new object();
		}

        /// <summary>
        /// Handles object cleanup for GC finalization.
        /// </summary>
        ~FlashStorage()
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

            }
        }

        #endregion  Constructors / Deconstructors

        #region Methods
        
        
        public string Read()
        {
            return string.Empty;         
        }

        public void Write(string data)
        {

        }

        #endregion Methods

	}
}
