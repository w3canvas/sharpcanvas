using System.Collections.Generic;
using System.Threading;
using Jint;
using Timer = System.Windows.Forms.Timer;

namespace SharpCanvas.Browser.Forms
{
    public class Timeout : ITimeout
    {
        private readonly Dictionary<int, Timer> _timers = new Dictionary<int, Timer>();
        private readonly object sync = new object();
        private int _index = 1;
        private readonly Engine _engine;

        public Timeout(Engine engine)
        {
            _engine = engine;
        }

        #region setTimeout

        /// <summary>
        /// Executes a code snippet or a function after specified delay.
        /// </summary>
        /// <param name="func">func is the function you want to execute after delay milliseconds</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the function call should be delayed by.</param>
        /// <returns>timeoutID is the ID of the timeout, which can be used with window.clearTimeout.</returns>
        public int setTimeout(string func, object milliseconds)
        {
            int m = 100;
            int.TryParse(milliseconds.ToString(), out m);
            var timer = new Timer();
            timer.Interval = m;
            int currentIndex = _index;
            timer.Tick += delegate
                                 {
                                     bool isLocked = !Monitor.TryEnter(sync);

                                     if (!isLocked)
                                     {
                                         if (func != null)
                                             _engine.Execute(func);
                                         clearTimeout(currentIndex);
                                         Monitor.Exit(sync);
                                     }
                                 };
            timer.Start();
            //preserve link to the timer inside internal collection
            _timers.Add(currentIndex, timer);
            return _index++;
        }

        /// <summary>
        /// Clears the delay set by window.setTimeout().
        /// </summary>
        /// <param name="key">where key is the ID of the timeout you wish to clear, as returned by window.setTimeout().</param>
        public void clearTimeout(int key)
        {
            if (_timers.ContainsKey(key)) //timer still exists
            {
                //stop the timer
                _timers[key].Stop();
                //release resources
                _timers[key].Dispose();
                //remove from internal collection
                _timers.Remove(key);
            }
        }

        #endregion

        #region setInterval

        /// <summary>
        /// Calls a function repeatedly, with a fixed time delay between each call to that function.
        /// </summary>
        /// <param name="func">func is the function you want to be called repeatedly.</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the setInterval() function should wait before each call to func.</param>
        /// <returns>unique interval ID you can pass to clearInterval().</returns>
        public int setInterval(string func, object milliseconds)
        {
            int m = 100;
            int.TryParse(milliseconds.ToString(), out m);
            var timer = new Timer();
            timer.Interval = m;
            timer.Tick += delegate
                                 {
                                     bool isLocked = !Monitor.TryEnter(sync);

                                     if (!isLocked)
                                     {
                                         if (func != null)
                                             _engine.Execute(func);
                                         Monitor.Exit(sync);
                                     }
                                 };
            timer.Start();
            //preserve link to the timer inside internal collection
            _timers.Add(_index, timer);
            return _index++;
        }

        /// <summary>
        /// Cancels repeated action which was set up using setInterval(). 
        /// </summary>
        /// <param name="key">is the identifier of the repeated action you want to cancel. This ID is returned from setInterval(). </param>
        public void clearInterval(int key)
        {
            clearTimeout(key);
        }

        #endregion
    }
}