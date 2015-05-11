using System.Windows;

namespace Messenger.Lib.Services
{
    public interface IDialogsService
    {
        bool AskUser(Window window, string message, string caption);
    }
}