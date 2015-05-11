using System;

namespace Messenger.Lib.Services
{
    public interface IDispatcherService
    {
        void RunOnMainThead(Action action);
    }
}