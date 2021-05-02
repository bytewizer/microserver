namespace Bytewizer.TinyCLR.Http
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Contains methods to verify the request protocol version of an HTTP request. 
    /// </summary>
    public static class HttpProtocol
    {
        public static readonly string Http10 = "HTTP/1.0";
        public static readonly string Http11 = "HTTP/1.1";
        public static readonly string Http2 = "HTTP/2";
        public static readonly string Http3 = "HTTP/3";
    }
}
