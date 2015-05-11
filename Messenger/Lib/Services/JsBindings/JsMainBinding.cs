using Messenger.Lib.Helpers;
using Messenger.Lib.Infrastructure;
using Messenger.ViewModel;

namespace Messenger.Lib.Services.JsBindings
{
    class JsMainBinding : JsBinding, IJsMainBinding
    {
        public JsMainBinding(ITaskBarOverlayService taskBarOverlayService, 
            INotificationsService notificationsService, 
            IViewModelFactory viewModelFactory, 
            IDispatcherService dispatcherService, 
            ITextHelpers textHelpers)
        {
            Ensure.Argument.IsNotNull(taskBarOverlayService, nameof(taskBarOverlayService));
            Ensure.Argument.IsNotNull(notificationsService, nameof(notificationsService));
            Ensure.Argument.IsNotNull(viewModelFactory, nameof(viewModelFactory));
            Ensure.Argument.IsNotNull(dispatcherService, nameof(dispatcherService));
            Ensure.Argument.IsNotNull(textHelpers, nameof(textHelpers));

            this.taskBarOverlayService = taskBarOverlayService;
            this.notificationsService = notificationsService;
            this.viewModelFactory = viewModelFactory;
            this.dispatcherService = dispatcherService;
            this.textHelpers = textHelpers;
        }

        private readonly ITaskBarOverlayService taskBarOverlayService;
        private readonly INotificationsService notificationsService;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IDispatcherService dispatcherService;
        private readonly ITextHelpers textHelpers;

        public void ShowNotification(string title, string description, string conversationId)
        {
            // Use the notifications service to display this notification on the desktop.
            this.notificationsService.ShowNotification(
                this.textHelpers.SanitizeInput(title),
                this.textHelpers.SanitizeInput(description), 
                this.textHelpers.SanitizeInput(conversationId));
        }

        public void UpdateTitle(string newTitle)
        {
            if (newTitle == null)
                return;
            
            // TODO: Find a better way to do this.
            this.dispatcherService.RunOnMainThead(() => this.viewModelFactory.Resolve<MainViewModel>().SetSubtitle(this.textHelpers.SanitizeInput(newTitle)));
        }

        public void UpdateBadge(int badgeCount)
        {
            // Use the corresponding service to update the app's icon overlay.
            this.taskBarOverlayService.UpdateBadgeOverlay(badgeCount);
        }
    }
}