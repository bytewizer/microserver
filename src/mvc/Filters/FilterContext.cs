using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.Filters
{
    /// <summary>
    /// An abstract context for filters.
    /// </summary>
    public abstract class FilterContext : ActionContext
    {
        /// <summary>
        /// Instantiates a new <see cref="FilterContext"/> instance.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/>.</param>
        /// <param name="filters">All applicable <see cref="IFilterMetadata"/> implementations.</param>
        public FilterContext(
            ActionContext actionContext,
            ArrayList filters)
            : base(actionContext)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            Filters = filters;
        }

        /// <summary>
        /// Gets all applicable <see cref="IFilterMetadata"/> implementations.
        /// </summary>
        public virtual ArrayList Filters { get; }

    }
}
