﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// An <see cref="StatusCodeResult"/> that when executed will produce an empty <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    public class OkResult : StatusCodeResult
    {
        private const int DefaultStatusCode = StatusCodes.Status200OK;

        /// <summary>
        /// Initializes a new instance of the <see cref="OkResult"/> class.
        /// </summary>
        public OkResult()
            : base(DefaultStatusCode)
        {
        }
    }
}
