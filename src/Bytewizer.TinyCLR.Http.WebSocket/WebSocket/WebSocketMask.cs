
namespace Bytewizer.TinyCLR.Http.WebSocket
{
    /// <summary>
    /// Is data masked or not.
    /// </summary>
    public enum WebSocketMask : byte
    {
        /// <summary>
        /// Data is not masked.
        /// </summary>
        Unmask = 0x0,

        /// <summary>
        /// Data is masked.
        /// </summary>
        Mask = 0x1,
    }
}
