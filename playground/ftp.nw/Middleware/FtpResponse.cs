using System;
using System.IO;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.Playground.Ftp
{
    public class FtpResponse : Middleware
    {
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            var ctx = context as ISocketContext;
            

        }
    }
}

//try
//{
//    if (ctx.Channel.InputStream == null)
//        return;

//    using (var reader = new StreamReader(ctx.Channel.InputStream))
//    {
//        string line;
//        do
//        {
//            line = reader.ReadLine();
//            //Debug.WriteLine(line);
//        } while (!reader.EndOfStream);
//    }

//    string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" +
//                      "<doctype !html><html><head><meta http-equiv='refresh' content='1'><title>Hello, world!</title>" +
//                      "<style>body { background-color: #111 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
//                      "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

//    // send the response to browser
//    ctx.Channel.Write(response);
//}
//catch
//{
//    // try to manage all unhandled exceptions in the pipeline
//    Debug.WriteLine("Unhandled exception message in the pipeline");
//}