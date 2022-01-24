namespace Bytewizer.TinyCLR.Http.WebSockets
{
  /// <summary>
  /// Indicates the state of a WebSocket connection.
  /// </remarks>
  public enum WebSocketState : ushort
  {
    /// <summary>
    /// Equivalent to numeric value 0. Indicates that the connection has not
    /// yet been established.
    /// </summary>
    Connecting = 0,

    /// <summary>
    /// Equivalent to numeric value 1. Indicates that the connection has
    /// been established, and the communication is possible.
    /// </summary>
    Open = 1,

    /// <summary>
    /// Equivalent to numeric value 2. Indicates that the connection is
    /// going through the closing handshake, or the close method has
    /// been invoked.
    /// </summary>
    Closing = 2,

    /// <summary>
    /// Equivalent to numeric value 3. Indicates that the connection has
    /// been closed or could not be established.
    /// </summary>
    Closed = 3
  }
}
