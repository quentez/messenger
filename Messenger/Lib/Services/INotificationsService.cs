namespace Messenger.Lib.Services
{
    public interface INotificationsService
    {
        void ShowNotification(string title, string description, string conversationId);
    }
}