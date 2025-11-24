using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCanvas.Runtime.EventLoop
{
    /// <summary>
    /// Event loop implementation for worker threads.
    /// Processes tasks in a FIFO queue on a dedicated thread.
    /// </summary>
    public class WorkerThreadEventLoop : IEventLoop
    {
        private readonly BlockingCollection<Action> _queue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private int _threadId = -1;
        private volatile bool _isRunning;

        public WorkerThreadEventLoop()
        {
            _queue = new BlockingCollection<Action>();
            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = false;
        }

        /// <inheritdoc/>
        public bool IsCurrentThread => Thread.CurrentThread.ManagedThreadId == _threadId;

        /// <inheritdoc/>
        public bool IsRunning => _isRunning;

        /// <inheritdoc/>
        public void Post(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (_cancellationTokenSource.IsCancellationRequested)
            {
                throw new InvalidOperationException("Event loop has been stopped");
            }

            _queue.Add(action);
        }

        /// <inheritdoc/>
        public Task<T> PostAsync<T>(Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var tcs = new TaskCompletionSource<T>();

            Post(() =>
            {
                try
                {
                    var result = func();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task PostAsync(Func<Task> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var tcs = new TaskCompletionSource<bool>();

            Post(async () =>
            {
                try
                {
                    await func();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <inheritdoc/>
        public void Run()
        {
            if (_isRunning)
            {
                throw new InvalidOperationException("Event loop is already running");
            }

            _threadId = Thread.CurrentThread.ManagedThreadId;
            _isRunning = true;

            try
            {
                foreach (var action in _queue.GetConsumingEnumerable(_cancellationTokenSource.Token))
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue processing
                        Console.Error.WriteLine($"Event loop task error: {ex}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when Stop() is called
            }
            finally
            {
                _isRunning = false;
            }
        }

        /// <inheritdoc/>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _queue.CompleteAdding();
        }

        public void Dispose()
        {
            Stop();
            _queue.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}
