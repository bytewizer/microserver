namespace Bytewizer.TinyCLR.Http.WebSockets
{
  /// <summary>
  /// Indicates whether each RSV (RSV1, RSV2, and RSV3) of a WebSocket frame is non-zero.
  /// </summary>
  internal enum Rsv : byte
  {
    /// <summary>
    /// Equivalent to numeric value 0. Indicates zero.
    /// </summary>
    Off = 0x0,

    /// <summary>
    /// Equivalent to numeric value 1. Indicates non-zero.
    /// </summary>
    On = 0x1
  }
}
