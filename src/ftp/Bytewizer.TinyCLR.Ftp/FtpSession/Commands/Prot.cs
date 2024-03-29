﻿using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// The <c>PROT</c> command handler.
        /// </summary>
        private void Prot()
        {
            try
            {
                var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));
             
                if (feature == null || feature.TlsBlockSize < 0)
                {
                    BadSequenceOfCommands();
                    return;
                }

                switch (_context.Request.Argument)
                {
                    case "C":
                        feature.TlsProt = "C";
                        _context.Response.Write(200, "PROT command successful.");
                        break;

                    case "P":
                        feature.TlsProt = "P";
                        _context.Response.Write(200, "PROT command successful.");
                        break;

                    case "S":
                    case "E":
                        ParameterNotImplemented();
                        break;

                    default:
                        ParameterNotRecognized();
                        break;
                }
            }
            catch
            {
                _context.Response.Write(500, "PROT command failed.");
            }
        }
    }
}