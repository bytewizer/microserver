using GHIElectronics.TinyCLR.IO;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for the <see cref="StaticFileMiddleware"/>.
    /// </summary>
    public class StaticFileOptions
    {
        /// <summary>
        /// Provides a drive mapping access for file streams.
        /// </summary>
        public IDriveProvider DriveProvider { get; set; }
        
        /// <summary>
        /// Used to map files to content-types.
        /// </summary>
        public IContentTypeProvider ContentTypeProvider { get; set; }

        /// <summary>
        /// The default content type for a request if the ContentTypeProvider cannot determine one.
        /// None is provided by default, so the client must determine the format themselves.
        /// http://www.w3.org/Protocols/rfc2616/rfc2616-sec7.html#sec7
        /// </summary>
        public string DefaultContentType { get; set; }

        /// <summary>
        /// If the file is not a recognized content-type should it be served.
        /// Default: false.
        /// </summary>
        public bool ServeUnknownFileTypes { get; set; }
    }
}