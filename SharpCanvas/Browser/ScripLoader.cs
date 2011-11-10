using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace SharpCanvas.Browser.Browser
{
    internal class ScripLoader
    {
        private readonly object _sync = new object();
        private bool _scripLoaderIsRunning = false;

        public bool IsBusy
        {
            get { return _scripLoaderIsRunning; }
        }

        private void ScripLoaderWorker(string url)
        {
            Uri webUrl;
            if(Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out webUrl))
            {
                
            }
            else
            {
                FileInfo fi = new FileInfo(url);
                if(fi.Exists)
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(url);
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    System.Diagnostics.Process compile;
                    compile = System.Diagnostics.Process.Start(psi);
                    System.IO.StreamReader myOutput = compile.StandardOutput;
                    compile.WaitForExit(2000);
                    if (compile.HasExited)
                    {
                        string output = myOutput.ReadToEnd();
                        Debug.Write(output);
                    }
                }
                else
                {
                    throw new IOException(string.Format("File {0} doesn't exists.", url));
                }

            }
        }

        private delegate void ScripLoaderWorkerDelegate(string url);

        public void LoadScriptAsync(string url)
        {
            ScripLoaderWorkerDelegate worker = new ScripLoaderWorkerDelegate(ScripLoaderWorker);
            AsyncCallback completedCallback = new AsyncCallback(ScriptLoadCompletedCallback);

            lock (_sync)
            {
                if (_scripLoaderIsRunning)
                    throw new InvalidOperationException("The script loader is currently busy.");

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);
                worker.BeginInvoke(url, completedCallback, async);
                _scripLoaderIsRunning = true;
            }
        }

        private void ScriptLoadCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            ScripLoaderWorkerDelegate worker =
              (ScripLoaderWorkerDelegate)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            // finish the asynchronous operation
            worker.EndInvoke(ar);

            // clear the running task flag
            lock (_sync)
            {
                _scripLoaderIsRunning = false;
            }

            // raise the completed event
            AsyncCompletedEventArgs completedArgs = new AsyncCompletedEventArgs(null,
              false, null);
            async.PostOperationCompleted(
              delegate(object e) { OnScriptLoadCompleted((AsyncCompletedEventArgs)e); },
              completedArgs);
        }

        public event AsyncCompletedEventHandler ScriptLoadCompleted;

        protected virtual void OnScriptLoadCompleted(AsyncCompletedEventArgs e)
        {
            if (ScriptLoadCompleted != null)
                ScriptLoadCompleted(this, e);
        }

    }
}
