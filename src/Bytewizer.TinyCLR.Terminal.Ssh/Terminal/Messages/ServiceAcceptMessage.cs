using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class ServiceAcceptMessage : Message
    {
        private const byte MessageNumber = 6; // SSH_MSG_SERVICE_ACCEPT

        public ServiceAcceptMessage(string name)
        {
            ServiceName = name;
        }

        public string ServiceName { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataWorker writer)
        {
            writer.Write(ServiceName, Encoding.UTF8);
        }
    }
}