namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Convenience methods for <see cref="FtpResponse"/>.
    /// </summary>
    public static class FtpResponseExtensions
    {
        /// <summary>
        /// Writes the given response using  UTF-8 encoding.
        /// </summary>
        /// <param name="response">The <see cref="FtpResponse"/>.</param>
        /// <param name="code">The response code.</param>
        /// <param name="message">The response message.</param>
        public static void Write(this FtpResponse response, int code, string message)
        {
            response.Code = code;
            response.Message = message;
        }
    }
}