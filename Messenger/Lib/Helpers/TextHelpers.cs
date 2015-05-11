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
    }
}