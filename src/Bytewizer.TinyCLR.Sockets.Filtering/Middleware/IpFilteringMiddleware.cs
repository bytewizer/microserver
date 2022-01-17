using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Sockets.Filtering
{
    public class IpFilteringMiddleware : Middleware
    {
        private readonly CidrNotation _cidr;

        public IpFilteringMiddleware(string cidr)
        {
            _cidr = CidrNotation.Parse(cidr);
        }

        protected override void Invoke(IContext context, RequestDelegate next)
        {
            var ctx = context as ISocketContext;

            try
            {
                if (_cidr.Contains(ctx.Channel.Connection.RemoteIpAddress))
                {
                    next(context);
                }
            }
            catch{}
        }
    }
}