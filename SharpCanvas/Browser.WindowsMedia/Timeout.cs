using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.JScript;

namespace SharpCanvas.Browser.Media
{
    public class Timeout : ITimeout
    {
        private readonly Dictionary<int, DispatcherTimer> _timers = new Dictionary<int, DispatcherTimer>();
        private int _index = 1;

        #region setTimeout

        /// <summary>
        /// Executes a code snippet or a function after specified delay.
        /// </summary>
        /// <param name="func">func is the function you want to execute after delay milliseconds</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the function call should be delayed by.</param>
        /// <returns>timeoutID is the ID of the timeout, which can be used with window.clearTimeout.</returns>
        public int setTimeout(object func, object milliseconds)
        {
            int m = 100;
            int.TryParse(milliseconds.ToString(), out m);
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(m);
            int currentIndex = _index;
            timer.Tick += delegate
                              {
                                  if (func != null)
                                      ((ScriptFunction) func).Invoke(this, new object[] {});
                                  clearTimeout(currentIndex);
                                  //timer.Stop();
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
        public int setInterval(object func, object milliseconds)
        {
            int m = 100;
            int.TryParse(milliseconds.ToString(), out m);
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(m);
            timer.Tick += delegate
                              {
                                  if (func != null)
                                      ((ScriptFunction) func).Invoke(this, new object[] {});
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