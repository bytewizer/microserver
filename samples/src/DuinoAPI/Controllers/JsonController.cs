using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Mvc;
using Bytewizer.TinyCLR.Http.Mvc.Filters;

namespace Bytewizer.TinyCLR.DuinoAPI
{
    public class JsonController : Controller
    {
        private readonly DeviceModel _deviceModel;
        private readonly NetworkModel _networkModel;
        
        public JsonController()
        {
            _deviceModel = new DeviceModel();
            _networkModel = new NetworkModel();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Method != HttpMethods.Get)
            {
                filterContext.Result = new BadRequestResult();
            }
        }

        public override void OnException(ExceptionContext filterContext)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;

            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult(new string[] { $"An error occurred in the {actionName} action." });
        }

        public IActionResult GetDeviceInfo()
        {
            return new JsonResult(_deviceModel) { Indented = true };
        }

        public IActionResult GetNetworkInfo()
        {
            return Json(_networkModel);

        }
    }
}