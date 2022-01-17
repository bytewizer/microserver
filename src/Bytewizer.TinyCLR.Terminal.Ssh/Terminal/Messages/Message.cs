using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public abstract class Message
    {
        public abstract byte MessageType { get; }

        protected byte[] RawBytes { get; set; }

        public void Load(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            RawBytes = bytes;
            using (var worker = new SshDataWorker(bytes))
            {
                var number = worker.ReadByte();
                if (number != MessageType)
                {
                    throw new ArgumentException(string.Format("Message type {0} is not valid.", number));
                }

                OnLoad(worker);
            }
        }

        public byte[] GetPacket()
        {
            using (var worker = new SshDataWorker())
            {
                worker.Write(MessageType);

                OnGetPacket(worker);

                return worker.ToByteArray();
            }
        }

        public static KeyExchangeInitMessage LoadFromKeyExchangeInitMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var msg = new KeyExchangeInitMessage();
            msg.Load(message.RawBytes);
            return msg;
        }

        protected virtual void OnLoad(SshDataWorker reader)
        {
            throw new NotSupportedException();
        }

        protected virtual void OnGetPacket(SshDataWorker writer)
        {
            throw new NotSupportedException();
        }
    }
}
