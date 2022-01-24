using System;
using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class KeyExchangeInitMessage : Message
    {
        private const byte MessageNumber = 20; // SSH_MSG_KEXINIT

        private static readonly Random _rng = new Random();

        public KeyExchangeInitMessage()
        {
            Cookie = new byte[16];
            _rng.NextBytes(Cookie);
        }

        public byte[] Cookie { get; private set; }

        public string[] KeyExchangeAlgorithms { get; set; }

        public string[] ServerHostKeyAlgorithms { get; set; }

        public string[] EncryptionAlgorithmsClientToServer { get; set; }

        public string[] EncryptionAlgorithmsServerToClient { get; set; }

        public string[] MacAlgorithmsClientToServer { get; set; }

        public string[] MacAlgorithmsServerToClient { get; set; }

        public string[] CompressionAlgorithmsClientToServer { get; set; }

        public string[] CompressionAlgorithmsServerToClient { get; set; }

        public string[] LanguagesClientToServer { get; set; }

        public string[] LanguagesServerToClient { get; set; }

        public bool FirstKexPacketFollows { get; set; }

        public uint Reserved { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataWorker reader)
        {
            Cookie = reader.ReadBinary(16);
            KeyExchangeAlgorithms = reader.ReadString(Encoding.UTF8).Split(',');
            ServerHostKeyAlgorithms = reader.ReadString(Encoding.UTF8).Split(',');
            EncryptionAlgorithmsClientToServer = reader.ReadString(Encoding.UTF8).Split(',');
            EncryptionAlgorithmsServerToClient = reader.ReadString(Encoding.UTF8).Split(',');
            MacAlgorithmsClientToServer = reader.ReadString(Encoding.UTF8).Split(',');
            MacAlgorithmsServerToClient = reader.ReadString(Encoding.UTF8).Split(',');
            CompressionAlgorithmsClientToServer = reader.ReadString(Encoding.UTF8).Split(',');
            CompressionAlgorithmsServerToClient = reader.ReadString(Encoding.UTF8).Split(',');
            LanguagesClientToServer = reader.ReadString(Encoding.UTF8).Split(',');
            LanguagesServerToClient = reader.ReadString(Encoding.UTF8).Split(',');
            FirstKexPacketFollows = reader.ReadBoolean();
            Reserved = reader.ReadUInt32();
        }

        protected override void OnGetPacket(SshDataWorker writer)
        {

            writer.Write(Cookie);
            writer.Write(KeyExchangeAlgorithms.Join(","), Encoding.UTF8);
            writer.Write(ServerHostKeyAlgorithms.Join(","), Encoding.UTF8);
            writer.Write(EncryptionAlgorithmsClientToServer.Join(","), Encoding.UTF8);
            writer.Write(EncryptionAlgorithmsServerToClient.Join(","), Encoding.UTF8);
            writer.Write(MacAlgorithmsClientToServer.Join(","), Encoding.UTF8);
            writer.Write(MacAlgorithmsServerToClient.Join(","), Encoding.UTF8);
            writer.Write(CompressionAlgorithmsClientToServer.Join(","), Encoding.UTF8);
            writer.Write(CompressionAlgorithmsServerToClient.Join(","), Encoding.UTF8);
            writer.Write(LanguagesClientToServer.Join(","), Encoding.UTF8);
            writer.Write(LanguagesServerToClient.Join(","), Encoding.UTF8);
            writer.Write(FirstKexPacketFollows);
            writer.Write(Reserved);
        }
    }
}
