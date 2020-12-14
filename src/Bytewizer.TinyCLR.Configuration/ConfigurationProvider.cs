using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Configuration
{
    /// <summary>
    /// Base class for implementing an <see cref="IConfigurationProvider"/>
    /// </summary>
    public abstract class ConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        /// Initializes a new <see cref="IConfigurationProvider"/>
        /// </summary>
        protected ConfigurationProvider()
        {
            Data = new Dictionary();
        }

        /// <summary>
        /// The configuration key value pairs for this provider.
        /// </summary>
        protected Dictionary Data { get; set; }

        /// <summary>
        /// Attempts to find a value with the given key, returns true if one is found, false otherwise.
        /// </summary>
        /// <param name="key">The key to lookup.</param>
        /// <param name="value">The value found at key if one is found.</param>
        /// <returns>True if key has a value, false otherwise.</returns>
        public virtual bool TryGet(string key, out string value)
            => Data.TryGetValue(key, out value);

        /// <summary>
        /// Sets a value for a given key.
        /// </summary>
        /// <param name="key">The configuration key to set.</param>
        /// <param name="value">The value to set.</param>
        public virtual void Set(string key, string value)
            => Data[key] = value;

        /// <summary>
        /// Loads (or reloads) the data for this provider.
        /// </summary>
        public virtual void Load()
        { 
        }

        /// <summary>
        /// Generates a string representing this provider name and relevant details.
        /// </summary>
        /// <returns> The configuration name. </returns>
        public override string ToString() => $"{GetType().Name}";
    }
}