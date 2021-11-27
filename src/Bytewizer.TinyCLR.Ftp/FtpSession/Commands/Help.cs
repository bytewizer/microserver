using System.Text;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Help()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("214-The following commands are recognized.");
            sb.AppendLine(" APPE");
            sb.AppendLine(" AUTH");
            sb.AppendLine(" CDUP");
            sb.AppendLine(" CWD");
            sb.AppendLine(" DELE");
            sb.AppendLine(" FEAT");
            sb.AppendLine(" HELP");
            sb.AppendLine(" LIST");
            sb.AppendLine(" MDTM");
            sb.AppendLine(" MKD");
            sb.AppendLine(" MLSD");
            sb.AppendLine(" MLST");
            sb.AppendLine(" MODE");
            sb.AppendLine(" NOOP");
            sb.AppendLine(" OPTS");
            sb.AppendLine(" PASS");
            sb.AppendLine(" PASV");
            sb.AppendLine(" PBSZ");
            sb.AppendLine(" PORT");
            sb.AppendLine(" PROT");
            sb.AppendLine(" PWD");
            sb.AppendLine(" QUIT");
            sb.AppendLine(" RETR");
            sb.AppendLine(" RMD");
            sb.AppendLine(" RNFR");
            sb.AppendLine(" RNTO");
            sb.AppendLine(" SIZE");
            sb.AppendLine(" STOR");
            sb.AppendLine(" STRU");
            sb.AppendLine(" SYST");
            sb.AppendLine(" TYPE");
            sb.AppendLine(" USER");
            sb.AppendLine("214 HELP command successfull.");

            _context.Response.Write(sb.ToString());
        }
    }
}