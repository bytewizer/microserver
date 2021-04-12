using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.RegularExpressions;

using GHIElectronics.TinyCLR.Cryptography;
using Bytewizer.TinyCLR.Http.WebSockets;

namespace Bytewizer.TinyCLR.Http
{
    public class WebSocketServer
    {
        private Thread _thread;
        private Socket _listener;

        private bool _active = false;
        private readonly ManualResetEvent _acceptEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _startedEvent = new ManualResetEvent(false);

        public void Start()
        {
            // Don't return until thread that calls Accept is ready to listen
            _startedEvent.Reset();

            // create the socket listener
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // bind the listening socket to the port
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 80);
            _listener.Bind(endPoint);

            // start listening
            _listener.Listen(5);

            _thread = new Thread(() =>
            {
                _active = true;
                AcceptConnections();
            });
            _thread.Priority = ThreadPriority.AboveNormal;
            _thread.Start();

            // Waits for thread that calls Accept() to start
            _startedEvent.WaitOne();

            Debug.WriteLine($"Started socket listener");
        }

        public void Stop()
        {
            _active = false;

            // Signal the accept thread to continue
            _acceptEvent.Set();

            // Wait for thread to exit 
            _thread.Join();
            _thread = null;

            _listener.Close();
            _listener = null;

            Debug.WriteLine("Stopped socket listener");
        }

        private void AcceptConnections()
        {
            // Set the started event to signaled state
            _startedEvent.Set();

            while (_active)
            {
                // Set the accept event to nonsignaled state
                _acceptEvent.Reset();

                Debug.WriteLine("Waiting for a connection...");
                using (var remoteSocket = _listener.Accept())
                {
                    // Set the accept event to signaled state
                    _acceptEvent.Set();

                    // Send response to client
                    WebSocket(remoteSocket);

                    // Close connection
                    remoteSocket.Close();
                }

                // Wait until a connection is made before continuing
                _acceptEvent.WaitOne();
            }

            Debug.WriteLine("Exited AcceptConnection()");
        }

        private void WebSocket(Socket socket)
        {
            bool _active;

            using (NetworkStream stream = new NetworkStream(socket))
            {
                _active = true;

                var receivedData = new byte[1000000];
                var receivedDataLength = stream.Read(receivedData, 0, receivedData.Length);

                var requestString = Encoding.UTF8.GetString(receivedData, 0, receivedDataLength);

                if (new Regex("^GET").IsMatch(requestString))
                {
                    const string eol = "\r\n";

                    var receivedWebSocketKey = new Regex("Sec-WebSocket-Key: (.*)").Match(requestString).Groups[1].Value.Trim();
                    var keyHash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(receivedWebSocketKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"));

                    var response = "HTTP/1.1 101 Switching Protocols" + eol;
                    response += "Connection: Upgrade" + eol;
                    response += "Upgrade: websocket" + eol;
                    response += "Sec-WebSocket-Accept: " + Convert.ToBase64String(keyHash) + eol;
                    response += eol;

                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);

                    while (_active)
                    {
                        var frame = WebSocketFrame.ReadFrame(stream, true);
                        if (frame.IsClose)
                        {
                            Debug.WriteLine("Client disconnected.");
                            _active = false;
                            break;
                        }
                        else
                        {
                            Debug.WriteLine(frame.PrintToString());
                            var receivedPayload = frame.PayloadData.ToArray();
                            //var receivedPayload = ParsePayloadFromFrame(receivedData);
                            var receivedString = Encoding.UTF8.GetString(receivedPayload);

                            Debug.WriteLine($"Client: {receivedString}");

                            var response2 = $"ECHO: {receivedString}";

                            var payload = new PayloadData(Encoding.UTF8.GetBytes(response2));
                            var responseFrame = WebSocketFrame.CreatePongFrame(payload, false);
                            Debug.WriteLine(responseFrame.PrintToString());
                            var dataToSend = responseFrame.ToArray();
                            //var dataToSend = CreateFrameFromString(response2);

                            Debug.WriteLine($"Server: {response2}");

                            stream.Write(dataToSend, 0, dataToSend.Length);
                        }

                        //receivedData = new byte[1000000];
                        //var numBytesRead = stream.Read(receivedData, 0, receivedData.Length);
                        //if (numBytesRead > 0)
                        //{
                        //    if ((receivedData[0] & (byte)Opcode.CloseConnection) == (byte)Opcode.CloseConnection)
                        //    {
                        //        // Close connection request.
                        //        Debug.WriteLine("Client disconnected.");
                        //        _active = false;
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        var frame = WebSocketFrame.ReadFrame(stream, true);
                        //        Debug.WriteLine(frame.PrintToString());

                        //        var receivedPayload = frame.PayloadData.ToArray();
                        //        //var receivedPayload = ParsePayloadFromFrame(receivedData);
                        //        var receivedString = Encoding.UTF8.GetString(receivedPayload);

                        //        Debug.WriteLine($"Client: {receivedString}");

                        //        var response2 = $"ECHO: {receivedString}";
                        //        var dataToSend = CreateFrameFromString(response2);

                        //        Debug.WriteLine($"Server: {response2}");

                        //        stream.Write(dataToSend, 0, dataToSend.Length);
                        //    }
                        //}

                        Thread.Sleep(100);
                    }
                }
            }
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

        public static byte[] ParsePayloadFromFrame(byte[] incomingFrameBytes)
        {
            var payloadLength = 0L;
            var totalLength = 0L;
            var keyStartIndex = 0L;

            // 125 or less.
            // When it's below 126, second byte is the payload length.
            if ((incomingFrameBytes[1] & 0x7F) < 126)
            {
                payloadLength = incomingFrameBytes[1] & 0x7F;
                keyStartIndex = 2;
                totalLength = payloadLength + 6;
            }

            // 126-65535.
            // When it's 126, the payload length is in the following two bytes
            if ((incomingFrameBytes[1] & 0x7F) == 126)
            {
                payloadLength = BitConverter.ToInt16(new[] { incomingFrameBytes[3], incomingFrameBytes[2] }, 0);
                keyStartIndex = 4;
                totalLength = payloadLength + 8;
            }

            // 65536 +
            // When it's 127, the payload length is in the following 8 bytes.
            if ((incomingFrameBytes[1] & 0x7F) == 127)
            {
                payloadLength = BitConverter.ToInt64(new[] { incomingFrameBytes[9], incomingFrameBytes[8], incomingFrameBytes[7], incomingFrameBytes[6], incomingFrameBytes[5], incomingFrameBytes[4], incomingFrameBytes[3], incomingFrameBytes[2] }, 0);
                keyStartIndex = 10;
                totalLength = payloadLength + 14;
            }

            if (totalLength > incomingFrameBytes.Length)
            {
                throw new Exception("The buffer length is smaller than the data length.");
            }

            var payloadStartIndex = keyStartIndex + 4;

            byte[] key = { incomingFrameBytes[keyStartIndex], incomingFrameBytes[keyStartIndex + 1], incomingFrameBytes[keyStartIndex + 2], incomingFrameBytes[keyStartIndex + 3] };

            var payload = new byte[payloadLength];
            Array.Copy(incomingFrameBytes, (int)payloadStartIndex, payload, 0, (int)payloadLength);
            for (int i = 0; i < payload.Length; i++)
            {
                payload[i] = (byte)(payload[i] ^ key[i % 4]);
            }

            return payload;
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


//byte[] data = new byte[1024];
//int numBytesRead = stream.Read(data, 0, data.Length);
//if (numBytesRead > 0)
//{
//    string str = Encoding.UTF8.GetString(data, 0, numBytesRead);
//    Debug.WriteLine(str);
//}

//receivedData = new byte[1000000];
//socket.Receive(receivedData);

//if ((receivedData[0] & (byte)Opcode.CloseConnection) == (byte)Opcode.CloseConnection)
//{
//    // Close connection request.
//    Debug.WriteLine("Client disconnected.");
//    socket.Close();
//    break;
//}
//else
//{
//    var receivedPayload = ParsePayloadFromFrame(receivedData);
//    var receivedString = Encoding.UTF8.GetString(receivedPayload);

//    Debug.WriteLine($"Client: {receivedString}");

//    var response2 = $"ECHO: {receivedString}";
//    var dataToSend = CreateFrameFromString(response2);

//    Debug.WriteLine($"Server: {response}");

//    socket.Send(dataToSend);


