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
        public FilterContext(ActionContext actionContext)
            : base(actionContext)
        {
        }
    }
}
