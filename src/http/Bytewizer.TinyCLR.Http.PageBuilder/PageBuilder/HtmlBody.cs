using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.PageBuilder
{
    /// <summary>
    /// HTML page body.
    /// </summary>
    public class HtmlBody
    {
        /// <summary>
        /// HTML body.
        /// </summary>
        public string Content { get; set; } = "";

        /// <summary>
        /// H1 text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string H1Text(string text, string id = null, string classId = null, string style = null)
        {
            string ret = "<h1";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</h1>";
            return ret;
        }

        /// <summary>
        /// H2 text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string H2Text(string text, string id = null, string classId = null, string style = null)
        {
            string ret = "<h2";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</h2>";
            return ret;
        }

        /// <summary>
        /// H3 text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string H3Text(string text, string id = null, string classId = null, string style = null)
        {
            string ret = "<h3";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</h3>";
            return ret;
        }

        /// <summary>
        /// Paragraph.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Preformatted(string text, string id = null, string classId = null, string style = null)
        {
            string ret = "<pre";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</pre>";
            return ret;
        }

        /// <summary>
        /// Paragraph.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Paragraph(string text, string id = null, string classId = null, string style = null)
        {
            string ret = "<p";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</p>";
            return ret;
        }

        /// <summary>
        /// Div.  
        /// Do not use within an encapsulating Paragraph.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Div(string text, string id = null, string classId = null, string style = null)
        {
            string ret = "<div";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</div>";
            return ret;
        }

        /// <summary>
        /// Link.
        /// </summary>
        /// <param name="textToDisplay">Text to display.</param>
        /// <param name="url">URL.</param>
        /// <param name="newWindow">Flag to indicate if the link should be opened in a new window.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Link(string textToDisplay, string url, bool newWindow = false, string id = null, string classId = null, string style = null)
        {
            string ret = "<a href='" + url + "'";
            if (newWindow) ret += " target='_blank'";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + textToDisplay + "</a>";
            return ret;
        }

        /// <summary>
        /// Image.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="url">URL.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Image(string text, string url, string id = null, string classId = null, string style = null)
        {
            string ret = "<img src='" + url + "'";
            if (!string.IsNullOrEmpty(text)) ret += " alt='" + text + "'";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += "/>";
            return ret;
        }

        /// <summary>
        /// Ordered list.
        /// </summary>
        /// <param name="items">Items.</param>
        /// <param name="listType">HTML list type, i.e. 1, A, a, I, or i.</param>
        /// <param name="emptyVal">Value to display if the list itself is empty.  If null, no list items will be added.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string OrderedList(ArrayList items, string listType = "1", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            string ret = "<ol";
            if (!string.IsNullOrEmpty(listType)) ret += " type='" + listType + "'";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">";
            if (items != null)
            {
                if (items.Count < 1 && !String.IsNullOrEmpty(emptyVal))
                {
                    ret += "<li>" + emptyVal + "</li>";
                }
                else
                {
                    foreach (string item in items)
                    {
                        ret += "<li>" + item + "</li>";
                    }
                }
            }
            ret += "</ol>";
            return ret;
        }

        /// <summary>
        /// Unordered list.
        /// </summary>
        /// <param name="items">Items.</param>
        /// <param name="listType">HTML list type, i.e. disc, circle, square, or none.</param>
        /// <param name="emptyVal">Value to display if the list itself is empty.  If null, no list items will be added.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string UnorderedList(ArrayList items, string listType = "disc", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            string ret = "<ul";
            if (!string.IsNullOrEmpty(listType)) ret += " type='" + listType + "'";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">";
            if (items != null)
            {
                if (items.Count < 1 && !String.IsNullOrEmpty(emptyVal))
                {
                    ret += "<li>" + emptyVal + "</li>";
                }
                else
                {
                    foreach (string item in items)
                    {
                        ret += "<li>" + item + "</li>";
                    }
                }
            }
            ret += "</ul>";
            return ret;
        }

        /// <summary>
        /// Button.
        /// </summary>
        /// <param name="textToDisplay">Text to display.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Button(string textToDisplay, string id = null, string classId = null, string style = null)
        {
            string ret = "<button";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            else ret += " style='background-color:#33bd55; color:white; padding:8px; border:none; outline:none'";
            ret += ">" + textToDisplay + "</button>";
            return ret;
        }

        /// <summary>
        /// Horizontal rule.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string HorizontalRule(string id = null, string classId = null, string style = null)
        {
            string ret = "<hr";
            if (!string.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!string.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!string.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">";
            return ret;
        }
    }
}