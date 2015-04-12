using System;
using System.Collections;

namespace MicroServer.Net.Http.Files
{
    /// <summary>
    /// Can serve files from multiple services.
    /// </summary>
    public class CompositeFileService : IFileService
    {
        private readonly IFileService[] _fileServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeFileService"/> class.
        /// </summary>
        /// <param name="fileServices">One or more file services.</param>
        public CompositeFileService(params IFileService[] fileServices)
        {
            _fileServices = fileServices;
        }

        #region IFileService Members

        /// <summary>
        /// Loops through all services and returns the first matching file.
        /// </summary>
        /// <param name="context">Context used to locate and return files</param>
        public virtual bool GetFile(FileContext context)
        {
            foreach (IFileService fileService in _fileServices)
            {
                fileService.GetFile(context);
                if (context.IsFound)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Loops through all services and returns the first matching file.
        /// </summary>
        /// <param name="context">Context used to return files</param>
        /// <param name="fullPath">Full path used to locate files</param>
        public virtual bool GetFile(FileContext context, string fullPath)
        {
            foreach (IFileService fileService in _fileServices)
            {
                fileService.GetFile(context);
                if (context.IsFound)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets if the specified url corresponds to a directory serving files
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns></returns>
        public bool IsDirectory(Uri uri)
        {
            foreach (IFileService fileService in _fileServices)
            {
                if (fileService.IsDirectory(uri))
                    return true;
            }
           
            return false;
        }

        /// <summary>
        /// Get all files that exists in the specified directory
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns></returns>
        /// <remarks>Will return all matching files from all services.</remarks>
        public ArrayList GetFiles(Uri uri)
        {
            //return _fileServices.SelectMany(service => service.GetFiles(uri));
            ArrayList results = new ArrayList();

            foreach (IFileService fileService in _fileServices)
            {
                results.Add(fileService.GetFiles(uri));
            }
            return results;
        }

        /// <summary>
        /// Gets a list of all sub directores
        /// </summary>
        /// <param name="uri">URI (as requested by the HTTP client) which should correspond to a directory.</param>
        /// <returns></returns>
        /// <remarks>Will return all matching directories from all inner services.</remarks>
        public ArrayList GetDirectories(Uri uri)
        {
            //return _fileServices.SelectMany(service => service.GetDirectories(uri));
            ArrayList results = new ArrayList();

            foreach (IFileService fileService in _fileServices)
            {
                results.Add(fileService.GetDirectories(uri));
            }
            return results;
        }

        #endregion
    }
}
