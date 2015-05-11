using System.Collections.Generic;
using CefSharp.WinForms;

namespace Messenger.Lib.Services.JsBindings
{
    public interface IJsBinding
    {
        IJsBridgeService JsBridgeService { get; set; }
        ChromiumWebBrowser Browser { get; set; }
    }
}