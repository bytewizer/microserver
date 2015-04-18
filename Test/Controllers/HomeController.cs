using System;

using MicroServer.Service;
using MicroServer.Net.Http.Mvc;

using MicroServer.Serializers.Json;

namespace MicroServer.Sample
{
    public class HomeController : Controller
    {
        public IActionResult Index(ControllerContext context)
        {
            string ipAddress = ServiceManager.Current.HttpService.InterfaceAddress.ToString();
            string port = ServiceManager.Current.HttpService.ActivePort.ToString();
            ViewData["IPADDRESS"] = string.Concat(ipAddress, ":", port);

            return View();
        }

        public IActionResult GetHello(ControllerContext context)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + DateTime.Now.ToString() + "</h1></body></html>\r\n";

            return ContentResult(response, "text/html");
        }

        public IActionResult GetEmpty(ControllerContext context)
        {
            return EmptyResult();
        }

        public IActionResult GetFile(ControllerContext context)
        {
            string rootFilePath = string.Concat(ServiceManager.Current.StorageRoot, @"\",
                                        MicroServer.Net.Http.Constants.HTTP_WEB_ROOT_FOLDER, @"\");

            string fileNamePath = @"\images\bay.jpg";

            return FileResult(rootFilePath, fileNamePath);
        }

        public IActionResult GetJson(ControllerContext context)
        {

            JArray jcar = new JArray();
            jcar.Add(new JProperty("make", "ford"));
            jcar.Add(new JProperty("model", "mustang"));
            jcar.Add(new JProperty("color", "blue"));

            JArray jcar1 = new JArray();
            jcar1.Add(new JProperty("make", "bmw"));
            jcar1.Add(new JProperty("model", "M5"));
            jcar1.Add(new JProperty("color", "black"));

            JArray jgroup = new JArray();
            jgroup.Add(new JProperty("group", "car"));
            jgroup.Add(new JProperty("type", jcar));
            jgroup.Add(new JProperty("type", jcar1));

            JArray jar = new JArray();
            jar.Add(new JProperty("style", ""));
            jar.Add(new JProperty("data-role", "update"));
            jar.Add(new JProperty("data-inset", "true"));
            jar.Add(new JProperty("group", jgroup));

            JObject jdom = new JObject();
            jdom.Add("date", new DateTime(1983, 3, 20).ToString());
            jdom.Add("data-role", "update");
            jdom.Add("style", jar);

            return JsonResult(jdom);
        }

        public IActionResult GetRedirectToGetFile(ControllerContext context)
        {
            return RedirectResult("Home", "GetFile");
        }

        public IActionResult GetRedirectToGoogle(ControllerContext context)
        {
            return RedirectResult("http://www.google.com/");
        }
    }
}
