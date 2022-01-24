using System;
using System.Text;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public abstract class PublicKeyAlgorithm
    {
        public PublicKeyAlgorithm(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var bytes = Convert.FromBase64String(key);
                //ImportKey(bytes);
            }
        }

        public abstract string Name { get; }

        public string GetFingerprint()
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(CreateKeyAndCertificatesData());
                return BitConverter.ToString(bytes).Replace("-", ":");
            }
        }

        public byte[] GetSignature(byte[] signatureData)
        {
            if (signatureData == null)
            {
                throw new ArgumentNullException(nameof(signatureData));
            }

            using (var worker = new SshDataWorker(signatureData))
            {
                if (worker.ReadString(Encoding.UTF8) != Name)
                { 
                    throw new Exception("Signature was not created with this algorithm."); 
                }

                var signature = worker.ReadBinary();
                return signature;
            }
        }

        public byte[] CreateSignatureData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var worker = new SshDataWorker())
            {
                //var signature = SignData(data);

                worker.Write(Name, Encoding.UTF8);
                //worker.WriteBinary(signature);

                return worker.ToByteArray();
            }
        }

        public abstract void ImportKey(RSAParameters key);

        public abstract RSAParameters ExportKey();

        public abstract void LoadKeyAndCertificatesData(byte[] data);

        public abstract byte[] CreateKeyAndCertificatesData();
    }
}
