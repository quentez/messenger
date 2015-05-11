using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ObjectBuilder2;

namespace Messenger.ViewModel
{
    public class NotificationListViewModel : ViewModelBase
    {
        #region Constructor & Private fields.

        public NotificationListViewModel()
        {
            // Create the notifications observable collection.
            this.Notifications = new ObservableCollection<NotificationItemViewModel>();
        }

        #endregion

        #region Obervable properties.

        public ObservableCollection<NotificationItemViewModel> Notifications { get; }

        #endregion

        #region Public methods.

        public void AddNotification(string title, string description, Action clickAction)
        {
            // Start by clearing expired notifications.
            var expiredNotifications = this.Notifications.Where(n => !n.IsPlaying).ToList();
            expiredNotifications.ForEach(n => this.Notifications.Remove(n));

            // If we have 4 notifications or more, stop here.
            if (this.Notifications.Count >= 4)
            {
                return;
            }

            // Find the visual index for this new notification.
            var visualIndex = 0;
            this.Notifications
                .OrderBy(n => n.VisualIndex)
                .ForEach(n =>
            {
                if (n.VisualIndex == visualIndex)
                {
                    visualIndex = n.VisualIndex + 1;
                }
            });

            // Create the ViewModel for this notification and insert it in the collection.
            this.Notifications.Add(new NotificationItemViewModel
            {
                Title = title,
                Description = description,
                ClickAction = clickAction,
                VisualIndex = visualIndex
            });
        }

        #endregion
    }
}