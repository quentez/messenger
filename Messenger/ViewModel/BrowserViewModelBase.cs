using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Messenger.Lib.Infrastructure;
using Messenger.Lib.Services;
using Messenger.Lib.Services.JsBindings;
using Microsoft.Practices.Unity;

namespace Messenger.ViewModel
{
    public abstract class BrowserViewModelBase<T> : ViewModelBase, IKeyboardHandler, IMenuHandler where T : IJsBinding
    {
        #region Constructor & Private variables.

        protected BrowserViewModelBase(IJsBridgeService jsBridgeService)
        {
            Ensure.Argument.IsNotNull(jsBridgeService, "jsBridgeService");
            this.isOpen = true;

            // Create our browser.
            this.Browser = new ChromiumWebBrowser(string.Empty)
            {
                KeyboardHandler = this,
                MenuHandler = this
            };

            // Register events on the browser.
            this.Browser.IsBrowserInitializedChanged += this.Browser_Initialized;

            // Create the binding.
            this.JsBinding = jsBridgeService.RegisterBrowser<T>(this.Browser);

            // Commands.
            this.WindowActivatedCommand = new RelayCommand(() =>
            {
                // Focus browser on UI thread.
                DispatcherHelper.RunAsync(() => this.Browser.SetFocus(true));
            });
            this.ClosedCommand = new RelayCommand<Window>(window =>
            {
                // Dispose the browser when the window gets closed.
                if (this.Browser.IsDisposed || this.Browser.Disposing)
                {
                    return;
                }
                this.Browser.Dispose();
            });
        }

        [Dependency]
        public IDialogsService DialogsService { get; set; }
        [Dependency]
        public IAppConstants AppConstants { get; set; }

        private bool isOpen;

        #endregion

        #region Public properties.

        /// <summary>
        /// The CEF browser that we manage.
        /// </summary>
        public ChromiumWebBrowser Browser { get; }

        /// <summary>
        /// The first URL the browser should navigate to.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// The binding for our current browser.
        /// </summary>
        public T JsBinding { get; set; }

        /// <summary>
        /// Indicates whether this window should close or not.
        /// </summary>
        public bool IsOpen
        {
            get { return this.isOpen; }
            set { this.Set(() => this.IsOpen, ref this.isOpen, value); }
        }

        #endregion

        #region Event handlers.

        private void Browser_Initialized(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            // Check if we need to load a page right away or not.
            if (string.IsNullOrWhiteSpace(this.BaseUrl))
            {
                return;
            }

            // Load the base page.
            this.Browser.Load(this.BaseUrl);
        }

        #endregion

        #region Commands.

        /// <summary>
        /// Focuses the browser when the window gets activated.
        /// </summary>
        public RelayCommand WindowActivatedCommand { get; }

        /// <summary>
        /// Called when the Window was closed.
        /// </summary>
        public RelayCommand<Window> ClosedCommand { get; }

        #endregion

        #region IKeyboardHandler implementation.

        public bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, CefEventFlags modifiers, bool isSystemKey)
        {
            // Handle keypress to display DevTools when needed.
            if (type.HasFlag(KeyType.KeyUp)
                && (Keys)code == Keys.I
                && modifiers.HasFlag(CefEventFlags.ControlDown)
                && modifiers.HasFlag(CefEventFlags.ShiftDown))
            {
                this.Browser.ShowDevTools();
                return true;
            }

            // Handle keypress to reload the page.
            if (type.HasFlag(KeyType.KeyUp)
                && (Keys)code == Keys.R
                && modifiers.HasFlag(CefEventFlags.ControlDown))
            {
                this.Browser.Reload();
                return true;
            }

            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, bool isKeyboardShortcut)
        {
            return false;
        }

        #endregion

        #region IMenuHandler implementation.

        public bool OnBeforeContextMenu(IWebBrowser browser, IContextMenuParams parameters)
        {
            return false;
        }

        #endregion
    }
}