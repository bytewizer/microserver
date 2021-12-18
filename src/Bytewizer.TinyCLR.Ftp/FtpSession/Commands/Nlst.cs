using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>LIST</c> commands.
        /// </summary>
        private void Nlst()
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

                using (Stream ns = GetNetworkStream())
                {
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        var enumerator = _fileProvider.EnumerateDirectories("");

                        while (enumerator.MoveNext())
                        {
                            DirectoryInfo directory = new DirectoryInfo((string)enumerator.Current);

                            if ((directory.Attributes & FileAttributes.System) == FileAttributes.System)
                            {
                                continue;
                            }

                            sw.WriteLine(directory.Name);
                            sw.Flush();
                        }

                        enumerator = _fileProvider.EnumerateFiles("");

                        while (enumerator.MoveNext())
                        {
                            FileInfo file = new FileInfo((string)enumerator.Current);
                            sw.WriteLine(file.Name);
                            sw.Flush();
                        }                
                    }
                }

                _context.Response.Write(223, "Transfer complete.");
            }
            catch
            {
                _context.Response.Write(500, "NLST command failed.");
            }
        }
    }
}