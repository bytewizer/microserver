using System;
using System.Collections;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Sockets;


namespace Bytewizer.TinyCLR.Http
{
    class HttpServerOptions : ServerOptions
    {
        /// <summary>
        /// Specifies the name the server represents.
        /// </summary>
        public string Name { get; set; } = "Microserver";
    }
}
