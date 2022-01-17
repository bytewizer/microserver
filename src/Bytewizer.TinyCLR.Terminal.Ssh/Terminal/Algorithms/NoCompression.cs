
namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class NoCompression : CompressionAlgorithm
    {
        public string Name
        {
            get { return "none"; }
        }

        public override byte[] Compress(byte[] input)
        {
            return input;
        }

        public override byte[] Decompress(byte[] input)
        {
            return input;
        }
    }
}
