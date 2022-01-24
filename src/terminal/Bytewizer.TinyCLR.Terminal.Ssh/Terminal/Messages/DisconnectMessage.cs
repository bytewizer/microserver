using System;
using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class DisconnectMessage : Message
    {
        private const byte MessageNumber = 1; // SSH_MSG_DISCONNECT

        public DisconnectMessage()
        {
        }

        public DisconnectMessage(DisconnectReason reasonCode, string description = "", string language = "en")
        {
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            if (string.IsNullOrEmpty(language))
            {
                throw new ArgumentNullException(nameof(language));
            }

            ReasonCode = reasonCode;
            Description = description;
            Language = language;
        }

        public DisconnectReason ReasonCode { get; private set; }
        public string Description { get; private set; }
        public string Language { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataWorker reader)
        {
            ReasonCode = (DisconnectReason)reader.ReadUInt32();
            Description = reader.ReadString(Encoding.UTF8);
            if (reader.DataAvailable >= 4)
                Language = reader.ReadString(Encoding.UTF8);
        }

        protected override void OnGetPacket(SshDataWorker writer)
        {
            writer.Write((uint)ReasonCode);
            writer.Write(Description, Encoding.UTF8);
            writer.Write(Language ?? "en", Encoding.UTF8);
        }
    }
}
