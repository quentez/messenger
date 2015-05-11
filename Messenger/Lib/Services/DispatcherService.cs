using System;
using GalaSoft.MvvmLight.Threading;

namespace Messenger.Lib.Services
{
    class DispatcherService : IDispatcherService
    {
        public void RunOnMainThead(Action action)
        {
            DispatcherHelper.RunAsync(action);
        }
    }
}