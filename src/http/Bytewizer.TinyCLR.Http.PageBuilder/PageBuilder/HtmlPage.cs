using System;

namespace Bytewizer.TinyCLR.Http.PageBuilder
{
    /// <summary>
    /// HTML response page.
    /// </summary>
    public class HtmlPage
    {

        /// <summary>
        /// HTML document head.
        /// </summary>
        public HtmlHead Head { get; private set; } = new HtmlHead();

        /// <summary>
        /// HTML document body.
        /// </summary>
        public HtmlBody Body { get; private set; } = new HtmlBody();


        /// <summary>
        /// Instantiate the object.
        /// </summary>
        public HtmlPage()
        {

        }

        /// <summary>
        /// Create an HTML string from the document.
        /// </summary>
        /// <returns>HTML string.</returns>
        public override string ToString()
        {
            string ret =
                "<!DOCTYPE html>" +
                "<html>" +
                Head.Content +
                "<body>" +
                Body.Content +
                "</body></html>";
            return ret;
        }
    }
}
