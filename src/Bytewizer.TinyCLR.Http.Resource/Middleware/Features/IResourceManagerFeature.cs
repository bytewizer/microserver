using System.Resources;

namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// Represents the <see cref="ResourceManager"/> feature.
    /// </summary>
    public interface IResourceManagerFeature
    {
        ResourceManager ResourceManager { get; }
    }
}