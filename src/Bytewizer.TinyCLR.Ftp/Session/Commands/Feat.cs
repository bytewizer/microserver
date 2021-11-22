using System.Text;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Feat()
        {
            //var lines = new string[] {
            //    "AUTH TLS;TLS-C", "MDTM", "MLST type*;size*;modify*;create*;",
            //    "PASV", "PBSZ", "PROT C;P;", "Rest Stream", "SIZE"
            //};

            //_context.Response.Write(211, "Extensions supported:", lines, "End");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("211-Extensions supported:");
            sb.AppendLine(" AUTH TLS;TLS-C");
            sb.AppendLine(" MDTM");
            sb.AppendLine(" MLST type*;size*;modify*;create*;");
            sb.AppendLine(" PASV");
            sb.AppendLine(" PBSZ");
            sb.AppendLine(" PROT C;P;");
            sb.AppendLine(" Rest Stream");
            sb.AppendLine(" SIZE");
            sb.AppendLine("211 End");

            _context.Response.Write(sb.ToString());

        }
    }
}