using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SharpCanvas.Host.Browser
{
    internal class ScripLoader
    {
        private readonly object _sync = new object();
        private bool _scripLoaderIsRunning = false;

        public bool IsBusy
        {
            get { return _scripLoaderIsRunning; }
        }

        public void LoadScript(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");
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
                    System.Diagnostics.Process? compile;
                    compile = System.Diagnostics.Process.Start(psi);
                    if (compile == null) return;
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
    }
}
