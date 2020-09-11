using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.Http.Cookies
{
    public class SetCookieHeaderValue
    {
        private const string DomainToken = "domain";
        private const string ExpiresToken = "expires";
        private const string MaxAgeToken = "max-age";
        private const string PathToken = "path";
        private const string SecureToken = "secure";
        private const string HttpOnlyToken = "httponly";

        public SetCookieHeaderValue(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Name = name;
            Value = value;
        }

        public string Name { get; }
        
        public string Value { get; }

        public string Comment { get; set; }

        public bool Discard { get; set; }

        public string Domain { get; set; }

        public bool Expired { get; set; }

        public DateTime Expires { get; set; }

        public bool HttpOnly { get; set; }

        public TimeSpan MaxAge { get; set; }

        public bool Secure { get; set; }

        public string Path { get; set; }

        public DateTime TimeStamp { get; }

        public int Version { get; }

        internal string GetHeaderValue()
        {
            var sb = new StringBuilder();

            sb.Append(Name);
            sb.Append('=');
            sb.Append(Value);

            if (Comment != null)
            {
                sb.Append($"; Comment={Domain}");
            }

            if (Domain != null)
            {
                sb.Append($"; {DomainToken}={Domain}");
            }

            //if (Expires != null)
            //{
            //    //sb.Append($"; {ExpiresToken}={Expires.GetValueOrDefault().ToUtcString()}");
            //}

            //if (MaxAge != null)
            //{
            //    sb.Append($"; {MaxAgeToken}={MaxAge}");
            //}

            if (Path != null)
            {
                sb.Append($"; {PathToken}={Path}");
            }

            if (Secure)
            {
                sb.Append($"; {SecureToken}");
            }

            if (HttpOnly)
            {
                sb.Append($"; {HttpOnlyToken}");
            }

            return sb.ToString();
        }

        private static long GetMaxAge(SetCookieHeaderValue cookie)
        {
            var maxAge = (long)cookie.Expires.Subtract(cookie.TimeStamp).TotalSeconds;
            if (maxAge < 0L)
            {
                maxAge = -1L;
            }

            return maxAge;
        }

    }
}
