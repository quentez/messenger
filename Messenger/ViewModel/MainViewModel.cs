using System;
using System.Threading.Tasks;
using CefSharp;
using GalaSoft.MvvmLight.Messaging;
using Messenger.Lib.Infrastructure;
using Messenger.Lib.Services;
using Messenger.Lib.Services.JsBindings;

namespace Messenger.ViewModel
{
    public class MainViewModel : BrowserViewModelBase<IJsMainBinding>, ILifeSpanHandler, IRequestHandler
    {
        #region Constructor & Private variables.

        public MainViewModel(IJsBridgeService jsBridgeService,
            ITaskBarOverlayService taskBarOverlayService,
            IExternalProcessService externalProcessService,
            IInjectScriptService injectScriptService,
            IMessenger messenger,
            IAppConstants appConstants)
            : base(jsBridgeService)
        {
            Ensure.Argument.IsNotNull(taskBarOverlayService, nameof(taskBarOverlayService));
            Ensure.Argument.IsNotNull(externalProcessService, nameof(externalProcessService));
            Ensure.Argument.IsNotNull(messenger, nameof(messenger));
            Ensure.Argument.IsNotNull(appConstants, nameof(appConstants));

            this.TaskBar = taskBarOverlayService;
            this.externalProcessService = externalProcessService;
            this.injectScriptService = injectScriptService;
            this.appConstants = appConstants;

            // Initialize variables.
            this.isLoading = true;

            // Create the browser for the main page.
            this.Browser.LifeSpanHandler = this;
            this.Browser.KeyboardHandler = this;
            this.Browser.RequestHandler = this;
            this.BaseUrl = this.appConstants.AppUrl + this.appConstants.SignInPath;

            // Register events on the browser.
            this.Browser.FrameLoadEnd += this.Browser_FrameLoadEnd;

            // Set the default title.
            this.SetSubtitle();

            // Initialize the inject script service.
            this.injectScriptService.InitializeAsync();
        }

        private readonly IExternalProcessService externalProcessService;
        private readonly IInjectScriptService injectScriptService;
        private readonly IAppConstants appConstants;
        private bool isLoading;
        private string title;

        public void SetSubtitle(string subtitle = null)
        {
            this.Title = "Messenger" + (string.IsNullOrWhiteSpace(subtitle) ? string.Empty : " - " + subtitle);
        }

        #endregion

        #region Observable properties.

        public bool IsLoading
        {
            get { return this.isLoading; }
            set { this.Set(() => this.IsLoading, ref this.isLoading, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { this.Set(() => this.Title, ref this.title, value); }
        }

        public ITaskBarOverlayService TaskBar { get; }

        #endregion

        #region Event handlers.

        private async void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs)
        {
            // Display the browser after the main page is loaded.
            if (!frameLoadEndEventArgs.IsMainFrame)
                return;

            // Inject our custom script in the page.
            var scriptToInject = await this.injectScriptService.GetScriptAsync();
            this.Browser.ExecuteScriptAsync(scriptToInject);

            // Display the browser.
            await Task.Delay(TimeSpan.FromSeconds(.5));
            this.IsLoading = false;
            this.Browser.SetFocus(false);
        }

        #endregion

        #region ILifeSpanHandler implementation.

        public bool OnBeforePopup(IWebBrowser browser, string url, ref int x, ref int y, ref int width, ref int height)
        {
            this.externalProcessService.OpenUrl(url);
            return true;
        }

        public void OnBeforeClose(IWebBrowser browser)
        {
            // Do nothing.
        }

        #endregion

        #region IRequestHandler implementation.

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
        {
            // Make sure this is an internal request.
            if (request.Url.StartsWith(this.appConstants.AppUrl)
                || request.Url.StartsWith(this.appConstants.SignInUrl))
                return false;

            // Otherwise, if it's a facebook url, open it externally.
            if (!request.Url.StartsWith(this.appConstants.ExternalAllowedUrl))
                return true;

            this.externalProcessService.OpenUrl(request.Url);
            return true;
        }

        public bool OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            return false;
        }

        public void OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest request, IResponse response)
        {
            return false;
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        public bool OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, WebPluginInfo info)
        {
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
        }

        #endregion
    }
}