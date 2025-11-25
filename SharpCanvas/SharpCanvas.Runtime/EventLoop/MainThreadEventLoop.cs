using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCanvas.Runtime.EventLoop
{
    /// <summary>
    /// Event loop implementation for the main thread.
    /// For console applications, this executes tasks synchronously on the calling thread.
    /// </summary>
    public class MainThreadEventLoop : IEventLoop
    {
        private readonly int _mainThreadId;
        private volatile bool _isRunning;

        public MainThreadEventLoop()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
            _isRunning = false;
        }

        /// <inheritdoc/>
        public bool IsCurrentThread => Thread.CurrentThread.ManagedThreadId == _mainThreadId;

        /// <inheritdoc/>
        public bool IsRunning => _isRunning;

        /// <inheritdoc/>
        public void Post(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            // For main thread in console apps, execute synchronously
            if (IsCurrentThread)
            {
                action();
            }
            else
            {
                // If called from another thread, use Task.Run to marshal back
                Task.Run(action);
            }
        }

        /// <inheritdoc/>
        public Task<T> PostAsync<T>(Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (IsCurrentThread)
            {
                try
                {
                    return Task.FromResult(func());
                }
                catch (Exception ex)
                {
                    var tcs = new TaskCompletionSource<T>();
                    tcs.SetException(ex);
                    return tcs.Task;
                }
            }
            else
            {
                return Task.Run(func);
            }
        }

        /// <inheritdoc/>
        public Task PostAsync(Func<Task> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (IsCurrentThread)
            {
                return func();
            }
            else
            {
                return Task.Run(func);
            }
        }

        /// <inheritdoc/>
        public void Run()
        {
            // Main thread event loop doesn't block
            // In a real GUI app (WPF, WinForms), this would pump messages
            _isRunning = true;
        }

        /// <inheritdoc/>
        public void Stop()
        {
            _isRunning = false;
        }
    }
}
