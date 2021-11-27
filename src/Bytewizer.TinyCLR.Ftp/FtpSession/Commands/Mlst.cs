using System.IO;
using System.Collections;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Mlst()
        {
            var lines = new ArrayList();
            var enumerator = _fileProvider.EnumerateDirectories();

            while (enumerator.MoveNext())
            {
                DirectoryInfo directory = new DirectoryInfo((string)enumerator.Current);
                var lastWriteTime = directory.LastWriteTime.ToUniversalTime().ToTimeString();
                var creationTime = directory.LastWriteTime.ToUniversalTime().ToTimeString();

                lines.Add($"Type=dir;Modify={lastWriteTime};Create={creationTime}; {directory.Name}");
            }

            enumerator = _fileProvider.EnumerateFiles();

            while (enumerator.MoveNext())
            {
                FileInfo file = new FileInfo((string)enumerator.Current);
                var lastWriteTime = file.LastWriteTime.ToUniversalTime().ToTimeString();
                var creationTime = file.LastWriteTime.ToUniversalTime().ToTimeString();

                lines.Add($"Type=dir;Size={file.Length};Modify={lastWriteTime};Create={creationTime}; {file.Name}");
            }

            _context.Response.Write(250, $"Listing {_context.Request.Command.Argument}", lines, "End");
        }
    }
}