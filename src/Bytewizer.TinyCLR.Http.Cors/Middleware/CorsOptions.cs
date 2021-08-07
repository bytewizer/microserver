// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for configuring the <see cref="CorsMiddleware"/>.
    /// </summary>
    public class CorsOptions
    {
        /// <summary>
        /// Create an instance with the default options settings.
        /// </summary>
        public CorsOptions()
        {
            Origins = "*";
            Headers = "*";
            Methods = "*";
        }

        /// <summary>
        /// The origins allowed in a string of comma-seprated values.
        /// </summary>
        public string Origins { get; set; }

        /// <summary>
        /// The methods allowed in a string of comma-seprated values.
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// The headers allowed in a string of comma-seprated values.
        /// </summary>
        public string Methods { get; set; }
    }
}