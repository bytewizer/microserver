﻿using System;
using System.IO;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.Playground.Sockets
{
    public class HttpResponse : Middleware
    {
        private long counter = 1;
      
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            var ctx = context as ISocketContext;
            
            try
            {
                if (ctx.Channel.InputStream == null)
                    return;

                Debug.WriteLine($"Response: # {counter++}");

                using (var reader = new StreamReader(ctx.Channel.InputStream))
                {
                    // read the context input stream (required or browser will stall the request)
                    while (reader.Peek() != -1)
                    {
                        var line = reader.ReadLine();
                        Debug.WriteLine(line);
                    }
                }

                string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\nConnection: Close\r\n\r\n" +
                                  "<doctype !html><html><head><meta http-equiv='refresh' content='5'><title>Hello, world!</title>" +
                                  "<style>body { background-color: #111 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                  "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                // send the response to browser
                ctx.Channel.Write(response);
            }
            catch (Exception ex)
            {
                // try to manage all unhandled exceptions in the pipeline
                Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
            }
            finally
            {
                // clear the connection once all data is sent (only after the last send)
                ctx.Channel.Clear();
            }
        }
    }
}