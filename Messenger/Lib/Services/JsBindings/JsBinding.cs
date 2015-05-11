using CefSharp;
using CefSharp.WinForms;

namespace Messenger.Lib.Services.JsBindings
{
    abstract class JsBinding : IJsBinding
    {
        #region Public properties.

        [JavascriptIgnore]
        public IJsBridgeService JsBridgeService { get; set; }
        [JavascriptIgnore]
        public ChromiumWebBrowser Browser { get; set; }

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