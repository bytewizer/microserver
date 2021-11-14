using System.Resources;

namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// Represents the <see cref="ResourceManager"/> feature.
    /// </summary>
    public class ResourceManagerFeature : IResourceManagerFeature
    {
        public ResourceManager ResourceManager { get; set; }
    }
}