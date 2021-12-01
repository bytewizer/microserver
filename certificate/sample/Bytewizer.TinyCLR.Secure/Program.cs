using Bytewizer.TinyCLR.Sockets;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Secure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServer server = new SocketServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseHttpResponse();
                });

            });
            server.Start();
        }
    }
}
