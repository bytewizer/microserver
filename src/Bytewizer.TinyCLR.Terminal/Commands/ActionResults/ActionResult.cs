﻿namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// A default implementation of <see cref="IActionResult"/>.
    /// </summary>
    public abstract class ActionResult : IActionResult
    {
        /// <inheritdoc/>
        public abstract void ExecuteResult(ActionContext context);
    }
}
