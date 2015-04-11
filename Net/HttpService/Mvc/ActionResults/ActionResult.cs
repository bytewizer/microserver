using System;

using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc
{
    public abstract class ActionResult
    {
        public abstract void ExecuteResult(IControllerContext context);
    }
}
