namespace Messenger.Lib.Services
{
    public interface IExternalProcessService
    {
        void OpenUrl(string url);
        void OpenFile(string filename);
    }
}