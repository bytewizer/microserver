
namespace Bytewizer.TinyCLR.Http.WebSocket
{
    /// <summary>
    /// Is final frame or not.
    /// </summary>
    public enum WebSocketFin : byte
    {
        /// <summary>
        /// There are more fragments.
        /// </summary>
        More = 0x0,

        /// <summary>
        /// This is the final frame.
        /// </summary>
        Final = 0x1,
    }
}
