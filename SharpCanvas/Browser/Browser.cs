using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SharpCanvas.Host.Browser;
using SharpCanvas.Core.Shared;
using Microsoft.JScript;

namespace SharpCanvas.Host.Browser
{
    public class Browser : Form
    {
        private static readonly Browser _instance = new Browser();
        private static Dictionary<int, int> _registeredZIndexes = new Dictionary<int, int>();
        public static object sync = new object();
        private GlobalScope _globalScope;
        WindowProxy _window;
        Size _defaultSize = new Size(900, 950);

        public IWindow Window
        {
            get { return _window; }
        }

        /// <summary>
        /// There can be only single instance of the Browser object, i.e. singleton pattern.
        /// </summary>
        public static Browser Instance
        {
            get { return _instance; }
        }

        public GlobalScope GlobalScope
        {
            get { return _globalScope; }
            set
            {
                object v = value;
                if (v is GlobalScope)
                {
                    _globalScope = v as GlobalScope;
                }
            }
        }

        //private const int WS_EX_COMPOSITED = 0x02000000;
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= WS_EX_COMPOSITED;
        //        return cp;
        //    }
        //}

        private Browser()
        {
            //by default browser is a parent control for all other controls, windows, iframes, canvases, etc.
            Size = _defaultSize;
            Visible = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Name = "Browser";
            
            _window = new WindowProxy();
            _window.innerHeight = Size.Width;
            _window.innerWidth = Size.Height- 25;
            _window.Left = 0;
            _window.Top = 0;

            //BackColor = Color.Green;
            (_window.GetRealObject() as UserControl).BackColor = Color.White;
            Controls.Add(_window.GetRealObject() as UserControl);
        }

        public int GetRelativeZIndex(int absoluteZIndex)
        {
            lock (sync)
            {
                if (_registeredZIndexes.ContainsKey(absoluteZIndex))
                {
                    return _registeredZIndexes[absoluteZIndex];
                }
                else
                {
                    int[] keys = new int[_registeredZIndexes.Count];
                    _registeredZIndexes.Keys.CopyTo(keys, 0);
                    int relativeZIndex = 0;
                    bool relativeFound = false;
                    foreach (int zIndex in keys)
                    {
                        if (relativeFound)
                        {
                            _registeredZIndexes[zIndex]++;
                        }
                        if (zIndex > absoluteZIndex && !relativeFound)
                        {
                            relativeZIndex = _registeredZIndexes[zIndex] + 1;
                            relativeFound = true;
                        }
                    }
                    _registeredZIndexes.Add(absoluteZIndex, relativeZIndex);
                    return relativeZIndex;
                }
            }
        }
    }
}
