namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class KeyExchangeDhInitMessage : Message
    {
        private const byte MessageNumber = 30; // SSH_MSG_KEXDH_INIT

        public byte[] E { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataWorker reader)
        {
            E = reader.ReadMpint();
        }
    }
}
