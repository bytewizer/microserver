namespace Bytewizer.TinyCLR.Hardware
{
    /// <summary>
    /// Defines the model of a TinyCLR board.
    /// </summary>
    public enum BoardModel
    {
        /// <summary>
        /// Unknown board model.
        /// </summary>
        Unidentified = 0,

        /// <summary>
        /// SCM20260D development board.
        /// </summary>
        Sc20260D = 1,

        /// <summary>
        /// SC20100S development board.
        /// </summary>
        Sc20100S = 2,

        /// <summary>
        /// FEZ Bit single board computer.
        /// </summary>
        Bit = 3,

        /// <summary>
        /// FEZ Duino single board computer.
        /// </summary>
        Duino = 4,

        /// <summary>
        /// FEZ Feather single board computer.
        /// </summary>
        Feather = 5,

        /// <summary>
        /// FEZ Portal single board computer.
        /// </summary>
        Portal = 6,

        /// <summary>
        /// FEZ Stick single board computer.
        /// </summary>
        Stick = 7,
    }
}