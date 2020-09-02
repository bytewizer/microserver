namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Specifies protocols for authentication.
    /// </summary>
    public enum AuthenticationSchemes
    {
        /// <summary>
        /// Indicates that no authentication is allowed.
        /// </summary>
        None,

        /// <summary>
        /// Indicates digest authentication.
        /// </summary>
        Digest = 1,

        /// <summary>
        /// Indicates basic authentication.
        /// </summary>
        Basic = 8,

        /// <summary>
        /// Indicates anonymous authentication.
        /// </summary>
        Anonymous = 0x8000
    }
}
