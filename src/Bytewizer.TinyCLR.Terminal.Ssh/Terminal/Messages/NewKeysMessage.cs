namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class NewKeysMessage : Message
    {
        private const byte MessageNumber = 21; // SSH_MSG_NEWKEYS

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataWorker reader)
        {
        }

        protected override void OnGetPacket(SshDataWorker writer)
        {
        }
    }
}
