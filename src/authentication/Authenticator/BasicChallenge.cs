namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public class BasicChallenge
    {
        public BasicChallenge(string realm)
        {
            Realm = realm;
        }

        public string Realm { get; set; }

        public static string ToResponse(string realm)
        {
            return new BasicChallenge(realm).ToBasicString();
        }

        internal  string ToBasicString()
        {
            return string.Format("Basic realm=\"{0}\"", Realm);
        }
    }
}
