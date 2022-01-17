﻿using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class ChannelRequestMessage : ConnectionServiceMessage
    {
        private const byte MessageNumber = 98; // SSH_MSG_CHANNEL_REQUEST

        public uint RecipientChannel { get; set; }
        public string RequestType { get; set; }
        public bool WantReply { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataWorker reader)
        {
            RecipientChannel = reader.ReadUInt32();
            RequestType = reader.ReadString(Encoding.UTF8);
            WantReply = reader.ReadBoolean();
        }

        protected override void OnGetPacket(SshDataWorker writer)
        {
            writer.Write(RecipientChannel);
            writer.Write(RequestType, Encoding.UTF8);
            writer.Write(WantReply);
        }
    }
}