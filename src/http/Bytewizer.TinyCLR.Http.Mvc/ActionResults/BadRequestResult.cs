// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// A <see cref="StatusCodeResult"/> that when executed will produce a Bad Request (400) response.
    /// </summary>
    public class BadRequestResult : StatusCodeResult
    {
        private const int DefaultStatusCode = StatusCodes.Status400BadRequest;

        /// <summary>
        /// Creates a new <see cref="BadRequestResult"/> instance.
        /// </summary>
        public BadRequestResult()
            : base(DefaultStatusCode)
        {
        }
    }
}
