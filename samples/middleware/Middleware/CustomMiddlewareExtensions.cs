using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Sample
{
    public static class CustomMiddlewareExtensions
    {
        public static void UseCustomMiddleware(this ServerOptions option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            option.UseMiddleware(new CustomMiddleware());
        }
    }
}
