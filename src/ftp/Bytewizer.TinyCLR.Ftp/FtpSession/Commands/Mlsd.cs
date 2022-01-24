using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>MLSD</c> command.
        /// </summary>
        private void Mlsd()
        {
            //CommandNotImplemented();

            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return;
            }

            //var path = _context.Request.Command.Argument;

            try
            {
                //write to channel
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (Stream ns = GetNetworkStream())
                {
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        //_fileProvider.SetWorkingDirectory(path);
                        var path = _fileProvider.GetLocalDirectory();

                        DirectoryInfo directory = new DirectoryInfo(path);
                        if (directory.Exists)
                        {
                            sw.WriteLine($"Type=cdir;Modify=; {_fileProvider.GetWorkingDirectory()}");

                            if (_fileProvider.GetBaseDirectory()
                                != _fileProvider.GetLocalDirectory())
                            {
                                sw.WriteLine($"Type=pdir;Modify=;Create=; ..");
                            }
                        }

                        var enumerator = _fileProvider.EnumerateDirectories(path);

                        while (enumerator.MoveNext())
                        {
                            directory = new DirectoryInfo((string)enumerator.Current);
                            if (directory.Exists)
                            {
                                if ((directory.Attributes & FileAttributes.System) == FileAttributes.System)
                                {
                                    continue;
                                }

                                var lastWriteTime = directory.LastWriteTime.ToTimeString();
                                var creationTime = directory.LastWriteTime.ToTimeString();

                                sw.WriteLine($"Type=dir;Modify={lastWriteTime}; {directory.Name}");
                                //sw.Flush();
                            }
                        }

                        enumerator = _fileProvider.EnumerateFiles(path);

                        while (enumerator.MoveNext())
                        {
                            FileInfo file = new FileInfo((string)enumerator.Current);
                            if (file.Exists)
                            {
                                var lastWriteTime = file.LastWriteTime.ToTimeString();
                                var creationTime = file.LastWriteTime.ToTimeString();

                                sw.WriteLine($"Type=file;Size={file.Length};Modify={lastWriteTime}; {file.Name}");
                                //sw.Flush();
                            }
                        }
                    }
                }

                _context.Response.Write(226, "Closing data connection.");
            }
            catch
            {
                _context.Response.Write(500, "MLSD command failed.");
            }
        }
    }
}