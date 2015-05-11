using GalaSoft.MvvmLight;

namespace Messenger.ViewModel
{
    public interface IViewModelFactory
    {
        T Resolve<T>() where T : ViewModelBase;
    }
}