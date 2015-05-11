using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;

namespace Messenger.Lib.Services
{
    class DispatcherService : IDispatcherService
    {
        public void RunOnMainThead(Action action)
        {
            DispatcherHelper.RunAsync(action);
        }

        public void RunAllRenderTasks()
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Loaded, new Action(() => { }));
        }
    }
}