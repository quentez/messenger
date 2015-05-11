using System.Windows;

namespace Messenger.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public MainViewModel Main => ((App)Application.Current).ViewModelFactory.Resolve<MainViewModel>();
        public NotificationListViewModel NotificationList => ((App)Application.Current).ViewModelFactory.Resolve<NotificationListViewModel>();
    }
}