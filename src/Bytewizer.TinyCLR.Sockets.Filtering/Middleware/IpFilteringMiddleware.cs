using System;
using System.IO;
using System.Net;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;


namespace Bytewizer.Playground.Sockets
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