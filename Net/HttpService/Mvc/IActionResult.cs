using System;
using System.Text;

using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc
{
    public interface IActionResult
    {
        void ExecuteResult(IControllerContext context);
    }
}
