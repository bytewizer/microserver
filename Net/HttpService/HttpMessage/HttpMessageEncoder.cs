using System;
using System.IO;
using System.Collections;

using MicroServer.Net.Sockets;
using Microsoft.SPOT;

namespace MicroServer.Net.Http.Messages
{
    /// <summary>
    ///     Used to encode request/response into a byte stream.
    /// </summary>
    public class HttpMessageEncoder : IMessageEncoder
    {
        private HttpContext _message;
        private HttpResponse _response;
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
            if (!(message is HttpContext))
                throw new InvalidOperationException("This encoder only supports messages deriving from 'HttpMessage'");

            _message = (HttpContext)message;
            _response = (HttpResponse)_message.Response;

            if (_response.Body == null || _response.Body.Length == 0)
            {
                _response.Headers["Content-Length"] = "0";
            }
            else
            {
                _response.ContentLength = (int)_response.Body.Length;
            }

            _writer.WriteLine(_response.StatusLine);

            if (_response.Headers != null && _response.Headers.Count > 0)
            {
                foreach (DictionaryEntry header in _response.Headers)
                {
                    _writer.Write(string.Concat(header.Key, ": ", header.Value, "\r\n"));
                }
            }

            if (_response.Cookies != null && _response.Cookies.Count > 0)
            {
                //Set-Cookie: <name>=<value>[; <name>=<value>][; expires=<date>][; domain=<domain_name>][; path=<some_path>][; secure][; httponly]
                foreach (DictionaryEntry item in _response.Cookies)
                {
                    HttpResponseCookie cookie = (HttpResponseCookie)item.Value;
                    _writer.Write(string.Concat(
                                "Set-Cookie: ",
                                cookie.ToString(),
                                "\r\n")
                                );
                }
            }

            _writer.Write("\r\n");
            _writer.Flush();

            if (_response.Body == null || _response.ContentLength == 0)
            {                
                return _stream;
            }
            else
            {
                byte[] bodyBuffer = new byte[(int)_response.Body.Length];
                _response.Body.Read(bodyBuffer, 0, (int)_response.Body.Length);
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
            if (_message != null && _response.Body != null)
                _response.Body.Dispose();

            _message = null;
            _stream.SetLength(0);
        }
    }
}