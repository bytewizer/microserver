using System;
using System.IO;

using MicroServer.Extensions;
using MicroServer.Net.Http.Serializers;
using MicroServer.Net.Http.Exceptions;
using MicroServer.Net.Sockets;
using System.Net.Sockets;
using MicroServer.Utilities;
using Microsoft.SPOT;

namespace MicroServer.Net.Http.Messages
{
    /// <summary>
    ///     Decodes HTTP messages
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Per default the body is not decoded. To change that behavior you should use the constructor that takes
    ///         a message serializer.
    ///     </para>
    /// </remarks>
    public class HttpMessageDecoder : IMessageDecoder
    {
        private readonly HttpCookieParser _cookieParser = new HttpCookieParser();
        private readonly HeaderParser _headerParser;
        private readonly IMessageSerializer _messageSerializer;
        private int _frameContentBytesLeft;
        private bool _isHeaderParsed;
        private SocketChannel _channel;
        private HttpMessage _message;
        private MessageReceivedHandler _messageReceived;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpMessageDecoder" /> class.
        /// </summary>
        public HttpMessageDecoder()
        {
            _headerParser = new HeaderParser();
            _headerParser.HeaderParsed = OnHeader;
            _headerParser.RequestLineParsed = OnRequestLine;
            _headerParser.Completed = OnHeaderParsed;
            _messageReceived = delegate { };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpMessageDecoder" /> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <exception cref="System.ArgumentNullException">messageSerializer</exception>
        public HttpMessageDecoder(IMessageSerializer messageSerializer)
        {
            if (messageSerializer == null) throw new ArgumentNullException("messageSerializer");
            _messageSerializer = messageSerializer;
            _headerParser = new HeaderParser();
            _headerParser.HeaderParsed = OnHeader;
            _headerParser.RequestLineParsed = OnRequestLine;
            _headerParser.Completed = OnHeaderParsed;
            _messageReceived = delegate { };
        }

        /// <summary>
        ///     A message have been received.
        /// </summary>
        /// <remarks>
        ///     Do note that streams are being reused by the decoder, so don't try to close it.
        /// </remarks>
        public MessageReceivedHandler MessageReceived
        {
            get { return _messageReceived; }
            set
            {
                if (value == null)
                    _messageReceived = (c, m) => {  };
                else
                    _messageReceived = value;
            }
        }

        /// <summary>
        ///     We've received bytes from the socket. Build a message out of them.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public void ProcessReadBytes(SocketChannel channel, SocketBuffer buffer)
        {
            _channel = channel;

            var receiveBufferOffset = buffer.Offset;
            var bytesLeftInReceiveBuffer = buffer.BytesTransferred;
            while (true)
            {
                if (bytesLeftInReceiveBuffer <= 0)
                    break;

                if (!_isHeaderParsed)
                {
                    var offsetBefore = receiveBufferOffset;
                    receiveBufferOffset = _headerParser.Parse(buffer, receiveBufferOffset);
                    if (!_isHeaderParsed)
                        return;

                    bytesLeftInReceiveBuffer -= receiveBufferOffset - offsetBefore;
                    _frameContentBytesLeft = _message.ContentLength;
                    if (_frameContentBytesLeft == 0)
                    {
                        TriggerMessageReceived(_message);
                        _message = null;
                        _isHeaderParsed = false;
                        continue;
                    }

                    _message.Body = new MemoryStream();
                }

                var bytesRead = BytesProcessed(buffer.Offset, receiveBufferOffset);
                var bytesToWrite = System.Math.Min(_frameContentBytesLeft, buffer.BytesTransferred - bytesRead);
                _message.Body.Write(buffer.Buffer, receiveBufferOffset, bytesToWrite);
                _frameContentBytesLeft -= bytesToWrite;
                receiveBufferOffset += bytesToWrite;
                bytesLeftInReceiveBuffer -= bytesToWrite;
                if (_frameContentBytesLeft == 0)
                {
                    _message.Body.Position = 0;
                    TriggerMessageReceived(_message);
                    Clear();
                }
            }
        }

        /// <summary>
        ///     Reset decoder state so that we can decode a new message
        /// </summary>
        public void Clear()
        {
            _message = null;
            _isHeaderParsed = false;
            _frameContentBytesLeft = 0;
        }

        private int BytesProcessed(int startOffset, int currentOffset)
        {
            return currentOffset - startOffset;
        }

        private void OnHeader(string name, string value)
        {
            _message.AddHeader(name, value);
        }

        private void OnHeaderParsed()
        {
            _isHeaderParsed = true;
        }

        private void OnRequestLine(string part1, string part2, string part3)
        {
            if (part1.StartsWith("http/"))
            {
                int code;
                if (!ParseUtility.TryParseInt(part2, out code))
                    throw new BadRequestException(
                        string.Concat("Second word in the status line should be a HTTP code, you specified ", part2, "."));

                if (_messageSerializer != null)
                    _message = new HttpResponse(code, part3, part1);
                else
                    _message = new HttpResponseBase(code, part3, part1);
            }
            else
            {
                if (!part3.StartsWith("http/"))
                    throw new BadRequestException(
                        string.Concat("Status line for requests should end with the HTTP version. Your line ended with ", part3, "."));

                _message = _messageSerializer != null
                    ? new HttpRequest(part1, part2, part3)
                    : new HttpRequestBase(part1, part2, part3);
            }
        }

        private void TriggerMessageReceived(HttpMessage message)
        {
            var request = message as HttpRequest;
            if (_messageSerializer != null && request != null)
            {
                if (message.Body != null && message.Body.Length > 0)
                {
                    var result = _messageSerializer.Deserialize(message.Headers["Content-Type"], message.Body);

                    if (result == null)
                        throw new BadRequestException("Unsupported content-type: " + message.ContentType);

                    var formAndFiles = result as FormAndFilesResult;
                    if (formAndFiles != null)
                    {
                        request.Form = formAndFiles.Form;
                        request.Files = formAndFiles.Files;
                    }
                    else
                        throw new HttpException(500, "Unknown decoder result: " + result);
                }
                var cookies = request.Headers["Cookie"];
                if (cookies != null)
                {
                    request.Cookies = _cookieParser.Parse(cookies);
                }
            }

            _messageReceived(_channel, message);
        }
    }
}