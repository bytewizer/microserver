using System;
using System.Collections;

namespace Bytewizer.Playground
{
    [Serializable]
    public class FlashObject
    {
        public FlashObject()
        {
            Ssid = "ssid";
            Password = "password";
        }

        public string Ssid { get; set; }
        public string Password { get; set; }
    }
}