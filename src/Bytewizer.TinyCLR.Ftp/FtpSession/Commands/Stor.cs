﻿using System.IO;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Stor()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return;
            }

            var path = _context.Request.Command.Argument;

            var feature = (SessionFeature)_context.Features.Get(typeof(ISessionFeature));
            var marker = feature.RestMarker;

            try
            {
                // write to channel 
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (NetworkStream ns = GetNetworkStream())
                {
                    FileMode mode;
                    if (marker == 0)
                    {
                        mode = FileMode.Create;
                    }
                    else
                    {
                        mode = FileMode.Open;
                    }

                    using (FileStream fileStream = _fileProvider.OpenFileForWrite(path, mode))
                    {
                        ns.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }

                _context.Response.Write(223, "Transfer complete.");

            }
            catch
            {
                _context.Response.Write(500, "STOR command failed.");
            }
        }
    }
}