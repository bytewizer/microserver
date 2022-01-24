using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.PageBuilder
{
    /// <summary>
    /// HTML head.
    /// </summary>
    public class HtmlHead
    {        
        // Refer to https://www.w3schools.com/html/html_head.asp

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; } = null;

        /// <summary>
        /// Style.
        /// </summary>
        public string Style { get; set; } = null;

        /// <summary>
        /// Base URL.
        /// </summary>
        public string BaseUrl { get; set; } = null;

        /// <summary>
        /// Character set.
        /// </summary>
        public string MetaCharset { get; set; } = "UTF-8";

        /// <summary>
        /// Keywords.
        /// </summary>
        public string MetaKeywords { get; set; } = null;

        /// <summary>
        /// Description.
        /// </summary>
        public string MetaDescription { get; set; } = null;

        /// <summary>
        /// Author.
        /// </summary>
        public string MetaAuthor { get; set; } = null;

        /// <summary>
        /// Additional elements, which will be added to the HTML exactly as included in the list, in the order in which they appear in the list.
        /// Elements added to this list must be HTML formatted.
        /// </summary>
        public ArrayList AdditionalElements { get; private set; } = new ArrayList();

        /// <summary>
        /// HEAD content.
        /// </summary>
        public string Content
        {
            get
            {
                string ret =
                  "<head>";

                if (!String.IsNullOrEmpty(Title))
                    ret += "<title>" + Title + "</Title>";

                if (!String.IsNullOrEmpty(Style))
                    ret += "<style>" + Environment.NewLine + Style + Environment.NewLine + "</style>";

                if (!String.IsNullOrEmpty(BaseUrl))
                    ret += "<base href='" + BaseUrl + "'>";

                if (!String.IsNullOrEmpty(MetaCharset))
                    ret += "<meta charset='" + MetaCharset + "'>";

                if (!String.IsNullOrEmpty(MetaKeywords))
                    ret += "<meta name='keywords' content='" + MetaKeywords + "'>";

                if (!String.IsNullOrEmpty(MetaDescription))
                    ret += "<meta name='description' content='" + MetaDescription + "'>";

                if (!String.IsNullOrEmpty(MetaAuthor))
                    ret += "<meta name='author' content='" + MetaAuthor + "'>";

                if (AdditionalElements != null && AdditionalElements.Count > 0)
                {
                    foreach (string element in AdditionalElements)
                    {
                        ret += element;
                    }
                }

                ret +=
                    "</head>";

                return ret;
            }
        }

        /// <summary>
        /// Instantiate the object.
        /// </summary>
        public HtmlHead()
        {

        }
    }
}
