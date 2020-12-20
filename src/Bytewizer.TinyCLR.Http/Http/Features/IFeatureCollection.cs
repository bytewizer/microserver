using System;

namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// Represents a collection of HTTP features.
    /// </summary>
    public interface IFeatureCollection
    {
        /// <summary>
        /// Indicates if the collection can be modified.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets or sets a given feature. Setting a null value removes the feature.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested feature, or null if it is not present.</returns>
        object this[Type key] { get; set; }

        /// <summary>
        /// Retrieves the requested feature from the collection.
        /// </summary>
        /// <returns>The requested feature, or null if it is not present.</returns>
        object Get(Type type);

        /// <summary>
        /// Sets the given feature in the collection.
        /// </summary>
        /// <param name="instance">The feature value.</param>
        void Set(Type type, object instance);
    }
}