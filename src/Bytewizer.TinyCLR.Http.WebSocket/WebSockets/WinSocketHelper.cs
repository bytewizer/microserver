using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public static class WebSocketHelper
    {
        public static void EntryPoint(HttpContext context)
        {
            bool active = true;

            var inputStream = context.Channel.InputStream;
            while (active)
            {
                if (inputStream != null & inputStream.Length > 0)
                {
                    var frame = WebSocketFrame.ReadFrame(inputStream, true);
                    if (frame.IsClose)
                    {
                        Debug.WriteLine("Client disconnected.");
                        active = false;
                        break;
                    }
                    else if (frame.IsText)
                    {
                        var receivedPayload = frame.PayloadData.ToArray();
                        var receivedString = Encoding.UTF8.GetString(receivedPayload);
                        
                        var responseString = $"ECHO: {receivedString}";
                        var responsePayload = new PayloadData(Encoding.UTF8.GetBytes(responseString));
                        var responseFrame = new WebSocketFrame(Fin.Final, Opcode.Text, responsePayload, false, false);
                        var responseData = responseFrame.ToArray();

                        inputStream.Write(responseData, 0, responseData.Length);
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
