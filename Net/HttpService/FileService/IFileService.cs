using System;
using System.Collections;

namespace MicroServer.Net.Http.Files
{
    /// <summary>
    /// Serves files
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Get a file
        /// </summary>
        /// <param name="context">Context used to locate and return files</param>
        /// <remarks><c>true</c> if the file was attached to the response; otherwise false;</remarks>
        bool GetFile(FileContext context);

        /// <summary>
        /// Get a file
        /// </summary>
        /// <param name="context">Context used to return files</param>
        /// <param name="fullPath">Full path used to locate files</param>
        /// <remarks><c>true</c> if the file was attached to the response; otherwise false;</remarks>
        bool GetFile(FileContext context, string fullPath);

        /// <summary>
        /// Gets if the specified url corresponds to a directory serving files
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns></returns>
        bool IsDirectory(Uri uri);

        /// <summary>
        /// Get all files that exists in the specified directory
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns></returns>
        ArrayList GetFiles(Uri uri);

        /// <summary>
        /// Gets a list of all sub directores 
        /// </summary>
        /// <param name="uri">URI (as requested by the HTTP client) which should correspond to a directory.</param>
        /// <returns></returns>
        ArrayList GetDirectories(Uri uri);
    }
}