using System.Collections;

namespace Bytewizer.TinyCLR.Telnet.Features
{
    /// <summary>
    /// A feature interface for this session. Use <see cref="TelnetContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public class SecureShellFeature 
    {
        /// <summary>
        /// Gets or sets available HostKeys.
        /// </summary>
        public Hashtable HostKeys { get; set; }

        /// <summary>
        /// Gets or sets available key exchange algorithms.
        /// </summary>
        public Hashtable KeyExchangeAlgorithms { get; set; }

        /// <summary>
        ///  Gets or sets available public key algorithms.
        /// </summary>
        public Hashtable PublicKeyAlgorithms { get; set; }

        /// <summary>
        ///  Gets or sets available encryption algorithms.
        /// </summary>
        public Hashtable EncryptionAlgorithms { get; set; }

        /// <summary>
        ///  Gets or sets available compression algorithms.
        /// </summary>
        public Hashtable CompressionAlgorithms  { get; set; }
    }
}