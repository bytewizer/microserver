using System.Net;

namespace Bytewizer.TinyCLR.Ftp.Features
{
    /// <summary>
    /// Represents the <see cref="SessionFeature"/> feature.
    /// </summary>
    internal interface ISessionFeature
    {
        string FromPath { get; }
    }
}