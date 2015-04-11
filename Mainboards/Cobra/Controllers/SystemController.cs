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


namespace MicroServer.CobraII
{
    public class SystemController : Controller
    {     
        public ActionResult Info(ControllerContext context)
        {
            JObject jdom = new JObject();
            jdom.Add("software", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            jdom.Add("version", SystemInfo.Version.ToString());
            jdom.Add("oemString", SystemInfo.OEMString);
            jdom.Add("isEmulator", SystemInfo.IsEmulator.ToString());
            jdom.Add("idModel", SystemInfo.SystemID.Model);
            jdom.Add("idOem", SystemInfo.SystemID.OEM);
            jdom.Add("idSku", SystemInfo.SystemID.SKU);
            jdom.Add("availableMemory", Debug.GC(false).ToString("n0") + " bytes");
            jdom.Add("ipAddress", HttpService.Current.InterfaceAddress.ToString());
            jdom.Add("ipPort", HttpService.Current.ServicePort);
            jdom.Add("hostname", ServiceManager.Current.ServerName);
            jdom.Add("dnsSuffix", ServiceManager.Current.DnsSuffix);
            jdom.Add("sntpEnabled", ServiceManager.Current.SntpEnabled);
            jdom.Add("dhcpEnabled", ServiceManager.Current.DhcpEnabled);
            jdom.Add("dnsEnabled", ServiceManager.Current.DnsEnabled);
            jdom.Add("httpEnabled", ServiceManager.Current.HttpEnabled);
            jdom.Add("loglevel", ServiceManager.Current.LogLevel.ToString());

            return new JsonResult(jdom);
        }

        public ActionResult Start(ControllerContext context)
        {
            ServiceManager.Current.StartAll();
            
            return new EmptyResult();
        }

        public ActionResult Stop(ControllerContext context)
        {
            ServiceManager.Current.StopAll();

            return new EmptyResult();
        }

        public ActionResult Restart(ControllerContext context)
        {
            ServiceManager.Current.RestartAll();

            return new EmptyResult();
        }

        public ActionResult Reboot(ControllerContext context)
        {

            return new EmptyResult();
        }

        public ActionResult Settings(ControllerContext context)
        {
            string cfgPath = ServiceManager.Current.StorageRoot + @"\settings.xml";
            
            JObject jdom = new JObject();

            if (context.HttpContext.Request.HttpMethod.ToUpper().Equals("POST"))
            {
                foreach (DictionaryEntry parameters in context.HttpContext.Request.Form)
                {
                    IParameter parameter = (IParameter)parameters.Value;     
                    if (!StringUtility.IsNullOrEmpty(parameter.Name) && !StringUtility.IsNullOrEmpty(parameter.Value))
                        ConfigManager.SetSetting(parameter.Name, parameter.Value);
                }

                ConfigManager.Save(cfgPath);
            }
            else 
            {
                ConfigManager.Load(cfgPath);

                foreach (DictionaryEntry parameter in ConfigManager.GetSettings())
                {
                    jdom.Add(parameter.Key.ToString(), parameter.Value.ToString());
                }
            }

            return new JsonResult(jdom);
        }
    }
}
