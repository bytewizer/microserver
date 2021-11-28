namespace Bytewizer.TinyCLR.Ftp.Features
{
    /// <summary>
    /// Represents the <see cref="ISessionFeature"/> feature.
    /// </summary>
    internal class SessionFeature : ISessionFeature
    {
        public string FromPath { get; set; }
        public int TlsBlockSize { get; set; }
        public string TlsProt { get; set; }
        public int RestPosition { get; set; }
    }
}