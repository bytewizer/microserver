﻿using System;

#if NanoCLR
namespace Bytewizer.NanoCLR.Sockets
#else
namespace Bytewizer.TinyCLR.Sockets
#endif
{
    /// <summary>
    /// A delegate which is executed when the socket has and error.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="execption">The <see cref="Exception"/> for the error.</param>
    public delegate void SocketErrorHandler(object sender, Exception execption);
}
