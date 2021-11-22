using System.Text;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Help()
        {
            //var lines = new string[] {
            //    "APPE", "AUTH", "CDUP", "CWD", "CD", "DELE", "EPSV",
            //    "FEAT", "HELP", "LIST", "MDTM", "MKD", "MLSD", "MLST",
            //    "NLST", "NOOP", "PASS", "PASV", "PBSZ", "PORT", "PROT",
            //    "PWD", "QUIT", "REST", "RETR", "RMD", "RNFR", "RNTO",
            //    "SIZE", "STOR", "SYST", "TYPE", "USER", "XCUP","XCWD",
            //    "XMKD", "XPWD", "XRMD" 
            //};

            //_context.Response.Write(214, "The following commands are recognized.", lines, "HELP command successfull.");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("214-The following commands are recognized.");
            sb.AppendLine(" APPE");
            sb.AppendLine(" AUTH");
            sb.AppendLine(" CDUP");
            sb.AppendLine(" CWD");
            sb.AppendLine(" CD");
            sb.AppendLine(" DELE");
            sb.AppendLine(" EPSV");
            sb.AppendLine(" FEAT");
            sb.AppendLine(" HELP");
            sb.AppendLine(" LIST");
            sb.AppendLine(" MDTM");
            sb.AppendLine(" MKD");
            sb.AppendLine(" MLSD");
            sb.AppendLine(" MLST");
            sb.AppendLine(" NLST");
            sb.AppendLine(" NOOP");
            sb.AppendLine(" PASS");
            sb.AppendLine(" PASV");
            sb.AppendLine(" PBSZ");
            sb.AppendLine(" PORT");
            sb.AppendLine(" PROT");
            sb.AppendLine(" PWD");
            sb.AppendLine(" QUIT");
            sb.AppendLine(" REST");
            sb.AppendLine(" RETR");
            sb.AppendLine(" RMD");
            sb.AppendLine(" RNFR");
            sb.AppendLine(" RNTO");
            sb.AppendLine(" SIZE");
            sb.AppendLine(" STOR");
            sb.AppendLine(" SYST");
            sb.AppendLine(" TYPE");
            sb.AppendLine(" USER");
            sb.AppendLine(" XCUP");
            sb.AppendLine(" XCWD");
            sb.AppendLine(" XMKD");
            sb.AppendLine(" XPWD");
            sb.AppendLine(" XRMD");
            sb.AppendLine("214 HELP command successfull.");

            _context.Response.Write(sb.ToString());
        }
    }
}