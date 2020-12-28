namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    /// <summary>
    /// Defines an interface for model binders.
    /// </summary>
    public interface IModelBinder
    {
        /// <summary>
        /// Attempts to bind a model.
        /// </summary>
        /// <param name="context">The <see cref="ModelBinderContext"/>.</param>
        bool CanBind(IModelBinderContext context);

        /// <summary>
        /// Bind a model.
        /// </summary>
        /// <param name="context">The <see cref="ModelBinderContext"/>.</param>
        object Bind(IModelBinderContext context);
    }
}
