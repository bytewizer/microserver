using System;
using System.IO;
using System.Collections;

using MicroServer.Net.Sockets;

namespace MicroServer.Net.Http.Messages
{
    /// <summary>
    ///     Used to encode request/response into a byte stream.
    /// </summary>
    public class HttpMessageEncoder : IMessageEncoder
    {
        private HttpMessage _message;
        private readonly MemoryStream _stream;
        private readonly StreamWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageEncoder"/> class.
        /// </summary>
        public HttpMessageEncoder()
        {
            _stream = new MemoryStream(); 
            _stream.SetLength(0);
            _writer = new StreamWriter(_stream);
        }

        /// <summary>
        ///     Are about to send a new message
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <remarks>
        ///     Can be used to prepare the next message. for instance serialize it etc.
        /// </remarks>
        /// <exception cref="NotSupportedException">Message is of a type that the encoder cannot handle.</exception>
        public Stream Prepare(object message)
        {
            if (!(message is HttpMessage))
                throw new InvalidOperationException("This encoder only supports messages deriving from 'HttpMessage'");

            _message = (HttpMessage)message;

            if (_message.Body == null || _message.Body.Length == 0)
            {
                _message.Headers["Content-Length"] = "0";
            }
            else
            {
                _message.ContentLength = (int)_message.Body.Length;
            }

            _writer.WriteLine(_message.StatusLine);

            foreach (DictionaryEntry header in _message.Headers)
            {
                _writer.Write(string.Concat(header.Key, ": ", header.Value, "\r\n"));
            }
            _writer.Write("\r\n");
            _writer.Flush();

            if (_message.Body == null || _message.ContentLength == 0)
            {                
                return _stream;
            }
            else
            {
                byte[] bodyBuffer = new byte[(int)_message.Body.Length];
                _message.Body.Read(bodyBuffer, 0, (int)_message.Body.Length);
                _stream.Write(bodyBuffer, 0, bodyBuffer.Length);
                _writer.Flush();
            }
 
            return _stream;
        }

        /// <summary>
        ///     The previous <see cref="IMessageEncoder.Send"/> has just completed.
        /// </summary>
        /// <param name="bytesTransferred"></param>
        /// <remarks><c>true</c> if the message have been sent successfully; otherwise <c>false</c>.</remarks>
        public bool SendCompleted(int bytesTransferred)
        {
            if (_stream.Length == bytesTransferred)
            {
                Clear();
                return true;
            }

            Clear();
            return false;
        }

        /// <summary>
        ///     Remove everything used for the last message
        /// </summary>
        public void Clear()
        {
            if (_message != null && _message.Body != null)
                _message.Body.Dispose();

            _message = null;
            _stream.SetLength(0);
        }
    }
}