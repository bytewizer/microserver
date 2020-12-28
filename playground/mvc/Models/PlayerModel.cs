using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.Playground.Mvc.Models
{
    //{"TeamId":288272,"PlayerName":"Nazem Kadri","Points":18,"Goals":10,"Assists":16}

    public class PlayerModel
    {
        public int TeamId { get; set; }
        public string PlayerName { get; set; }
        public int Points { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
    }
}
