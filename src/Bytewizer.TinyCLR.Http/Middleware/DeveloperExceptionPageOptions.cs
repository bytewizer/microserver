﻿namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for configuring the <see cref="DeveloperExceptionPageMiddleware"/>.
    /// </summary>
    public class DeveloperExceptionPageOptions
    {
        /// <summary>
        /// Create an instance with the default options settings.
        /// </summary>
        public DeveloperExceptionPageOptions()
        {
            DisplayStackTrace = true;
        }

        /// <summary>
        /// Include stack trace in exception page.
        /// </summary>
        public bool DisplayStackTrace { get; set; }
    }
}
