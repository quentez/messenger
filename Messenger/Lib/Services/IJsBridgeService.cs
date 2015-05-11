using CefSharp.WinForms;
using Messenger.Lib.Services.JsBindings;

namespace Messenger.Lib.Services
{
    public interface IJsBridgeService
    {
        T RegisterBrowser<T>(ChromiumWebBrowser browser) where T : IJsBinding;
    }
}