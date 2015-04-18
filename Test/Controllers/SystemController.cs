using System;
using System.Collections;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using MicroServer;
using MicroServer.Service;
using MicroServer.Serializers.Json;
using MicroServer.Utilities;

using MicroServer.Net.Http;
using MicroServer.Net.Http.Mvc;

namespace MicroServer.Emulator
{
    public class SystemController : Controller
    {     
        public IActionResult Start(ControllerContext context)
        {
            ServiceManager.Current.StartAll();
            
            return EmptyResult();
        }

        public IActionResult Stop(ControllerContext context)
        {
            ServiceManager.Current.StopAll();

            return EmptyResult();
        }

        public IActionResult Restart(ControllerContext context)
        {
            ServiceManager.Current.RestartAll();

            return EmptyResult();
        }

        public IActionResult Reboot(ControllerContext context)
        {
            return EmptyResult();
        }
    }
}
