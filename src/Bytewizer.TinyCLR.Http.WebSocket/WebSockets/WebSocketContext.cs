using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public class WebSocketContext
    {
        
        public WebSocketContext(byte[] payload, bool isText)
        {
            Payload = payload;
            IsText = isText;
            //SubProtocols = subProtocols;
        }

        public byte[] Payload { get; private set; }

        public bool IsText { get; private set; }

        //public string[] SubProtocols { get; private set; }    
    }
}
