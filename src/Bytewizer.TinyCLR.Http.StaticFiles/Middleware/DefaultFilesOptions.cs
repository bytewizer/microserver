using GHIElectronics.TinyCLR.IO;
using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for selecting default file names.
    /// </summary>
    public class DefaultFilesOptions
    {
        /// <summary>
        /// Configuration for the DefaultFilesMiddleware.
        /// </summary>
        public DefaultFilesOptions()
        {
            // Prioritized list
            DefaultFileNames = new ArrayList()
            {
                "index.html",
                "index.htm"
            };
        }

        /// <summary>
        /// Provides a drive mapping access for file streams.
        /// </summary>
        public IDriveProvider DriveProvider { get; set; }

        /// <summary>
        /// An ordered list of file names to select by default. List length and ordering may affect performance.
        /// </summary>
        public ArrayList DefaultFileNames { get; set; }
    }
}