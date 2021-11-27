using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Mlsd()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return;
            }

            try
            {
                //write to channel
                _context.Channel.Write(150, "Status okay, opening data connection.");
                using (NetworkStream ns = GetNetworkStream())
                {
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        var enumerator = _fileProvider.EnumerateDirectories();
                        var remotePath = _fileProvider.GetWorkingDirectory();

                        sw.WriteLine($"250-Listing {_context.Request.Command.Argument}");

                        while (enumerator.MoveNext())
                        {
                            DirectoryInfo directory = new DirectoryInfo((string)enumerator.Current);
                            var lastWriteTime = directory.LastWriteTime.ToUniversalTime().ToTimeString();
                            var creationTime = directory.LastWriteTime.ToUniversalTime().ToTimeString();

                            sw.WriteLine($" Type=dir;Modify={lastWriteTime};Create={creationTime}; {directory.Name}");
                            sw.Flush();
                        }

                        enumerator = _fileProvider.EnumerateFiles();

                        while (enumerator.MoveNext())
                        {
                            FileInfo file = new FileInfo((string)enumerator.Current);
                            var lastWriteTime = file.LastWriteTime.ToUniversalTime().ToTimeString();
                            var creationTime = file.LastWriteTime.ToUniversalTime().ToTimeString();

                            sw.WriteLine($" Type=file;Size={file.Length};Modify={lastWriteTime};Create={creationTime}; {file.Name}");
                            sw.Flush();
                        }

                        sw.WriteLine("250 End");
                        sw.Flush();
                    }
                }

                _context.Response.Write(223, "Transfer complete.");
            }
            catch
            {
                _context.Response.Write(500, "LIST command failed.");
            }
        }
    }
}