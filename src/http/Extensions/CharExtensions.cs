namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// Contains extension methods for <see cref="char"/> object.
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Indicates whether the specified Unicode character is categorized as white space.
        /// </summary>
        /// <param name="source">The Unicode character to evaluate.</param>
        /// <returns><c>true</c> if c is white space; otherwise, <c>false</c>.</returns>
        public static bool IsWhiteSpace(this char source)
        {
            return (source == ' ' || source == '\t' || source == '\n' || source == '\r' || source == '\v');
        }
    }
}