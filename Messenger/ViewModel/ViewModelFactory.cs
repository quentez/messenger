using GalaSoft.MvvmLight;
using Messenger.Lib.Infrastructure;
using Microsoft.Practices.Unity;

namespace Messenger.ViewModel
{
    class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(IUnityContainer container)
        {
            Ensure.Argument.IsNotNull(container, "container");
            this.container = container;
        }

        private readonly IUnityContainer container;

        public T Resolve<T>() where T : ViewModelBase
        {
            return this.container.Resolve<T>();
        }
    }
}