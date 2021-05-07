namespace Bytewizer.TinyCLR.Ftp.Internal
{
    internal class FtpMiddleware : Middleware
    {

        public FtpMiddleware()
        {

        }

        protected override void Invoke(FtpContext context, RequestDelegate next)
        {
            
            next(context);
        
        }
    }
}