using System.Text;

namespace Bytewizer.TinyCLR.Terminal.Ansi
{
    /// <summary>
    /// Extension methods for <see cref="StringBuilder"/>.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the the ANSI sequence to set all of the attribute, foreground, and background colors to this instance.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="attribute">Which attribute to set.</param>
        /// <param name="foregroundColor">Which foreground color to set.</param>
        /// <param name="backgroundColor">Which background color to set.</param>
        public static StringBuilder AppendAnsi(
            this StringBuilder builder, 
            AnsiTextAttribute attribute, 
            AnsiForegroundColor foregroundColor, 
            AnsiBackgroundColor backgroundColor)
        {
            return builder.Append(AnsiHandler.SetTextAttributes(attribute, foregroundColor, backgroundColor));
        }

        /// <summary>
        /// Appends the the ANSI sequence to set foreground colors to this instance.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="attribute">Which attribute to set.</param>
        public static StringBuilder AppendAnsi(this StringBuilder builder, AnsiTextAttribute attribute)
        {
            return builder.Append(AnsiHandler.SetTextAttribute(attribute));
        }

        /// <summary>
        /// Appends the the ANSI sequence to set foreground colors to this instance.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="foregroundColor">Which foreground color to set.</param>
        public static StringBuilder AppendAnsi(this StringBuilder builder, AnsiForegroundColor foregroundColor)
        {
            return builder.Append(AnsiHandler.SetForegroundColor(foregroundColor));
        }

        /// <summary>
        /// Appends the the ANSI sequence to set background colors to this instance.
        /// </summary>
        /// <param name="builder">The string builder instance.</param>
        /// <param name="backgroundColor">Which background color to set.</param>
        public static StringBuilder AppendAnsi(this StringBuilder builder, AnsiBackgroundColor backgroundColor)
        {
            return builder.Append(AnsiHandler.SetBackgroundColor(backgroundColor));
        }
    }
}