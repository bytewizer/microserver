using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>LIST</c> command.
        /// </summary>
        private void List()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return; 
            }
            
            // TODO: ListArguments to support server side filtering
            //var argument = new ListArguments(_context.Request.Command.Argument);

            try
            {
                //write to channel
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (Stream ns = GetNetworkStream())
                {
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        var enumerator = _fileProvider.EnumerateDirectories(""); //TODO: Allow argument paths

                        while (enumerator.MoveNext())
                        {
                            DirectoryInfo directory = new DirectoryInfo((string)enumerator.Current);
                            if (directory.Exists)
                            {
                                if ((directory.Attributes & FileAttributes.System) == FileAttributes.System)
                                {
                                    continue;
                                }

                                var ro = (directory.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;

                                sw.WriteLine(
                                    string.Format(
                                        "d{0}{0}{0}   1 owner   group {1,15} {2} {3}",
                                        ro ? "r-x" : "rwx",
                                        0,
                                        directory.LastWriteTime.ToString(
                                            directory.LastWriteTime.Year == DateTime.Now.Year ?
                                            "MMM dd HH:mm" : "MMM dd  yyyy"),
                                        directory.Name));

                                //sw.WriteLine(
                                //    string.Format(
                                //        "{0:MM-dd-yy  hh:mmtt}       {1,-14} {2}",
                                //        directory.LastWriteTime,
                                //        "<DIR>",
                                //        directory.Name));

                                sw.Flush();
                            }
                        }

                        enumerator = _fileProvider.EnumerateFiles(""); //TODO: Allow argument paths

                        while (enumerator.MoveNext())
                        {
                            FileInfo file = new FileInfo((string)enumerator.Current);
                            if (file.Exists)
                            {
                                var ro = ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);

                                sw.WriteLine(
                                    string.Format(
                                        "-{0}{0}{0}   1 owner   group {1,15} {2} {3}",
                                        ro ? "r-x" : "rwx",
                                        file.Length,
                                        file.LastWriteTime.ToString(
                                            file.LastWriteTime.Year == DateTime.Now.Year ?
                                            "MMM dd HH:mm" : "MMM dd  yyyy"),
                                        file.Name));

                                //sw.WriteLine(
                                //    string.Format(
                                //        "{0:MM-dd-yy  hh:mmtt} {1,20} {2}",
                                //        file.LastWriteTime,
                                //        file.Length,
                                //        file.Name));

                                sw.Flush();
                            }
                        }
                    }
                }

                _context.Response.Write(226, "Closing data connection.");
            }
            catch
            {
                _context.Response.Write(500, "LIST command failed.");
            }
        }
    }
}