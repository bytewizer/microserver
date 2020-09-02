using System;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public class DigestChallenge
    {
        public DigestChallenge(string realm)
        {
            Realm = realm;
            Nonce = CreateNonceValue();
            Algorithm = "MD5";
            Qop = "auth";
        }

        public string Domain { get; set; }

        public string Stale { get; set; }

        public string Realm { get; set; }

        public string Nonce { get; set; }

        public string Opaque { get; set; }

        public string Algorithm { get; set; }

        public string Qop { get; set; }

        public static string ToResponse(string realm)
        {
           return new DigestChallenge(realm).ToDigestString();
        }
        
        private string CreateNonceValue()
        {
            var src = new byte[16];
            var rand = new Random();
            rand.NextBytes(src);

            var res = new StringBuilder(32);
            foreach (var b in src)
                res.Append(b.ToString("x2"));

            return res.ToString();
        }

        internal string ToDigestString()
        {
            var output = new StringBuilder(128);

            var domain = Domain;
            if (domain != null)
                output.Append(string.Format(
                  "Digest realm=\"{0}\", domain=\"{1}\", nonce=\"{2}\"",
                  Realm,
                  domain,
                  Nonce));
            else
                output.Append(string.Format(
                  "Digest realm=\"{0}\", nonce=\"{1}\"", Realm, Nonce));

            var opaque = Opaque;
            if (opaque != null)
                output.Append(string.Format(", opaque=\"{0}\"", opaque));

            var stale = Stale;
            if (stale != null)
                output.Append(string.Format(", stale={0}", stale));

            var algo = Algorithm;
            if (algo != null)
                output.Append(string.Format(", algorithm={0}", algo));

            var qop = Qop;
            if (qop != null)
                output.Append(string.Format(", qop=\"{0}\"", qop));

            return output.ToString();
        }
    }
}
