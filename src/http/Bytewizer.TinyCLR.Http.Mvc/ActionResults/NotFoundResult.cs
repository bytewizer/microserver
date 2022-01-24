// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// Represents an <see cref="StatusCodeResult"/> that when executed will produce a Not Found (404) response.
    /// </summary>
    public class NotFoundResult : StatusCodeResult
    {
        private const int DefaultStatusCode = StatusCodes.Status404NotFound;

        /// <summary>
        /// Creates a new <see cref="NotFoundResult"/> instance.
        /// </summary>
        public NotFoundResult() : base(DefaultStatusCode)
        {
        }
    }
}
