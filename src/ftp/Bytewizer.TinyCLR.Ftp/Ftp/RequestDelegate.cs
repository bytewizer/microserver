namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// A function that can process a FTP request.
    /// </summary>
    /// <param name="context">The context for the request.</param>
    public delegate void RequestDelegate(FtpContext context);
}