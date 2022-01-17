using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class FailureMessage : UserauthServiceMessage 
    {
        private const byte MessageNumber = 51; // SSH_MSG_USERAUTH_FAILURE

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataWorker writer)
        {
            writer.Write("password,publickey", Encoding.UTF8);
            writer.Write(false);
        }
    }
}