using System;
using System.Text;
using System.IO;
using System.Collections;

namespace MicroServer.Net.Http.Mvc.Views
{
    /// <summary>
    ///     TokenParser is a class which implements a simple token replacement parser.
    /// </summary>
    /// <remarks>
    ///     TokenParser is used by the calling code by implementing an event handler for
    ///     the delegate TokenHandler(string strToken, ref string strReplacement)
    /// </remarks>
    public class TokenEngine
    {
        public TokenEngine(string content, IViewData tokens)
        {
            Content = content;
            Tokens = tokens;
            Default = string.Empty;
        }

        /// <summary>
        /// Gets token collection used for matching.
        /// </summary>
        public IViewData Tokens { get; private set; }

        /// <summary>
        /// Gets the unparsed text.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Gets and sets the default replacement value.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        ///     ExtractToken parses a token in the format "[%TOKENNAME%]". 
        /// </summary>
        /// <param name="token" type="string">
        ///     <para>
        ///         This is a token parsed from a text file in the format "[%TOKENNAME%]".
        ///     </para>
        /// </param>
        /// <returns>
        ///     It returns the string between the tokens "[%" and "%]"
        /// </returns>
        private string ExtractToken(string token)
        {
            int firstPos = token.IndexOf("[%") + 2;
            int secondPos = token.LastIndexOf("%]");
            string result = token.Substring(firstPos, secondPos - firstPos);

            return result.Trim();
        }

        /// <summary>
        ///     Parse() iterates through each character of the class variable "inputText"
        /// </summary>
        /// <returns>
        ///     Parse() returns a string representing inputText with its tokens exchanged
        ///     for the callking code's values.
        /// </returns>
        private String Parse()
        {
            const string tokenStart = "[";
            const string tokenNext = "%";
            const string tokenEnd = "]";

            String outText = String.Empty;
            String token = String.Empty;
            String replacement = String.Empty;

            int i = 0;
            string tok;
            string tok2;
            int len = Content.Length;

            while (i < len)
            {
                tok = Content[i].ToString();
                if (tok == tokenStart)
                {
                    i++;
                    tok2 = Content[i].ToString();
                    if (tok2 == tokenNext)
                    {
                        i--;
                        while (i < len & tok2 != tokenEnd)
                        {

                            tok2 = Content[i].ToString();
                            token += tok2;
                            i++;
                        }
                        OnToken(ExtractToken(token), ref replacement);
                        outText += replacement;
                        token = String.Empty;
                        tok = Content[i].ToString();
                    }
                }
                outText += tok;
                i++;
            }
            return outText;
        }

        /// <summary>
        ///     This is called to return the parsed text file.
        /// </summary>
        /// <returns>
        ///     A string representing the text file with all its tokens replaced by data
        ///     supplied by the calling code through the Tokenhandler delegate
        /// </returns>
        public override string ToString()
        {
            string result;

            try
            {
                result = Parse();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Subscribe to this event to receive request messages.
        /// </summary>
        public void OnToken(string token, ref string replacement)
        {
            if (Tokens.Count() > 0)
            {
                if (Tokens.Contains(token))
                    replacement = (string)Tokens[token];
            }
            else
            {
                replacement = Default;
            }
        }

        /// <summary>
        /// Subscribe to this event to receive request messages.
        /// </summary>
        public delegate void TokenHandler(string strToken, ref string strReplacement);
    }
}
