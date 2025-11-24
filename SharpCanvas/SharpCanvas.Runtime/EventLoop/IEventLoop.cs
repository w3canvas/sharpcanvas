using System;
using System.Threading.Tasks;

namespace SharpCanvas.Runtime.EventLoop
{
    /// <summary>
    /// Represents an event loop that processes tasks and messages in order.
    /// Different implementations exist for different contexts (main thread, worker threads, etc.)
    /// </summary>
    public interface IEventLoop
    {
        /// <summary>
        /// Posts an action to be executed on this event loop.
        /// The action will be queued and executed in order.
        /// </summary>
        /// <param name="action">The action to execute</param>
        void Post(Action action);

        /// <summary>
        /// Posts a function to be executed on this event loop and returns a Task for the result.
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="func">The function to execute</param>
        /// <returns>A Task that completes when the function has executed</returns>
        Task<T> PostAsync<T>(Func<T> func);

        /// <summary>
        /// Posts an async function to be executed on this event loop.
        /// </summary>
        /// <param name="func">The async function to execute</param>
        /// <returns>A Task that completes when the function has executed</returns>
        Task PostAsync(Func<Task> func);

        /// <summary>
        /// Runs the event loop (blocks until stopped).
        /// This is typically called on worker threads.
        /// </summary>
        void Run();

        /// <summary>
        /// Stops the event loop.
        /// </summary>
        void Stop();

        /// <summary>
        /// Checks if the current thread is the event loop's thread.
        /// </summary>
        bool IsCurrentThread { get; }

        /// <summary>
        /// Gets whether the event loop is running.
        /// </summary>
        bool IsRunning { get; }
    }
}
