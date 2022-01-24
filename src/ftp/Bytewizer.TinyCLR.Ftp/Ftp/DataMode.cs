namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Defines FTP data modes.
    /// </summary>
    public enum DataMode
    {
        /// <summary>
        /// No data mode defined.
        /// </summary>
        None,

        /// <summary>
        /// Passive data mode.
        /// </summary>
        Passive,

        /// <summary>
        /// Active data mode.
        /// </summary>
        Active,

        /// <summary>
        /// Extended passive data mode.
        /// </summary>
        ExtendedPassive,

        /// <summary>
        /// Extended active data mode.
        /// </summary>
        ExtendedActive
    }
}
