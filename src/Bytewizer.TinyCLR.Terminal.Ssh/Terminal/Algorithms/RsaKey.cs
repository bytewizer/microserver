using System;
using System.Text;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class RsaKey : PublicKeyAlgorithm
    {
        private readonly RSACryptoServiceProvider _algorithm = new RSACryptoServiceProvider();

        public RsaKey(string key = null)
            : base(key)
        {
        }

        public override string Name
        {
            get { return "ssh-rsa"; }
        }

        public override void ImportKey(RSAParameters key)
        {
            _algorithm.ImportParameters(key);
        }

        public override RSAParameters ExportKey()
        {
            return _algorithm.ExportParameters(true);
        }

        public override void LoadKeyAndCertificatesData(byte[] data)
        {
            using (var worker = new SshDataWorker(data))
            {
                if (worker.ReadString(Encoding.UTF8) != Name)
                {
                    throw new Exception("Key and certificates were not created with this algorithm.");
                }

                var args = new RSAParameters();
                args.Exponent = worker.ReadMpint();
                args.Modulus = worker.ReadMpint();

                _algorithm.ImportParameters(args);
            }
        }

        public override byte[] CreateKeyAndCertificatesData()
        {
            using (var worker = new SshDataWorker())
            {
                var args = _algorithm.ExportParameters(false);

                worker.Write(Name, Encoding.UTF8);
                worker.WriteMpint(args.Exponent);
                worker.WriteMpint(args.Modulus);

                return worker.ToByteArray();
            }
        }
    }
}
