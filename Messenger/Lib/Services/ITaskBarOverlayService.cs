using System.ComponentModel;
using System.Windows.Media;

namespace Messenger.Lib.Services
{
    public interface ITaskBarOverlayService : INotifyPropertyChanged
    {
        ImageSource TaskBarOverlay { get; }
        void UpdateBadgeOverlay(string newUnreadCount);
    }
}