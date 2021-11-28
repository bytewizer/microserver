using System.IO;
using System.Collections;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>MLST</c> command.
        /// </summary>
        private void Mlst()
        {
            var lines = new ArrayList();

            try
            {
                var enumerator = _fileProvider.EnumerateDirectories("");

                while (enumerator.MoveNext())
                {
                    DirectoryInfo directory = new DirectoryInfo((string)enumerator.Current);
                    if (directory.Exists)
                    {
                        if ((directory.Attributes & FileAttributes.System) == FileAttributes.System)
                        {
                            continue;
                        }

                        var lastWriteTime = directory.LastWriteTime.ToTimeString();
                        var creationTime = directory.LastWriteTime.ToTimeString();

                        lines.Add($"Type=dir;Modify={lastWriteTime};Create={creationTime}; {directory.Name}");
                    }
                }

                enumerator = _fileProvider.EnumerateFiles("");

                while (enumerator.MoveNext())
                {
                    FileInfo file = new FileInfo((string)enumerator.Current);
                    if (file.Exists)
                    {
                        var lastWriteTime = file.LastWriteTime.ToTimeString();
                        var creationTime = file.LastWriteTime.ToTimeString();

                        lines.Add($"Type=dir;Size={file.Length};Modify={lastWriteTime};Create={creationTime}; {file.Name}");
                    }

                    _context.Response.Write(250, $"Listing {_context.Request.Command.Argument}", lines, "End");
                }
            }
            catch
            {
                _context.Response.Write(500, "MLST command failed.");
            }
        }
    }
}