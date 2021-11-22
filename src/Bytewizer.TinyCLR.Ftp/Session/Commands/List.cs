using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void List()
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
                        DirectoryInfo d = new DirectoryInfo((string)enumerator.Current);
                        string date = d.LastWriteTime.ToString("MMM dd HH:mm");
                        string line = "drwxr-xr-x    2 2003     2003     4096     " + date + " " + d.Name;
                        sw.WriteLine(line);
                        sw.Flush();
                    }

                    enumerator = _fileProvider.EnumerateFiles();

                    while (enumerator.MoveNext())
                    {
                        FileInfo f = new FileInfo((string)enumerator.Current);
                        string date = f.LastWriteTime.ToString("MMM dd HH:mm");
                        string line = "-rw-r--r--    2 2003     2003     " + f.Length + " " + date + "  " + f.Name;
                        sw.WriteLine(line);
                        sw.Flush();
                    }
                }
            }

            _context.Response.Write(223, "Transfer complete.");
        }
    }
}