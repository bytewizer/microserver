namespace Bytewizer.TinyCLR.Hardware
{
    /// <summary>
    /// Defines the specific hardware model of a TinyCLR board.
    /// </summary>
    public enum ChipsetModel
    {
        /// <summary>
        /// Unknown chipset model.
        /// </summary>
        Unidentified = 0,

        /// <summary>
        /// SCM20260 chipset model.
        /// </summary>
        Sc20260 = 1,

        /// <summary>
        /// SC20100 chipset model.
        /// </summary>
        Sc20100 = 2,
    }
}