namespace Messenger.Lib.Services.JsBindings
{
    public interface IJsBindingFactory
    {
        T Resolve<T>() where T : IJsBinding;
    }
}