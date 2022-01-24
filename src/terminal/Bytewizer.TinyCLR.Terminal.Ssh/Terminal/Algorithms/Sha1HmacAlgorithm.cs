using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class HmacSha1Algorithm : HmacAlgorithm
    {
        private readonly HMACSHA1 _hmac =  new HMACSHA1();

        public string Name
        {
            get { return "hmac-sha1"; }
        }

        public uint DigestLength
        {
            get { return 20; }
        }

        public uint KeySize
        {
            get { return 20; }
        }

        public byte[] ComputeHash(uint packetNumber, byte[] data)
        {
            using (var worker = new SshDataWorker())
            {
                worker.Write(packetNumber);
                worker.Write(data);

                return _hmac.ComputeHash(worker.ToByteArray());
            }
        }
    }
}
