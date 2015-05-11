namespace Messenger.Lib.Helpers
{
    public interface ITextHelpers
    {
        string JsEscape(string src);
        string SanitizeInput(string src);
    }
}