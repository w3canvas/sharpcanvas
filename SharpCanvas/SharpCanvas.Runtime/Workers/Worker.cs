#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpCanvas.Runtime.EventLoop;

namespace SharpCanvas.Runtime.Workers
{
    /// <summary>
    /// Represents a Web Worker - a script that runs in a background thread with its own event loop.
    /// Workers provide a simple means for web content to run scripts in background threads.
    /// The worker thread can perform tasks without interfering with the user interface.
    /// Messages are processed on the worker's event loop thread, ensuring proper ordering.
    /// </summary>
    public class Worker : IDisposable
    {
        private readonly Queue<WorkerMessage> _messageQueue;
        private readonly object _queueLock = new object();
        private readonly WorkerThreadEventLoop _eventLoop;
        private readonly IEventLoop? _mainThreadEventLoop;
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
        /// Creates a new Worker with its own event loop
        /// </summary>
        /// <param name="mainThreadEventLoop">Optional main thread event loop for posting messages back</param>
        public Worker(IEventLoop? mainThreadEventLoop = null)
        {
            _messageQueue = new Queue<WorkerMessage>();
            _cancellationTokenSource = new CancellationTokenSource();
            _isTerminated = false;
            _eventLoop = new WorkerThreadEventLoop();
            _mainThreadEventLoop = mainThreadEventLoop;
        }

        /// <summary>
        /// Gets the event loop for this worker
        /// </summary>
        public IEventLoop EventLoop => _eventLoop;

        /// <summary>
        /// Posts a message to the worker. Messages are processed on the worker's event loop.
        /// Transferable objects (OffscreenCanvas, ImageBitmap) are transferred with zero-copy semantics.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="transfer">Optional list of transferable objects to transfer</param>
        public void postMessage(object message, List<ITransferable>? transfer = null)
        {
            if (_isTerminated)
            {
                throw new InvalidOperationException("Cannot post message to terminated worker");
            }

            // Handle transferable objects BEFORE queuing
            // This ensures proper zero-copy transfer semantics
            if (transfer != null)
            {
                foreach (var transferable in transfer)
                {
                    if (transferable.IsNeutered)
                    {
                        throw new InvalidOperationException("Cannot transfer an already neutered object");
                    }
                    // Neuter on the calling thread to prevent further use
                    transferable.Neuter();
                }
            }

            var workerMessage = new WorkerMessage
            {
                Data = message,
                Transfer = transfer
            };

            // Post to event loop for processing
            _eventLoop.Post(() =>
            {
                lock (_queueLock)
                {
                    _messageQueue.Enqueue(workerMessage);
                    Monitor.Pulse(_queueLock);
                }
            });
        }

        /// <summary>
        /// Starts the worker with the specified task.
        /// The worker task runs on a dedicated thread with its own event loop.
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
                    // Start the worker task in parallel with the event loop
                    var userTaskThread = new Thread(() =>
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
                    });
                    userTaskThread.Start();

                    // Run the event loop on this thread (blocks until stopped)
                    _eventLoop.Run();

                    // Wait for user task to complete
                    userTaskThread.Join(1000);
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
        /// Posts a message back to the main thread (used from within worker).
        /// If a main thread event loop was provided, messages are posted to it for proper ordering.
        /// Otherwise, the OnMessage event is invoked directly.
        /// </summary>
        public void SendToMainThread(object message)
        {
            if (_mainThreadEventLoop != null)
            {
                // Post to main thread event loop for proper ordering
                _mainThreadEventLoop.Post(() =>
                {
                    OnMessage?.Invoke(this, new MessageEvent { Data = message });
                });
            }
            else
            {
                // No event loop provided, invoke directly
                OnMessage?.Invoke(this, new MessageEvent { Data = message });
            }
        }

        /// <summary>
        /// Terminates the worker immediately.
        /// Stops the event loop and cancels any pending work.
        /// </summary>
        public void terminate()
        {
            if (_isTerminated)
            {
                return;
            }

            _isTerminated = true;
            _cancellationTokenSource?.Cancel();

            // Stop the event loop
            _eventLoop.Stop();

            lock (_queueLock)
            {
                Monitor.PulseAll(_queueLock);
            }

            _workerTask?.Wait(1000); // Wait up to 1 second for clean shutdown
        }

        public void Dispose()
        {
            terminate();
            _eventLoop.Dispose();
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
