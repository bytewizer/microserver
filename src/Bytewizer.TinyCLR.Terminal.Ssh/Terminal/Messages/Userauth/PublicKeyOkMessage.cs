using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class PublicKeyOkMessage : UserauthServiceMessage
    {
        private const byte MessageNumber = 60; //SSH_MSG_USERAUTH_PK_OK

        public string KeyAlgorithmName { get; set; }
        public byte[] PublicKey { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataWorker writer)
        {
            writer.Write(KeyAlgorithmName, Encoding.UTF8);
            writer.WriteBinary(PublicKey);
        }
    }
}