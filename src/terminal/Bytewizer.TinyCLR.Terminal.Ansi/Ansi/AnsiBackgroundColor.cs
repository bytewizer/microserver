﻿namespace Bytewizer.TinyCLR.Terminal.Ansi
{
    /// <summary>
    /// ANSI codes for default background colors.
    /// </summary>
    /// <remarks>See <see cref="AnsiHandler"/> for typical usage scenarios.</remarks>
    public enum AnsiBackgroundColor : int
    {
        /// <summary>
        /// ANSI color code for black background.
        /// </summary>
        Black = 40,

        /// <summary>
        /// ANSI color code for red background.
        /// </summary>
        Red = 41,

        /// <summary>
        /// ANSI color code for green background.
        /// </summary>
        Green = 42,

        /// <summary>
        /// ANSI color code for yellow background.
        /// </summary>
        Yellow = 43,

        /// <summary>
        /// ANSI color code for blue background.
        /// </summary>
        Blue = 44,

        /// <summary>
        /// ANSI color code for magenta background.
        /// </summary>
        Magenta = 45,

        /// <summary>
        /// ANSI color code for cyan background.
        /// </summary>
        Cyan = 46,

        /// <summary>
        /// ANSI color code for white background.
        /// </summary>
        White = 47
    }
}
