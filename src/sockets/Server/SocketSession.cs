using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.TinyCLR.Sockets
{
    public class SocketSession
    {
        public Socket Socket { get; internal set; }

        public ConnectionInfo Connection { get; internal set; }

        public Stream InputStream { get; internal set; }

        //public Stream OutputStream { get; set; }

        public void Assign(Socket channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Socket = channel;
            InputStream = new NetworkStream(channel);
            Connection = ConnectionInfo.Set(channel);
        }

        public void Assign(Socket channel, X509Certificate certificate, SslProtocols allowedProtocols)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            var streamBuilder = new SslStreamBuilder(certificate, allowedProtocols);

            Socket = channel;
            InputStream = streamBuilder.Build(channel);
            Connection = ConnectionInfo.Set(channel);
        }

        public bool IsClosed(int timeoutMicroSeconds = 1)
        {
            if (Socket == null)
                return true;

            return Socket.Poll(timeoutMicroSeconds, SelectMode.SelectRead) && Socket.Available < 1;
        }

        public void Clear()
        {
            if (Socket != null)
            {
                Socket.Close();
            }

            if (InputStream != null)
            {
                InputStream.Close();
                InputStream = null;
            }

            //if (OutputStream != null)
            //{
            //    OutputStream.Close();
            //    OutputStream = null;
            //}

            Socket = null;
        }

        //public void Write()
        //{
        //    Write(OutputStream);
        //}

        public void Write(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var bytes = Encoding.UTF8.GetBytes(text);
            InputStream.Write(bytes, 0, bytes.Length);
        }

        public int Write(byte[] message)
        {
            if (message.Length <= 0)
            {
                throw new ArgumentNullException(nameof(message));
            }

            int bytesSent = 0;
            try
            {
                InputStream.Write(message, 0, message.Length);
                bytesSent += message.Length;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
            }
            return bytesSent;
        }

        public int Write(Stream message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            byte[] sendBuffer = new byte[1460];
            int bytesSent = 0;
            try
            {
                if (message.Length > 0)
                {
                    int sentBytes = 0;
                    message.Position = 0;
                    while ((sentBytes = message.Read(sendBuffer, 0, sendBuffer.Length)) > 0)
                    {
                        InputStream.Write(sendBuffer, 0, sentBytes);
                        bytesSent += sentBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
            }
            return bytesSent;
        }
    }
}