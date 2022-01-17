using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class SuccessMessage : UserauthServiceMessage
    {
        private const byte MessageNumber = 52; // SSH_MSG_USERAUTH_SUCCESS

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataWorker writer)
        {
        }
    }
}
