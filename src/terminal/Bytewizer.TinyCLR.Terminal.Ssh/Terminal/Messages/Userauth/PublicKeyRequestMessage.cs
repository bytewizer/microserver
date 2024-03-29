﻿using Bytewizer.TinyCLR.Terminal;
using System;
using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class PublicKeyRequestMessage : RequestMessage
    {
        public bool HasSignature { get; private set; }
        public string KeyAlgorithmName { get; private set; }
        public byte[] PublicKey { get; private set; }
        public byte[] Signature { get; private set; }

        public byte[] PayloadWithoutSignature { get; private set; }

        protected override void OnLoad(SshDataWorker reader)
        {
            base.OnLoad(reader);

            if (MethodName != "publickey")
            {
                throw new ArgumentException(string.Format("Method name {0} is not valid.", MethodName));
            }

            HasSignature = reader.ReadBoolean();
            KeyAlgorithmName = reader.ReadString(Encoding.UTF8);
            PublicKey = reader.ReadBinary();

            if (HasSignature)
            {
                Signature = reader.ReadBinary();
                //PayloadWithoutSignature = RawBytes.Take(RawBytes.Length - Signature.Length - 5).ToArray(); ??
                PayloadWithoutSignature = RawBytes.Take(RawBytes.Length - Signature.Length - 5);
            }
        }
    }
}
