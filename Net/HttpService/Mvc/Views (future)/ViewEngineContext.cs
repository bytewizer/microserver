using System;

using MicroServer.Net.Http.Files;

namespace MicroServer.Net.Http.Mvc.Views
{
    public class ViewEngineContext
    {
        public ViewContext ViewContext { get; set; }
        public IFileService FileService { get; set; }
    }
}