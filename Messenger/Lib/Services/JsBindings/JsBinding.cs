using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.WinForms;
using GalaSoft.MvvmLight.Messaging;
using Messenger.Lib.Helpers;
using Messenger.Lib.Messages;
using Messenger.Lib.Services;
using Messenger.Lib.Services.JsBindings;
using Microsoft.Practices.Unity;

namespace FrontApp.Lib.Services.JsBindings
{
    abstract class JsBinding : IJsBinding
    {
        #region Public properties.

        [Dependency]
        [JavascriptIgnore]
        public IAppConstants AppConstants { get; set; }
        [Dependency]
        [JavascriptIgnore]
        public IJsonService JsonService { get; set; }
        [Dependency]
        [JavascriptIgnore]
        public IExternalProcessService ExternalProcessService { get; set; }
        [Dependency]
        [JavascriptIgnore]
        public IDispatcherService DispatcherService { get; set; }
        [Dependency]
        [JavascriptIgnore]
        public ITextHelpers TextHelpers { get; set; }
        [Dependency]
        [JavascriptIgnore]
        public IMessenger Messenger { get; set; }

        [JavascriptIgnore]
        public IJsBridgeService JsBridgeService { get; set; }
        [JavascriptIgnore]
        public ChromiumWebBrowser Browser { get; set; }

        #endregion

        #region Public methods.

        public void CopyText(string textToCopy)
        {
            // Make sure we can copy this string.
            if (string.IsNullOrWhiteSpace(textToCopy))
            {
                return;
            }

            // Copy the currently selected text.
            this.DispatcherService.RunOnMainThead(() => Clipboard.SetText(textToCopy));
        }

        public void PasteText()
        {
            // Paste at the currently selected location.
            this.Browser?.Paste();
        }

        public void ReplaceMisspelling(string word)
        {
            // Have the browser replace the currently selected misspelled word.
            this.Browser?.ReplaceMisspelling(word);
        }

        public void OpenExternalUrl(string url)
        {
            this.ExternalProcessService.OpenUrl(url);
        }

        public void OpenFile(string filename)
        {
            // Open the specified file.
            this.ExternalProcessService.OpenFile(filename);
        }

        public void UserSignedOut()
        {
            // Broadcast an event notifying windows that we've signed out.
            this.Messenger.Send(new UserSignedOutMessage());
        }

        #endregion

        #region Base class attribution.

        [JavascriptIgnore]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [JavascriptIgnore]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [JavascriptIgnore]
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}