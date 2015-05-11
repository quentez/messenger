using System;

namespace Messenger.Lib.Helpers
{
    class TextHelpers : ITextHelpers
    {
        public string JsEscape(string src)
        {
            // Escape quotes.
            return src?
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
        }

        public string SanitizeInput(string src)
        {
            src = src.Trim();
            src = src.Replace(Environment.NewLine, " ");
            src = src.Substring(0, Math.Min(400, src.Length));

            return src;
        }
    }
}