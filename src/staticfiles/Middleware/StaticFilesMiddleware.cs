using System;
using System.IO;

using Bytewizer.TinyCLR.Sockets;

using GHIElectronics.TinyCLR.IO;

namespace Bytewizer.TinyCLR.Http
{
    public class StaticFilesMiddleware : Middleware
    {
        private readonly IDriveProvider _drive;
        private readonly MimeTypeProvider _mimeType;

        public StaticFilesMiddleware(IDriveProvider drive)
        {
            if (drive == null)
                throw new ArgumentNullException(nameof(drive));

            _mimeType = MimeTypeProvider.Instance;
            _drive = drive;
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;

            // only process GET requests
            if (request.Method == HttpMethods.Get)
            {
                var path = context.Request.Path;
                if (!string.IsNullOrEmpty(path))
                {
                    var filePath = path.Replace("/", Path.DirectorySeparatorChar.ToString()).Split('?')[0];
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (File.Exists(filePath))
                        {
                            var filename = Path.GetFileName(filePath);
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                var response = context.Response;
                                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                                response.Headers.ContentDisposition = $"inline; filename={filename}";
                                response.Headers.ContentType = _mimeType.Get(filename);
                                response.Headers.ContentLength = stream.Length;
                                response.Body = stream;

                                return;
                            }
                        }
                    }
                }
            }

            next(context);
        }
    }
}
