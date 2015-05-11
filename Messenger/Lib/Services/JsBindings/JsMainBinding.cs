using System;
using System.Diagnostics;
using Messenger.Lib.Infrastructure;
using Messenger.ViewModel;

namespace Messenger.Lib.Services.JsBindings
{
    class JsMainBinding : JsBinding, IJsMainBinding
    {
        public JsMainBinding(ITaskBarOverlayService taskBarOverlayService, 
            INotificationsService notificationsService, 
            IViewModelFactory viewModelFactory, 
            IDispatcherService dispatcherService)
        {
            Ensure.Argument.IsNotNull(taskBarOverlayService, nameof(taskBarOverlayService));
            Ensure.Argument.IsNotNull(notificationsService, nameof(notificationsService));

            this.taskBarOverlayService = taskBarOverlayService;
            this.notificationsService = notificationsService;
            this.viewModelFactory = viewModelFactory;
            this.dispatcherService = dispatcherService;
        }

        private readonly ITaskBarOverlayService taskBarOverlayService;
        private readonly INotificationsService notificationsService;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IDispatcherService dispatcherService;

        public void ShowNotification(string title, string description, string link)
        {
            // Use the notifications service to display this notification on the desktop.
            this.notificationsService.ShowNotification(title, description, link);
        }

        public void UpdateTitle(string newTitle)
        {
            if (newTitle == null)
                return;
            
            // TODO: Find a better way to do this.
            newTitle = newTitle.Trim();
            newTitle = newTitle.Substring(0, Math.Min(400, newTitle.Length));
            this.dispatcherService.RunOnMainThead(() => this.viewModelFactory.Resolve<MainViewModel>().SetSubtitle(newTitle));
        }

        public void UpdateBadge(int badgeCount)
        {
            // Use the corresponding service to update the app's icon overlay.
            this.taskBarOverlayService.UpdateBadgeOverlay(badgeCount);
        }
    }
}