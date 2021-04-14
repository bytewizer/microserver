
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.WebSockets;


namespace Bytewizer.Playground.WebSocket
{
    class Program
    {
        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseWebSockets();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            var websocket = context.GetWebSocket();

                            context.Response.SendFile(@"\websocket.html", "text/html");
                        });

                        endpoints.MapHubs();

                        //endpoints.Map("/chat", context =>
                        //{
                        //    WebSocketHelper.EntryPoint(context, typeof(ChatHub));
                        //});
                       
                    });
                });
            });
            server.Start();
        }


        public enum Opcode
        {
            Fragment = 0,
            Text = 1,
            Binary = 2,
            CloseConnection = 8,
            Ping = 9,
            Pong = 10
        }

        public static byte[] CreateFrameFromString(string message, Opcode opcode = Opcode.Text)
        {
            var payload = Encoding.UTF8.GetBytes(message);

            byte[] frame;

            if (payload.Length < 126)
            {
                frame = new byte[1 /*op code*/ + 1 /*payload length*/ + payload.Length /*payload bytes*/];
                frame[1] = (byte)payload.Length;
                Array.Copy(payload, 0, frame, 2, payload.Length);
            }
            else if (payload.Length >= 126 && payload.Length <= 65535)
            {
                frame = new byte[1 /*op code*/ + 1 /*payload length option*/ + 2 /*payload length*/ + payload.Length /*payload bytes*/];
                frame[1] = 126;
                frame[2] = (byte)((payload.Length >> 8) & 255);
                frame[3] = (byte)(payload.Length & 255);
                Array.Copy(payload, 0, frame, 4, payload.Length);
            }
            else
            {
                frame = new byte[1 /*op code*/ + 1 /*payload length option*/ + 8 /*payload length*/ + payload.Length /*payload bytes*/];
                frame[1] = 127; // <-- Indicates that payload length is in following 8 bytes.
                frame[2] = (byte)((payload.Length >> 56) & 255);
                frame[3] = (byte)((payload.Length >> 48) & 255);
                frame[4] = (byte)((payload.Length >> 40) & 255);
                frame[5] = (byte)((payload.Length >> 32) & 255);
                frame[6] = (byte)((payload.Length >> 24) & 255);
                frame[7] = (byte)((payload.Length >> 16) & 255);
                frame[8] = (byte)((payload.Length >> 8) & 255);
                frame[9] = (byte)(payload.Length & 255);
                Array.Copy(payload, 0, frame, 10, payload.Length);
            }

            frame[0] = (byte)((byte)opcode | 0x80 /*FIN bit*/);

            return frame;
        }
    }
}