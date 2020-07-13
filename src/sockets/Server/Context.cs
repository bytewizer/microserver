using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.Sockets
{
    public class Context
    {
        public Context() { }

        public Socket Channel { get; internal set; }
        
        public Stream InputStream { get; internal set; }
        
        public Stream OutputStream { get; internal set; }

        public bool IsConnected { get; private set; } = false;

        public void Assign(Socket channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Channel = channel;
            InputStream = new NetworkStream(channel);
            IsConnected = true;
        }

        public void Assign(Socket channel, X509Certificate certificate, SslProtocols allowedProtocols)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            var streamBuilder = new SslStreamBuilder(certificate, allowedProtocols);

            Channel = channel;
            InputStream = streamBuilder.Build(channel);
            IsConnected = true;
        }

        public void Close()
        {
            IsConnected = false;
            
            if (Channel != null)
            {
                Channel.Close();
            }

            if (InputStream != null)
            {
                InputStream.Close();
            }
            
            if (OutputStream != null)
            {
                OutputStream.Close();
            }
        }

        public void Send(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var bytes = Encoding.UTF8.GetBytes(text);
            InputStream.Write(bytes, 0, bytes.Length);
        }

        public int Send(byte[] message)
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

        public int Send(Stream message)
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