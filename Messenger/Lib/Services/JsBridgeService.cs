using CefSharp;
using CefSharp.WinForms;
using FrontApp.Lib.Services.JsBindings;
using Messenger.Lib.Helpers;
using Messenger.Lib.Infrastructure;
using Messenger.Lib.Services.JsBindings;
using Messenger.Lib.UIServices;
using Messenger.ViewModel;

namespace Messenger.Lib.Services
{
    class JsBridgeService : IJsBridgeService
    {
        #region Constructor & Private variables.

        public JsBridgeService(IViewModelFactory viewModelFactory, 
            IJsBindingFactory jsBindingFactory,
            IWindowService windowService,
            ITextHelpers textHelpers,
            IAppConstants appConstants)
        {
            Ensure.Argument.IsNotNull(viewModelFactory, "viewModelFactory");
            Ensure.Argument.IsNotNull(jsBindingFactory, "jsBindingFactory");
            Ensure.Argument.IsNotNull(windowService, "windowService");
            Ensure.Argument.IsNotNull(textHelpers, "textHelpers");
            Ensure.Argument.IsNotNull(appConstants, "appConstants");

            this.viewModelFactory = viewModelFactory;
            this.jsBindingFactory = jsBindingFactory;
            this.windowService = windowService;
            this.textHelpers = textHelpers;
            this.appConstants = appConstants;
        }

        private readonly IViewModelFactory viewModelFactory;
        private readonly IJsBindingFactory jsBindingFactory;
        private readonly IWindowService windowService;
        private readonly ITextHelpers textHelpers;
        private readonly IAppConstants appConstants;
        private IWebBrowser mainBrowser;
        
        #endregion

        #region Interface implementation & Public methods.
        
        public T RegisterBrowser<T>(ChromiumWebBrowser browser) where T : IJsBinding
        {
            Ensure.Argument.IsNotNull(browser, "browser");

            // Create the requested binding.
            var binding = this.jsBindingFactory.Resolve<T>();
            binding.JsBridgeService = this;
            binding.Browser = browser;

            // If this is the main biding, keep a reference to the browser.
            if (binding is IJsMainBinding)
            {
                this.mainBrowser = browser;
            }

            // Register the browser with this binding.
            browser.RegisterJsObject("dotnet", binding);

            return binding;
        }

        #endregion
    }
}