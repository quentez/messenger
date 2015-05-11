using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.Lib.Infrastructure
{
    public class AsyncSemaphore
    {
        public AsyncSemaphore(uint initialCount = 1)
        {
            this.currentCount = initialCount;
        }

        private readonly static Task CompletedTask = Task.FromResult(true);

        private readonly Queue<TaskCompletionSource<bool>> waitersQueue = new Queue<TaskCompletionSource<bool>>(); 
        private uint currentCount;

        public Task WaitAsync()
        {
            lock (this.waitersQueue)
            {
                if (this.currentCount > 0)
                {
                    this.currentCount--;
                    return CompletedTask;
                }

                var waiter = new TaskCompletionSource<bool>();
                this.waitersQueue.Enqueue(waiter);
                return waiter.Task;
            } 
        }

        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (this.waitersQueue)
            {
                if (this.waitersQueue.Count > 0)
                {
                    toRelease = this.waitersQueue.Dequeue();
                }
                else
                {
                    this.currentCount++;
                }
            }

            if (toRelease != null)
            {
                toRelease.SetResult(true); 
            }
        }
    }
}