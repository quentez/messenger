using Messenger.Lib.Services.JsBindings;

namespace FrontApp.Lib.Services.JsBindings
{
    public interface IJsBindingFactory
    {
        T Resolve<T>() where T : IJsBinding;
    }
}