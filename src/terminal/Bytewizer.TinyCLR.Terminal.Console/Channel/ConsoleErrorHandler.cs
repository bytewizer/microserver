using System;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// A delegate which is executed when the communications device class (CDC) for the connected client has and error.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="execption">The <see cref="Exception"/> for the error.</param>
    public delegate void ConsoleErrorHandler(object sender, Exception execption);
}