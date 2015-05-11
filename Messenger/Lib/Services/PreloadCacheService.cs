using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CefSharp;
using Messenger.Lib.Infrastructure;

namespace Messenger.Lib.Services
{
    class PreloadCacheService : IPreloadCacheService
    {
        #region Constructor & Private variables.

        public PreloadCacheService(IAppConstants appConstants)
        {
            Ensure.Argument.IsNotNull(appConstants, nameof(appConstants));
            this.appConstants = appConstants;
            this.preloadRoutes = new ConcurrentDictionary<Regex, Uri>();
            this.preloadedResponses = new ConcurrentDictionary<Uri, string>();
        }

        private readonly IAppConstants appConstants;
        private readonly IDictionary<Regex, Uri> preloadRoutes; 
        private readonly IDictionary<Uri, string> preloadedResponses;

        #endregion

        #region Interface implementation.

        public async Task AddAndPreloadResourceAsync(Regex route, Uri resource)
        {
            // Add the specified route to our collection.
            this.preloadRoutes[route] = resource;

            // Preload it right away.
            await this.PreloadRouteAsync(resource);
        }

        #endregion

        #region Private methods.

        private async Task PreloadRouteAsync(Uri routeTarget)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(this.appConstants.UserAgent);

                var response = await client.GetAsync(routeTarget);
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                this.preloadedResponses[routeTarget] = responseBody;
            }
            catch
            {
            }
        }

        #endregion

        #region IResourceHandler implementation.

        public void RegisterHandler(string url, ResourceHandler handler)
        {
            throw new NotImplementedException();
        }

        public void UnregisterHandler(string url)
        {
            throw new NotImplementedException();
        }

        public ResourceHandler GetResourceHandler(IWebBrowser browser, IRequest request)
        {
            // Find a corresponding route for this request.
            var route = this.preloadRoutes.Keys.FirstOrDefault(r => r.IsMatch(request.Url));
            if (route == null)
            {
                return null;
            }

            // Check if we have a cached response for this route.
            var routeTarget = this.preloadRoutes[route];
            string cachedResponse;
            if (!this.preloadedResponses.TryGetValue(routeTarget, out cachedResponse))
            {
                // If we couldn't find a cached response, queue this route to be reloaded.
                Task.Run(() => this.PreloadRouteAsync(routeTarget));
                return null;
            }

            // If a cached response was found, use it to create a resource handler.
            return ResourceHandler.FromString(cachedResponse);
        }

        #endregion
    }
}