#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Represents a Web Worker - a script that runs in a background thread.
    /// Workers provide a simple means for web content to run scripts in background threads.
    /// The worker thread can perform tasks without interfering with the user interface.
    /// </summary>
    public class Worker : IDisposable
    {
        private readonly Queue<WorkerMessage> _messageQueue;
        private readonly object _queueLock = new object();
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _workerTask;
        private bool _isTerminated;

        /// <summary>
        /// Event handler for messages from the worker
        /// </summary>
        public event EventHandler<MessageEvent>? OnMessage;

        /// <summary>
        /// Event handler for errors from the worker
        /// </summary>
        public event EventHandler<ErrorEvent>? OnError;

        /// <summary>
        /// Creates a new Worker
        /// </summary>
        public Worker()
        {
            _messageQueue = new Queue<WorkerMessage>();
            _cancellationTokenSource = new CancellationTokenSource();
            _isTerminated = false;
        }

        /// <summary>
        /// Posts a message to the worker
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="transfer">Optional list of transferable objects</param>
        public void postMessage(object message, List<ITransferable>? transfer = null)
        {
            if (_isTerminated)
            {
                throw new InvalidOperationException("Cannot post message to terminated worker");
            }

            var workerMessage = new WorkerMessage
            {
                Data = message,
                Transfer = transfer
            };

            // Handle transferable objects
            if (transfer != null)
            {
                foreach (var transferable in transfer)
                {
                    if (transferable.IsNeutered)
                    {
                        throw new InvalidOperationException("Cannot transfer an already neutered object");
                    }
                    transferable.Neuter();
                }
            }

            lock (_queueLock)
            {
                _messageQueue.Enqueue(workerMessage);
                Monitor.Pulse(_queueLock);
            }
        }

        /// <summary>
        /// Starts the worker with the specified task
        /// </summary>
        /// <param name="workerTask">The task to execute in the worker</param>
        public void Start(Action<Worker> workerTask)
        {
            if (_workerTask != null)
            {
                throw new InvalidOperationException("Worker already started");
            }

            _workerTask = Task.Run(() =>
            {
                try
                {
                    workerTask(this);
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, new ErrorEvent
                    {
                        Message = ex.Message,
                        Error = ex
                    });
                }
            }, _cancellationTokenSource!.Token);
        }

        /// <summary>
        /// Waits for and retrieves the next message from the queue
        /// </summary>
        public WorkerMessage? ReceiveMessage()
        {
            lock (_queueLock)
            {
                while (_messageQueue.Count == 0 && !_isTerminated)
                {
                    Monitor.Wait(_queueLock);
                }

                if (_messageQueue.Count > 0)
                {
                    return _messageQueue.Dequeue();
                }

                return null;
            }
        }

        /// <summary>
        /// Posts a message back to the main thread (used from within worker)
        /// </summary>
        public void SendToMainThread(object message)
        {
            OnMessage?.Invoke(this, new MessageEvent { Data = message });
        }

        /// <summary>
        /// Terminates the worker immediately
        /// </summary>
        public void terminate()
        {
            if (_isTerminated)
            {
                return;
            }

            _isTerminated = true;
            _cancellationTokenSource?.Cancel();

            lock (_queueLock)
            {
                Monitor.PulseAll(_queueLock);
            }

            _workerTask?.Wait(1000); // Wait up to 1 second for clean shutdown
        }

        public void Dispose()
        {
            terminate();
            _cancellationTokenSource?.Dispose();
        }
    }

    /// <summary>
    /// Represents a message passed to/from a worker
    /// </summary>
    public class WorkerMessage
    {
        public object? Data { get; set; }
        public List<ITransferable>? Transfer { get; set; }
    }

    /// <summary>
    /// Event data for message events
    /// </summary>
    public class MessageEvent : EventArgs
    {
        public object? Data { get; set; }
    }

    /// <summary>
    /// Event data for error events
    /// </summary>
    public class ErrorEvent : EventArgs
    {
        public string? Message { get; set; }
        public Exception? Error { get; set; }
    }
}
