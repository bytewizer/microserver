using System;
using System.Text;
using System.Collections;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public class DigestAuthentication
    {
        internal Hashtable Parameters;

        public DigestAuthentication()
        {
            Parameters = new Hashtable();
        }

        public DigestAuthentication(Hashtable parameters)
        {
            Parameters = parameters;
        }

        public string Algorithm
        {
            get { return (string)Parameters["algorithm"]; }
        }

        public string Nonce
        {
            get { return (string)Parameters["nonce"]; }
        }

        public string Opaque
        {
            get { return (string)Parameters["opaque"]; }
        }

        public string Qop
        {
            get { return (string)Parameters["qop"]; }
        }

        public string Realm
        {
            get { return (string)Parameters["realm"]; }
        }

        public string Cnonce
        {
            get { return (string)Parameters["cnonce"]; }
        }

        public string Nc
        {
            get { return (string)Parameters["nc"]; }
        }

        public string Password
        {
            get { return (string)Parameters["password"]; }
        }

        public string Response
        {
            get { return (string)Parameters["response"]; }
        }

        public string Uri
        {
            get { return (string)Parameters["uri"]; }
        }

        public string UserName
        {
            get { return (string)Parameters["username"]; }
        }

        private static string CreateA1(string username, string password, string realm)
        {
            return string.Format("{0}:{1}:{2}", username, realm, password);
        }

        private static string CreateA1(
          string username, string password, string realm, string nonce, string cnonce)
        {
            return string.Format(
              "{0}:{1}:{2}", Hash(CreateA1(username, password, realm)), nonce, cnonce);
        }

        private static string CreateA2(string method, string uri)
        {
            return string.Format("{0}:{1}", method, uri);
        }

        private static string CreateA2(string method, string uri, string entity)
        {
            return string.Format("{0}:{1}:{2}", method, uri, Hash(entity));
        }

        internal static DigestAuthentication Parse(string value)
        {
            try
            {
                var cred = value.Split(new[] { ' ' }, 2);
                if (cred.Length != 2)
                    return null;

                var schm = cred[0].ToLower();

                return new DigestAuthentication(ParseParameters(cred[1]));
            }
            catch
            {
            }

            return null;
        }

        internal static string CreateRequestDigest(Hashtable parameters)
        {
            var user = (string)parameters["username"];
            var pass = (string)parameters["password"];
            var realm = (string)parameters["realm"];
            var nonce = (string)parameters["nonce"];
            var uri = (string)parameters["uri"];
            var algo = (string)parameters["algorithm"];
            var qop = (string)parameters["qop"];
            var cnonce = (string)parameters["cnonce"];
            var nc = (string)parameters["nc"];
            var method = (string)parameters["method"];

            var a1 = algo != null && algo.ToLower() == "md5-sess"
                     ? CreateA1(user, pass, realm, nonce, cnonce)
                     : CreateA1(user, pass, realm);

            var a2 = qop != null && qop.ToLower() == "auth-int"
                     ? CreateA2(method, uri, (string)parameters["entity"])
                     : CreateA2(method, uri);

            var secret = Hash(a1);
            var data = qop != null
                       ? string.Format("{0}:{1}:{2}:{3}:{4}", nonce, nc, cnonce, qop, Hash(a2))
                       : string.Format("{0}:{1}", nonce, Hash(a2));

            return Hash(string.Format("{0}:{1}", secret, data));
        }

        private static string Hash(string value)
        {
            var src = Encoding.UTF8.GetBytes(value);
            var md5 = MD5.Create();
            var hashed = md5.ComputeHash(src);

            var res = new StringBuilder(64);
            foreach (var b in hashed)
                res.Append(b.ToString("x2"));

            return res.ToString();
        }

        internal static Hashtable ParseParameters(string value)
        {
            var res = new Hashtable();

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var index = 0;
            var lastCh = char.MinValue;

            var name = "";
            var oldPos = 0;
            bool gotEquals = false;
            while (index < value.Length)
            {
                var ch = value[index];
                switch (ch)
                {
                    case '=':
                        if (gotEquals)
                            break;

                        gotEquals = true;
                        if (lastCh != '\\')
                        {
                            name = value.Substring(oldPos, index - oldPos).Trim(' ');
                            oldPos = index + 1;
                        }
                        break;
                    case ',':
                        gotEquals = false;
                        if (lastCh != '\\')
                        {
                            res.Add(name, value.Substring(oldPos, index - oldPos).Trim(' ', '"'));
                            name = "";
                            oldPos = index + 1;
                        }
                        break;
                }
                lastCh = value[index];
                ++index;
            }

            if (name != "")
            {
                res.Add(name, value.Substring(oldPos).Trim(' ', '"'));
            }

            return res;
        }
        
        internal static string CreateNonceValue()
        {
            var src = new byte[16];
            var rand = new Random();
            rand.NextBytes(src);

            var res = new StringBuilder(32);
            foreach (var b in src)
                res.Append(b.ToString("x2"));

            return res.ToString();
        }

        internal string RequestString()
        {
            var output = new StringBuilder(256);
            output.Append(string.Format(
              "Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", response=\"{4}\"",
              (string)Parameters["username"],
              (string)Parameters["realm"],
              (string)Parameters["nonce"],
              (string)Parameters["uri"],
              (string)Parameters["response"]));

            var opaque = (string)Parameters["opaque"];
            if (opaque != null)
                output.Append(string.Format(", opaque=\"{0}\"", opaque));

            var algo = (string)Parameters["algorithm"];
            if (algo != null)
                output.Append(string.Format(", algorithm={0}", algo));

            var qop = (string)Parameters["qop"];
            if (qop != null)
                output.Append(string.Format(
                  ", qop={0}, cnonce=\"{1}\", nc={2}", qop, (string)Parameters["cnonce"], (string)Parameters["nc"]));

            return output.ToString();
        }
    }
}

//if (digestResponse.Parameters.ContainsKey(Qop))
//{
//    // Check if auth-int present in qop string
//    int index1 = digestResponse.Parameters[Qop].IndexOf(AuthInt);
//    if (index1 != -1)
//    {
//        // Get index of auth if present in qop string
//        int index2 = digestResponse.Parameters[Qop].IndexOf(Auth);

//        // If index2 < index1, auth option is available
//        // If index2 == index1, check if auth option available later in string after auth-int.
//        if (index2 == index1)
//        {
//            index2 = digestResponse.Parameters[Qop].IndexOf(Auth, index1 + AuthInt.Length);
//            if (index2 == -1)
//            {
//                qop = AuthInt;
//            }
//        }
//    }
//}