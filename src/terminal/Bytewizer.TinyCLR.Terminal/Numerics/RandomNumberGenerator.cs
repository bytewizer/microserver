using System;

namespace Bytewizer.TinyCLR.Numerics
{
    public class RandomNumberGenerator
    {
        private static readonly object _lock = new object();

        public Random GetRandom { get { return rnd; } }

        private static readonly Random rnd;

        static RandomNumberGenerator()
        {
            rnd = new Random();
        }

        public static RandomNumberGenerator Create()
        {
            return new RandomNumberGenerator();
        }

        public void GetBytes(byte[] buffer)
        {
            lock (_lock)
            {
                rnd.NextBytes(buffer);
            }
        }
    }
}
