namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public interface IModelBinder
    {
        bool CanBind(IModelBinderContext context);

        object Bind(IModelBinderContext context);
    }
}
