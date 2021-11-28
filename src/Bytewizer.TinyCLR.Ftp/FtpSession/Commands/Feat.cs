using System.Text;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>FEAT</c> command.
        /// </summary>
        private void Feat()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("211-Extensions supported:");
            sb.AppendLine(" AUTH TLS;TLS-C");
            sb.AppendLine(" UTF8");
            sb.AppendLine(" EPSV");
            sb.AppendLine(" EPRT");
            sb.AppendLine(" PASV");
            sb.AppendLine(" PBSZ");
            sb.AppendLine(" MLST type*;size*;modify*;create*;");
            sb.AppendLine(" PROT C;P;");
            sb.AppendLine(" MLSD");
            sb.AppendLine(" MDTM");
            sb.AppendLine(" SIZE");
            sb.AppendLine(" REST STREAM");
            sb.AppendLine("211 End");

            _context.Response.Write(sb.ToString());
        }
    }
}