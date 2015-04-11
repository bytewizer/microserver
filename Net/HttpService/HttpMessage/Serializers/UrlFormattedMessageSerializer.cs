using System;
using System.IO;

using MicroServer.Extensions;
using MicroServer.Net.Http.Messages;

namespace MicroServer.Net.Http.Serializers
{
    /// <summary>
    /// Serializer for <c>application/x-www-form-urlencoded</c>
    /// </summary>
    public class UrlFormattedMessageSerializer : IMessageSerializer
    {
        /// <summary>
        /// The mimetype that this decoder is for.
        /// </summary>
        /// <value>application/x-www-form-urlencoded</value>
        public const string MimeType = "application/x-www-form-urlencoded";

        /// <summary>
        ///     Content types that this serializer supports.
        /// </summary>
        public string[] SupportedContentTypes { get { return new[] {MimeType}; }}

        /// <summary>
        /// Deserialize the content from the stream.
        /// </summary>
        /// <param name="contentType">Used to identify the object which is about to be deserialized. Specified by the <c>Serialize()</c> method when invoked in the other end point.</param>
        /// <param name="source">Stream that contains the object to deserialize.</param>
        /// <returns>Created object</returns>
        /// <exception cref="SerializationException">Deserialization failed</exception>
        public object Deserialize(string contentType, Stream source)
        {
            if (contentType == null) throw new ArgumentNullException("contentType");
            if (source == null) throw new ArgumentNullException("source");

            if (!contentType.StartsWith(MimeType))
                return null;

            try
            {
                var result = new FormAndFilesResult()
                {
                    Form = new ParameterCollection()
                };
                var decoder = new UrlDecoder();
                decoder.Parse(new StreamReader(source), result.Form);
                source.Position = 0;
                return result;
            }
            catch (ArgumentException err)
            {
                throw new Exception(err.Message, err);
            }
        }
    }
}
