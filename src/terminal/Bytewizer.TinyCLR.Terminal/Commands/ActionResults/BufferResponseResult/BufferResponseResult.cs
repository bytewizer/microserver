using System;
using System.Text;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// An action result which sends buffered response content to the client. 
    /// </summary>
    public class BufferResponseResult : ResponseResult
    {
        private const string OverflowPromptFormat = @"Paging {0}%: [M]ore, [P]revious, [R]epeat, [A]ll, Enter to quit. > ";

        /// <summary>
        /// Initializes a new default instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        public BufferResponseResult()
        {
            Response = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        public BufferResponseResult(StringBuilder content)
        {
            Response = content;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        public BufferResponseResult(string content)
        {
            Response = new StringBuilder(content);
        }

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.TerminalContext.Response.Write("");     
        }

        /// <summary>
        /// Parses the text and checks to see if the number of lines is over the threshold
        /// if it is then it splits the text and returns the first portion. The rest of the
        /// text is passed back to the calling code in the out parameter BufferedText
        /// </summary>
        /// <param name="bufferLines">The text to parse.</param>
        /// <param name="appendLastLineFeed">Indicates if the last line should be appended with crlf.</param>
        /// <param name="appendOverflowIndicator">Indicates if the last line should be appended with the more indicator.</param>
        /// <param name="currentPosition">Current position in the list.</param>
        /// <param name="totalRows">Total rows in the output buffer.</param>
        /// <returns>Returns the text to return to the user.</returns>
        private static string Format(
            string[] bufferLines,
            bool appendLastLineFeed,
            bool appendOverflowIndicator,
            int currentPosition,
            int totalRows)
        {
            Debug.Assert(totalRows > 0);
            Debug.Assert(currentPosition >= 0);
            Debug.Assert(bufferLines.Length > 0);

            // The buffered output was already processed for ANSI and such. Using StringBuilder instead of AnsiBuilder here is
            // intended, to avoid double-processing. If we did not avoid double-processing, then things like help files trying
            // to explain how to use <%tokens%> for ANSI via escaped <% and %> sections would instead get replaced in processing.
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < bufferLines.Length - 1; i++)
            {
                output.AppendLine(bufferLines[i] + AnsiSequences.NewLine);
            }

            // Last line without NewLine; if we're not appending another overflow indicator, then this is assumed to be a prompt,
            // and if we are appending overflow indicator, we'll add the new line before adding said prompt.
            //output.Append(bufferLines[^1]);

            if (appendOverflowIndicator)
            {
                output.Append(AnsiSequences.NewLine);
                output.Append(FormatOverflowPrompt(currentPosition, totalRows));
            }

            return output.ToString();
        }

        private static string FormatOverflowPrompt(int currentPosition, int totalRows)
        {
            int percent = currentPosition * 100 / totalRows;
            return string.Format(OverflowPromptFormat, percent);
        }
    }
}