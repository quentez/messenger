using Messenger.Lib.Infrastructure;
using Messenger.Lib.UIServices;
using Messenger.ViewModel;

namespace Messenger.Lib.Services
{
    class NotificationsService : INotificationsService
    {
        public NotificationsService(IDispatcherService dispatcherService,
            IJsBridgeService jsBridgeService, 
            IWindowService windowService,
            IViewModelFactory viewModelFactory)
        {
            Ensure.Argument.IsNotNull(dispatcherService, "dispatcherService");
            Ensure.Argument.IsNotNull(jsBridgeService, "jsBridgeService");
            Ensure.Argument.IsNotNull(windowService, "windowService");
            Ensure.Argument.IsNotNull(viewModelFactory, "viewModelFactory");

            this.dispatcherService = dispatcherService;
            this.jsBridgeService = jsBridgeService;
            this.windowService = windowService;
            this.viewModelFactory = viewModelFactory;
        }

        private readonly IDispatcherService dispatcherService;
        private readonly IJsBridgeService jsBridgeService;
        private readonly IWindowService windowService;
        private readonly IViewModelFactory viewModelFactory;

        public void ShowNotification(string title, string description, string link)
        {
            // Get the singleton Notifications ViewModel and add the notification.
            this.dispatcherService.RunOnMainThead(() => this.viewModelFactory.Resolve<NotificationListViewModel>().AddNotification(title, description, () =>
            {
                // On click, open the link in the main browser using the JS bridge.
                //this.jsBridgeService.Open(link);

                // Then bring the main window to the front.
                this.windowService.ActivateMainWindow();
            }));
        }
    }
}