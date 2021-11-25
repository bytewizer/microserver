using System.Net;

namespace Bytewizer.TinyCLR.Ftp.Features
{
    /// <summary>
    /// Represents the <see cref="ISessionFeature"/> feature.
    /// </summary>
    internal class SessionFeature : ISessionFeature
    {
        public string FromPath { get; set; }
    }
}