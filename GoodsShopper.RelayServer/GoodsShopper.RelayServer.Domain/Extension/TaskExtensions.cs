using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoodsShopper.RelayServer.Domain.Extension
{
    public static class TaskExtensions
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, int timeoutInMillis)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                if (task == await Task.WhenAny(task, Task.Delay(timeoutInMillis, cancellationTokenSource.Token)))
                {
                    cancellationTokenSource.Cancel();

                    return await task;
                }
                else
                {
                    throw new TimeoutException();
                }
            }
        }

        public static async Task TimeoutAfter(this Task task, int timeoutInMillis)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                if (task == await Task.WhenAny(task, Task.Delay(timeoutInMillis, cancellationTokenSource.Token)))
                {
                    cancellationTokenSource.Cancel();
                }
                else
                {
                    throw new TimeoutException();
                }
            }
        }
    }
}
