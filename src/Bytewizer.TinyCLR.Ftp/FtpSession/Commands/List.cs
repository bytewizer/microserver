﻿using System;
using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void List()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return; 
            }
            
            //var argument = new ListArguments(_context.Request.Command.Argument);

            try
            {
                //write to channel
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (NetworkStream ns = GetNetworkStream())
                {
                    using (StreamWriter sw = new StreamWriter(ns))
                    {
                        var enumerator = _fileProvider.EnumerateDirectories();

                        while (enumerator.MoveNext())
                        {
                            DirectoryInfo directory = new DirectoryInfo((string)enumerator.Current);
                            string date = directory.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180)
                                ? directory.LastWriteTime.ToString("MMM dd  yyyy")
                                : directory.LastWriteTime.ToString("MMM dd HH:mm");

                            sw.WriteLine($"drwxr-xr-x    2 2003     2003     4096 {date} {directory.Name}");
                            sw.Flush();
                        }

                        enumerator = _fileProvider.EnumerateFiles();

                        while (enumerator.MoveNext())
                        {
                            FileInfo file = new FileInfo((string)enumerator.Current);
                            string date = file.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180)
                                ? file.LastWriteTime.ToString("MMM dd  yyyy")
                                : file.LastWriteTime.ToString("MMM dd HH:mm");

                            sw.WriteLine($"-rw-r--r--    2 2003     2003     {file.Length} {date} {file.Name}");
                            sw.Flush();
                        }
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