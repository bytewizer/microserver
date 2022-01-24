namespace Bytewizer.TinyCLR.Http.WebSockets
{
  /// <summary>
  /// Indicates whether the payload data of a WebSocket frame is masked.
  /// </summary>
  internal enum Mask : byte
  {
    /// <summary>
    /// Equivalent to numeric value 0. Indicates not masked.
    /// </summary>
    Off = 0x0,

    /// <summary>
    /// Equivalent to numeric value 1. Indicates masked.
    /// </summary>
    On = 0x1
  }
}
