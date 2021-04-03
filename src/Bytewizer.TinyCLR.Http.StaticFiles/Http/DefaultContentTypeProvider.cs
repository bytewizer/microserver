using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Provides a default mapping between file extensions and MIME types.
    /// </summary>
    public class DefaultContentTypeProvider : IContentTypeProvider
    {
        #region Extension mapping table
        /// <summary>
        /// Creates a new provider with a set of default mappings.
        /// </summary>
        public DefaultContentTypeProvider()
            : this(new Hashtable()
            {
                { ".css", "text/css" },
                { ".gif", "image/gif" },
                { ".gz", "application/x-gzip" },
                { ".htm", "text/html" },
                { ".html", "text/html" },
                { ".ico", "image/x-icon" },
                { ".jpeg", "image/jpeg" },
                { ".jpg", "image/jpeg" },
                { ".js", "application/javascript" },
                { ".json", "application/json" },
                { ".png", "image/png" },
                { ".woff", "application/font-woff" },
                { ".woff2", "font/woff2" },
            })
            {
        }
        #endregion

        /// <summary>
        /// Creates a lookup engine using the provided mapping.
        /// </summary>
        /// <param name="mapping">The cross reference table of file extensions and content-types. 
        /// Added types must be in all lowercase to properly match.</param>
        public DefaultContentTypeProvider(Hashtable mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }
            Mappings = mapping;
        }

        /// <summary>
        /// The cross reference table of file extensions and content-types. Added types must be in all lowercase to properly match.
        /// </summary>
        public Hashtable Mappings { get; private set; }

        /// <summary>
        /// Given a file path, determine the MIME type
        /// </summary>
        /// <param name="subpath">A file path</param>
        /// <param name="contentType">The resulting MIME type</param>
        /// <returns>True if MIME type could be determined</returns>
        public bool TryGetContentType(string subpath, out string contentType)
        {
            var extension = GetExtension(subpath);
            if (extension == null)
            {
                contentType = null;
                return false;
            }
            return TryGetValue(extension, out contentType);
        }

        private bool TryGetValue(string key, out string obj)
        {
            obj = default;

            if (Mappings == null)
            {
                return false;
            }

            var searchKey = key.ToLower();
            if (Mappings.Contains(searchKey))
            {
                obj = (string)Mappings[searchKey];
                return true;
            }

            return false;
        }

        private static string GetExtension(string path)
        {
            // Don't use Path.GetExtension as that may throw an exception if there are
            // invalid characters in the path. Invalid characters should be handled
            // by the FileProviders

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            int index = path.LastIndexOf('.');
            if (index < 0)
            {
                return null;
            }

            return path.Substring(index);
        }
    }
}