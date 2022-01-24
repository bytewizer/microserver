//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;

//using Bytewizer.TinyCLR.Logging;
//using Bytewizer.TinyCLR.SecureShell.Algorithms;
//using Bytewizer.TinyCLR.SecureShell.Messages;
//using Bytewizer.TinyCLR.Sockets;
//using Bytewizer.TinyCLR.Terminal.Features;

//namespace Bytewizer.TinyCLR.Terminal
//{
//    public class SecureShellSession
//    {
//        private uint _outboundFlow;
//        private uint _inboundFlow;

//        private readonly ILogger _logger;
//        private readonly TerminalContext _context;
//        private readonly TelnetServerOptions _terminalOptions;

//        public SecureShellSession(
//                ILogger logger,
//                TerminalContext context,
//                TerminalServerOptions terminalOptions)
//        {
//            _logger = logger;
//            _context = context;
//            _terminalOptions = terminalOptions;

//            EstablishConnection();

//        }

//        internal void EstablishConnection()
//        {
//            _context.Channel.Write($"SSH-2.0-TinyCLR_1.0.0\r\n");

//            using (var reader = new StreamReader(_context.Channel.InputStream))
//            {
//                var protocolVersionExchange = reader.ReadLine();
//                Debug.WriteLine(protocolVersionExchange);
//            }

//            var initMessage = new KeyExchangeInitMessage()
//            {
//                EncryptionAlgorithmsClientToServer = new string[] { "3des-cbc" },
//                EncryptionAlgorithmsServerToClient = new string[] { "3des-cbc" },
//                CompressionAlgorithmsClientToServer = new string[] { "none" },
//                CompressionAlgorithmsServerToClient = new string[] { "none" },
//                MacAlgorithmsClientToServer = new string[] { "hmac-sha1" },
//                MacAlgorithmsServerToClient = new string[] { "hmac-sha1" },
//                KeyExchangeAlgorithms = new string[] { "diffie-hellman-group14-sha1" },
//                ServerHostKeyAlgorithms = new string[] { "ssh-rsa" },
//                LanguagesClientToServer = new string[] { " " },
//                LanguagesServerToClient = new string[] { " " },
//                FirstKexPacketFollows = false
//            };

//            var messageBytes = initMessage.GetPacket();
//            _context.Channel.Write(messageBytes);

//        }


//        private class Algorithms
//        {
//            public KexAlgorithm KeyExchange;
//            public PublicKeyAlgorithm PublicKey;
//            public EncryptionAlgorithm ClientEncryption;
//            public EncryptionAlgorithm ServerEncryption;
//            public HmacAlgorithm ClientHmac;
//            public HmacAlgorithm ServerHmac;
//            public CompressionAlgorithm ClientCompression;
//            public CompressionAlgorithm ServerCompression;
//        }

//        private class ExchangeContext
//        {
//            public string KeyExchange;
//            public string PublicKey;
//            public string ClientEncryption;
//            public string ServerEncryption;
//            public string ClientHmac;
//            public string ServerHmac;
//            public string ClientCompression;
//            public string ServerCompression;

//            public byte[] ClientKexInitPayload;
//            public byte[] ServerKexInitPayload;

//            public Algorithms NewAlgorithms;
//        }
//    }
//}