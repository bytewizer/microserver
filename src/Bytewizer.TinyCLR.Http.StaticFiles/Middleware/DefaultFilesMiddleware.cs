using System;
using System.IO;

using GHIElectronics.TinyCLR.IO;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Determines if there is a default file present and appends the default file path.
    /// </summary>
    public class DefaultFilesMiddleware : Middleware
    {
        private readonly DefaultFilesOptions _options;
        private readonly IDriveProvider _driveProvider;

        /// <summary>
        /// Initializes a default instance of the <see cref="DefaultFilesMiddleware"/> class.
        /// </summary>
        public DefaultFilesMiddleware()
        {
            _options = new DefaultFilesOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticFileMiddleware"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DefaultFilesOptions"/> used to configure the middleware.</param>
        public DefaultFilesMiddleware(DefaultFilesOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
            _driveProvider = _options.DriveProvider;
        }

        /// <summary>
        /// Processes a request to see if it matches a root directory, and if there are any files with the
        /// configured default names in that root directory.  If so this will append the corresponding file name to the request
        /// path for a later middleware to handle.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> that encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <param name="next">The next request handler to be executed.</param>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (ValidateMethod(context) && context.Request.Path == "/")
            {
                for (int matchIndex = 0; matchIndex < _options.DefaultFileNames.Count; matchIndex++)
                {
                    string defaultFile = (string)_options.DefaultFileNames[matchIndex];

                    var driveName = _driveProvider?.Name ?? string.Empty;
                    var fullPath = $@"{driveName}{defaultFile}";
                    var file = new FileInfo(fullPath);
                    
                    if (file.Exists)
                    {
                        context.Request.Path = context.Request.Path + defaultFile;
                    }
                }
            }

            next(context);
        }

        private static bool ValidateMethod(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get ||
                context.Request.Method == HttpMethods.Head)
            {
                return true;
            }

            return false;
        }
    }
}