namespace Bytewizer.TinyCLR.Terminal.Ansi
{
    /// <summary>
    /// ANSI codes for default foreground (character) colors.
    /// </summary>
    /// <remarks>See <see cref="AnsiHandler"/> for typical usage scenarios.</remarks>
    public enum AnsiForegroundColor : int
    {
        /// <summary>
        /// ANSI color code for black.
        /// </summary>
        Black = 30,

        /// <summary>
        /// ANSI color code for red.
        /// </summary>
        Red = 31,

        /// <summary>
        /// ANSI color code for green.
        /// </summary>
        Green = 32,

        /// <summary>
        /// ANSI color code for yellow.
        /// </summary>
        Yellow = 33,

        /// <summary>
        /// ANSI color code for blue.
        /// </summary>
        Blue = 34,

        /// <summary>
        /// ANSI color code for magenta.
        /// </summary>
        Magenta = 35,

        /// <summary>
        /// ANSI color code for cyan.
        /// </summary>
        Cyan = 36,

        /// <summary>
        /// ANSI color code for white.
        /// </summary>
        White = 37,

        /// <summary>
        /// ANSI color code for reset.
        /// </summary>
        Reset = 0


    }
}
