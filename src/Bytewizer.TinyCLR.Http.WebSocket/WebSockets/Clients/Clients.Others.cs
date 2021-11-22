using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    public partial class Clients : IClientsOthers
    {
        /// <summary>
        /// Provides methods to access caller operations.
        /// </summary>
        public IClientsOthers Others { get { return this; } }

        void IClientsOthers.SendText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
