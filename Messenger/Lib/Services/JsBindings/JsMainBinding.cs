using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Messenger.Lib.Infrastructure;
using Messenger.Lib.Services;

namespace FrontApp.Lib.Services.JsBindings
{
    class JsMainBinding : JsBinding, IJsMainBinding
    {
        public JsMainBinding(ITaskBarOverlayService taskBarOverlayService, 
            INotificationsService notificationsService, 
            IPreloadCacheService preloadCacheService)
        {
            Ensure.Argument.IsNotNull(taskBarOverlayService, nameof(taskBarOverlayService));
            Ensure.Argument.IsNotNull(notificationsService, nameof(notificationsService));
            Ensure.Argument.IsNotNull(preloadCacheService, nameof(preloadCacheService));

            this.taskBarOverlayService = taskBarOverlayService;
            this.notificationsService = notificationsService;
            this.preloadCacheService = preloadCacheService;
        }

        private readonly ITaskBarOverlayService taskBarOverlayService;
        private readonly INotificationsService notificationsService;
        private readonly IPreloadCacheService preloadCacheService;

        public void ShowNotification(string title, string description, string link)
        {
            // Use the notifications service to display this notification on the desktop.
            this.notificationsService.ShowNotification(title, description, link);
        }

        public void UpdateBadge(string badgeCount)
        {
            // Use the corresponding service to update the app's icon overlay.
            this.taskBarOverlayService.UpdateBadgeOverlay(badgeCount);
        }

        public void PreloadResource(string resource, string route)
        {
            Ensure.Argument.IsNotNullOrWhiteSpace(resource, nameof(resource));
            Ensure.Argument.IsNotNull(route, nameof(route));

            // Create a Uri using the provided resource.
            var appUri = new Uri(this.AppConstants.AppUrl);
            Uri resourceUri;
            if (!Uri.TryCreate(appUri, resource, out resourceUri))
            {
                return;
            }

            // Create a regular expression using the provided route.
            if (route.StartsWith("/"))
            {
                route = this.AppConstants.AppUrl + route;
            }
            route = route
                .Replace("/", "\\/")
                .Replace("?", "\\?");

            Regex routeRegex;
            try
            {
                routeRegex = new Regex(route);
            }
            catch
            {
                return;
            }

            // Preload this resource using the corresponding service.
            Task.Run(() => this.preloadCacheService.AddAndPreloadResourceAsync(routeRegex, resourceUri));
        }
    }
}