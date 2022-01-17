using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class KeyExchangeDhReplyMessage : Message
    {
        private const byte MessageNumber = 31; // SSH_MSG_KEXDH_REPLY

        public byte[] HostKey { get; set; }
        public byte[] F { get; set; }
        public byte[] Signature { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataWorker writer)
        {
            writer.WriteBinary(HostKey);
            writer.WriteMpint(F);
            writer.WriteBinary(Signature);
        }
    }
}
