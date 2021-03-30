using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Netbios
{
    public class NetBios : Middleware
    {
        private readonly byte[] encodedName;
        private readonly byte[] ipAddress;

        public NetBios(string name, byte[] address)
        {
            encodedName = EncodeNetbiosName(name);
            ipAddress = address;
        }

        protected override void Invoke(IContext context, RequestDelegate next)
        {
            var ctx = context as ISocketContext;
            
            try
            {
                if (ctx.Channel.InputStream == null)
                    return;

                byte[] buffer = new byte[ctx.Channel.InputStream.Length];
                ctx.Channel.InputStream.Read(buffer, 0, buffer.Length);

                //byte[] localIpAddress = ctx.Channel.Connection.LocalIpAddress.GetAddressBytes();
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 137);

                if ((buffer[2] >> 3) == 0) // opcode == 0
                {
                    byte[] nbName = new byte[32];
                    Array.Copy(buffer, 13, nbName, 0, 32);
                    if (Equal(buffer, 13, encodedName, 0, 32))
                    {
                        byte[] outBuffer = new byte[62];
                        Array.Clear(outBuffer, 0, 62);
                        outBuffer[0] = buffer[0]; // trnid
                        outBuffer[1] = buffer[1]; // trnid
                        outBuffer[2] = 0x85;
                        outBuffer[7] = 0x01;
                        outBuffer[12] = 0x20;
                        for (int i = 0; i < 32; i++)
                        {
                            outBuffer[i + 13] = encodedName[i];
                        }
                        outBuffer[47] = 0x20; // RR_TYPE: NB
                        outBuffer[49] = 0x01; // RR_CLASS: IN
                        outBuffer[51] = 0x0f;
                        outBuffer[52] = 0x0f;
                        outBuffer[53] = 0x0f;
                        outBuffer[55] = 0x06; // RDLENGTH
                        outBuffer[56] = 0x60; // NB_FLAGS
                        outBuffer[58] = ipAddress[0];
                        outBuffer[59] = ipAddress[1];
                        outBuffer[60] = ipAddress[2];
                        outBuffer[61] = ipAddress[3];
                        ctx.Channel.SendTo(
                            outBuffer,
                            outBuffer.Length,
                            0,
                            SocketFlags.None,
                            remoteEndPoint
                        );
                    }
                }
            }
            catch
            {
                // try to manage all unhandled exceptions in the pipeline
                Debug.WriteLine("Unhandled exception message in the pipeline");
            }
        }
        
        private byte[] EncodeNetbiosName(string name)
        {
            string Name = name.ToUpper();
            byte[] result = new byte[32];
            char c;
            for (int i = 0; i < 15; i++)
            {
                c = i < Name.Length ? Name[i] : ' ';
                //c = Name[i];
                result[i * 2] = (byte)(((byte)(c) >> 4) + 65);
                result[(i * 2) + 1] = (byte)(((byte)(c) & 0x0f) + 65);
            }
            result[30] = 0x41;
            result[31] = 0x41;
            return result;
        }

        private static bool Equal(byte[] Array1, int Start1, byte[] Array2, int Start2, int Count)
        {
            bool result = true;
            for (int i = 0; i < Count - 1; i++)
            {
                if (Array1[i + Start1] != Array2[i + Start2])
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}