namespace Bytewizer.TinyCLR.Http.WebSockets
{
  /// <summary>
  /// Indicates whether a WebSocket frame is the final frame of a message.
  /// </summary>
  internal enum Fin : byte
  {
    /// <summary>
    /// Equivalent to numeric value 0. Indicates more frames of a message follow.
    /// </summary>
    More = 0x0,

    /// <summary>
    /// Equivalent to numeric value 1. Indicates the final frame of a message.
    /// </summary>
    Final = 0x1
  }
}
