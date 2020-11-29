using System.IO;

namespace Bytewizer.TinyCLR
{
    static class StreamReaderExtensions
    {
        public static void SkipWhiteSpace(this StreamReader reader)
        {
            while (true)
            {
                var raw = reader.Peek();
                if (raw == -1)
                {
                    break;
                }
                var ch = (char)raw;
                if (!ch.IsWhiteSpace())
                {
                    break;
                }
                reader.Read();
            }
        }
    }
}
