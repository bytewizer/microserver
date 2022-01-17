using System.Collections;

namespace Bytewizer.TinyCLR.Terminal.Ansi
{
    /// <summary>
    /// ANSI sequences and ANSI building methods.
    /// </summary>
    public static class AnsiHandler
    {

        /// <summary>Gets the ANSI sequence to move the cursor up the specified number of lines.</summary>
        /// <param name="numLines">The number of lines to move the cursor up.</param>
        /// <returns>The ANSI sequence to move the cursor up the specified number of lines.</returns>
        public static string MoveCursorUp(int numLines)
        {
            return AnsiSequences.Esc + $"[{numLines}A";
        }

        /// <summary>Gets the ANSI sequence to move the cursor down the specified number of lines.</summary>
        /// <param name="numberLines">The number of lines to move the cursor down.</param>
        /// <returns>The ANSI sequence to move the cursor down the specified number of lines.</returns>
        public static string MoveCursorDown(int numberLines)
        {
            return AnsiSequences.Esc + $"[{numberLines}B";
        }

        /// <summary>Gets the ANSI sequence to move the cursor right the specified number of columns.</summary>
        /// <param name="numberColumns">The number of columns to move the cursor right.</param>
        /// <returns>The ANSI sequence to move the cursor right the specified number of columns.</returns>
        public static string MoveCursorRight(int numberColumns)
        {
            return AnsiSequences.Esc + $"[{numberColumns}C";
        }

        /// <summary>Gets the ANSI sequence to move the cursor left the specified number of columns.</summary>
        /// <param name="numberColumns">The number of columns to move the cursor left.</param>
        /// <returns>The ANSI sequence to move the cursor left the specified number of columns.</returns>
        public static string MoveCursorLeft(int numberColumns)
        {
            return AnsiSequences.Esc + $"[{numberColumns}D";
        }

        /// <summary>Gets the ANSI sequence to set the cursor to the specified coordinates.</summary>
        /// <param name="line">Which line to set the cursor on.</param>
        /// <param name="column">Which column to set the cursor on.</param>
        /// <returns>The ANSI sequence to set the cursor to the specified coordinates.</returns>
        public static string MoveCursorTo(int line, int column)
        {
            return AnsiSequences.Esc + $"[{line};{column}H";
        }

        /// <summary>Gets the ANSI sequence to set the foreground to the specified color.</summary>
        /// <param name="foregroundColor">Which foreground color to set.</param>
        /// <returns>The ANSI sequence to set the foreground to the specified color.</returns>
        public static string SetForegroundColor(AnsiForegroundColor foregroundColor)
        {
            return AnsiSequences.Esc + $"[{(int) foregroundColor}m";
        }

        /// <summary>Gets the ANSI sequence to set the background to the specified color.</summary>
        /// <param name="backgroundColor">Which background color to set.</param>
        /// <returns>The ANSI sequence to set the background to the specified color.</returns>
        public static string SetBackgroundColor(AnsiBackgroundColor backgroundColor)
        {
            return AnsiSequences.Esc + $"[{(int) backgroundColor}m";
        }

        /// <summary>Gets the ANSI sequence to set all of the attribute, foreground, and background colors.</summary>
        /// <param name="attribute">Which attribute to set.</param>
        /// <param name="foregroundColor">Which foreground color to set.</param>
        /// <param name="backgroundColor">Which background color to set.</param>
        /// <returns>The ANSI sequence to set all of the attribute, foreground, and background colors.</returns>
        public static string SetTextAttributes(AnsiTextAttribute attribute, AnsiForegroundColor foregroundColor, AnsiBackgroundColor backgroundColor)
        {
            return AnsiSequences.Esc + $"[{(int) attribute};{(int) foregroundColor};{(int) backgroundColor}m";
        }

        /// <summary>Gets the ANSI sequence to set all of the attribute.</summary>
        /// <param name="attribute">Which attribute to set.</param>
        /// <returns>The ANSI sequence to set all of the attribute, foreground, and background colors.</returns>
        public static string SetTextAttribute(AnsiTextAttribute attribute)
        {
            return AnsiSequences.Esc + $"[{(int)attribute}m";
        }

        /// <summary>Gets the ANSI sequence to set the text attribute.</summary>
        /// <param name="attribute">Which attribute to set.</param>
        /// <returns>The ANSI sequence to set the text attribute.</returns>
        public static string SetTextAttributes(AnsiTextAttribute attribute)
        {
            return AnsiSequences.Esc + $"[{(int) attribute}m";
        }

        /// <summary>Gets the ANSI sequence to set the text foreground and background colors.</summary>
        /// <param name="foregroundColor">Which foreground color to set.</param>
        /// <param name="backgroundColor">Which background color to set.</param>
        /// <returns>The ANSI sequence to set the text foreground and background colors.</returns>
        public static string SetTextAttributes(AnsiForegroundColor foregroundColor, AnsiBackgroundColor backgroundColor)
        {
            return AnsiSequences.Esc + $"[{(int) foregroundColor};{(int) backgroundColor}m";
        }
    }
}