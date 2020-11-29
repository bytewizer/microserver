using System;

namespace Bytewizer.TinyCLR.Http
{
    public class HttpCookieParser
    {
        private string _headerValue;
        private CookieCollection _cookies;
        private int _index;
        private string _cookieName = "";
        private ParserMethod _parserMethod;
        private string _cookieValue = "";

        private char Current
        {
            get
            {
                if (_index >= _headerValue.Length)
                    return char.MinValue;

                return _headerValue[_index];
            }
        }

        /// <summary>
        /// end of cookie string?
        /// </summary>
        protected bool IsEOF
        {
            get { return _index >= _headerValue.Length; }
        }

        /// <summary>
        /// Parse state method, remove all white spaces before the cookie name
        /// </summary>
        protected bool Name_Before()
        {
            while (CharExtensions.IsWhiteSpace(Current))
            {
                MoveNext();
            }

            _parserMethod = Name;
            return true;
        }

        /// <summary>
        /// Read cookie name until white space or equals are found
        /// </summary>
        protected virtual bool Name()
        {
            while (!CharExtensions.IsWhiteSpace(Current) && Current != '=')
            {
                _cookieName += Current;
                MoveNext();
            }

            _parserMethod = Name_After;
            return true;
        }

        /// <summary>
        /// Remove all white spaces until colon is found
        /// </summary>
        protected virtual bool Name_After()
        {
            while (CharExtensions.IsWhiteSpace(Current) || Current == ':')
            {
                MoveNext();
            }

            _parserMethod = Value_Before;
            return true;
        }

        /// <summary>
        /// Determine if the cookie value is quoted or regular.
        /// </summary>
        protected virtual bool Value_Before()
        {
            if (Current == '"')
            {
                _parserMethod = Value_Qouted;
            }
            else
            {
                _parserMethod = Value;

            }
            MoveNext();
            return true;
        }

        /// <summary>
        /// Read cookie value
        /// </summary>
        private bool Value()
        {
            while (Current != ';' && !IsEOF)
            {
                _cookieValue += Current;
                MoveNext();
            }

            _parserMethod = Value_After;
            return true;
        }

        /// <summary>
        /// Read cookie value qouted
        /// </summary>
        private bool Value_Qouted()
        {
            MoveNext(); // skip '"'

            var last = char.MinValue;
            while (Current != '"' && !IsEOF)
            {
                if (Current == '"' && last == '\\')
                {
                    _cookieValue += '#';
                    MoveNext();
                }
                else
                {
                    _cookieValue += Current;
                }

                last = Current;
                MoveNext();

            }

            _parserMethod = Value_After;
            return true;
        }


        private bool Value_After()
        {
            OnCookie(_cookieName, _cookieValue);
            _cookieName = "";
            _cookieValue = "";
            while (CharExtensions.IsWhiteSpace(Current) || Current == ';')
            {
                MoveNext();
            }

            _parserMethod = Name_Before;
            return true;
        }

        private void OnCookie(string name, string value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name == "") return; // ignore empty cookie names as defined in rfc 6265 http://tools.ietf.org/html/rfc6265

            _cookies.Add(name, value);
        }

        private void MoveNext()
        {
            if (!IsEOF)
                ++_index;
        }

        /// <summary>
        /// Parse cookie string
        /// </summary>
        /// <returns>A generated cookie collection.</returns>
        public ICookieCollection Parse(string value)
        {
            if (value == null) 
                throw new ArgumentNullException("value");

            _headerValue = value;
            _cookies = new CookieCollection();
            _parserMethod = Name_Before;

            while (!IsEOF)
            {
                _parserMethod();
            }

            OnCookie(_cookieName, _cookieValue);
            return _cookies;
        }

        /// <summary>
        /// Used to be able to quickly swap parser method.
        /// </summary>
        /// <returns></returns>
        private delegate bool ParserMethod();
    }
}
