using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Represents a SharedWorker - a worker that can be accessed from multiple browsing contexts
    /// (windows, iframes, or other workers) as long as they are in the same origin.
    /// Unlike dedicated Workers, SharedWorkers can be shared between multiple scripts.
    /// </summary>
    public class SharedWorker : IDisposable
    {
        private static readonly ConcurrentDictionary<string, SharedWorkerInstance> _instances = new ConcurrentDictionary<string, SharedWorkerInstance>();

        private readonly SharedWorkerInstance _instance;
        private readonly MessagePort _port;

        /// <summary>
        /// The port used to communicate with the shared worker
        /// </summary>
        public MessagePort port => _port;

        /// <summary>
        /// Event handler for errors from the shared worker
        /// </summary>
        public event EventHandler<ErrorEvent>? OnError;

        /// <summary>
        /// Creates a new SharedWorker or connects to an existing one with the same name
        /// </summary>
        /// <param name="name">The name of the shared worker</param>
        /// <param name="workerTask">The task to execute (only used if creating a new instance)</param>
        public SharedWorker(string name, Action<SharedWorkerGlobalScope>? workerTask = null)
        {
            _instance = _instances.GetOrAdd(name, n => new SharedWorkerInstance(n, workerTask));
            _port = _instance.CreatePort();

            _port.OnError += (sender, e) => OnError?.Invoke(sender, e);
        }

        /// <summary>
        /// Starts the shared worker (if not already running)
        /// </summary>
        public void Start()
        {
            _instance.Start();
        }

        public void Dispose()
        {
            _port.Dispose();
        }
    }

    /// <summary>
    /// Represents a message port for communicating with a SharedWorker
    /// </summary>
    public class MessagePort : IDisposable
    {
        private readonly Queue<WorkerMessage> _messageQueue;
        private readonly object _queueLock = new object();
        private bool _isClosed;

        /// <summary>
        /// Event handler for messages
        /// </summary>
        public event EventHandler<MessageEvent>? OnMessage;

        /// <summary>
        /// Event handler for errors
        /// </summary>
        public event EventHandler<ErrorEvent>? OnError;

        public MessagePort()
        {
            _messageQueue = new Queue<WorkerMessage>();
            _isClosed = false;
        }

        /// <summary>
        /// Posts a message through this port
        /// </summary>
        public void postMessage(object message, List<ITransferable>? transfer = null)
        {
            if (_isClosed)
            {
                throw new InvalidOperationException("Cannot post message to closed port");
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
        /// Starts the port (required before messages will be received)
        /// </summary>
        public void start()
        {
            // In the browser API, this is required to start receiving messages
            // In our implementation, we're always started, but we keep this for API compatibility
        }

        /// <summary>
        /// Closes this port
        /// </summary>
        public void close()
        {
            _isClosed = true;
            lock (_queueLock)
            {
                Monitor.PulseAll(_queueLock);
            }
        }

        internal WorkerMessage? ReceiveMessage()
        {
            lock (_queueLock)
            {
                while (_messageQueue.Count == 0 && !_isClosed)
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

        internal void SendMessage(object message)
        {
            OnMessage?.Invoke(this, new MessageEvent { Data = message });
        }

        internal void SendError(string message, Exception? error)
        {
            OnError?.Invoke(this, new ErrorEvent { Message = message, Error = error });
        }

        public void Dispose()
        {
            close();
        }
    }

    /// <summary>
    /// The global scope within a SharedWorker
    /// </summary>
    public class SharedWorkerGlobalScope
    {
        private readonly List<MessagePort> _ports = new List<MessagePort>();
        private readonly object _portsLock = new object();

        /// <summary>
        /// Event handler fired when a new connection is made to the shared worker
        /// </summary>
        public event EventHandler<ConnectEvent>? OnConnect;

        /// <summary>
        /// The name of this shared worker
        /// </summary>
        public string Name { get; }

        internal SharedWorkerGlobalScope(string name)
        {
            Name = name;
        }

        internal void AddPort(MessagePort port)
        {
            lock (_portsLock)
            {
                _ports.Add(port);
            }
            OnConnect?.Invoke(this, new ConnectEvent { Port = port });
        }

        internal void RemovePort(MessagePort port)
        {
            lock (_portsLock)
            {
                _ports.Remove(port);
            }
        }

        /// <summary>
        /// Closes the shared worker
        /// </summary>
        public void close()
        {
            lock (_portsLock)
            {
                foreach (var port in _ports)
                {
                    port.close();
                }
                _ports.Clear();
            }
        }
    }

    /// <summary>
    /// Event data for connect events
    /// </summary>
    public class ConnectEvent : EventArgs
    {
        public MessagePort? Port { get; set; }
    }

    /// <summary>
    /// Internal instance of a shared worker
    /// </summary>
    internal class SharedWorkerInstance
    {
        private readonly string _name;
        private readonly Action<SharedWorkerGlobalScope>? _workerTask;
        private readonly SharedWorkerGlobalScope _globalScope;
        private readonly List<MessagePort> _ports;
        private readonly object _portsLock = new object();
        private Task? _task;
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isStarted;

        public SharedWorkerInstance(string name, Action<SharedWorkerGlobalScope>? workerTask)
        {
            _name = name;
            _workerTask = workerTask;
            _globalScope = new SharedWorkerGlobalScope(name);
            _ports = new List<MessagePort>();
            _cancellationTokenSource = new CancellationTokenSource();
            _isStarted = false;
        }

        public MessagePort CreatePort()
        {
            var port = new MessagePort();
            lock (_portsLock)
            {
                _ports.Add(port);
            }
            _globalScope.AddPort(port);
            return port;
        }

        public void Start()
        {
            if (_isStarted || _workerTask == null)
            {
                return;
            }

            _isStarted = true;
            _task = Task.Run(() =>
            {
                try
                {
                    _workerTask(_globalScope);
                }
                catch (Exception ex)
                {
                    // Notify all ports of the error
                    lock (_portsLock)
                    {
                        foreach (var port in _ports)
                        {
                            port.SendError(ex.Message, ex);
                        }
                    }
                }
            }, _cancellationTokenSource!.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _task?.Wait(1000);

            lock (_portsLock)
            {
                foreach (var port in _ports)
                {
                    port.close();
                }
                _ports.Clear();
            }
        }
    }
}
