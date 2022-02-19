using System;

namespace Bytewizer.TinyCLR.Security.Cryptography
{

    /// <summary>
    /// Initializes a new instance of <see cref="RandomNumberGenerator"/>.
    /// </summary>
    public abstract class RandomNumberGenerator : IDisposable
    {
        private static readonly object _lock = new object();

        public Random GetRandom { get { return rnd; } }

        private static readonly Random rnd;

        static RandomNumberGenerator()
        {
            rnd = new Random();
        }

        /// <summary>
        /// A new instance of a cryptographic random number generator.
        /// </summary>
        public static RandomNumberGenerator Create()
        {
            return Create();
        }

        /// <summary>
        /// When overridden in a derived class, fills an array of bytes with a cryptographically strong random sequence of values.
        /// </summary>
        /// <param name="data">The array to fill with cryptographically strong random bytes.</param>
        public void GetBytes(byte[] data)
        {
            lock (_lock)
            {
                rnd.NextBytes(data);
            }
        }

        /// <summary>
        /// When overridden in a derived class, releases all resources used by the current instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        public void Dispose()
        {
           
        }
    }
}
