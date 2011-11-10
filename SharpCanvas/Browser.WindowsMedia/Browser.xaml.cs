using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpCanvas.Browser.Media
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : System.Windows.Window
    {
        private static readonly Browser _instance = new Browser();
        Window _window;
        Size _defaultSize = new Size(900, 800);
        
        public Browser()
        {
            //by default browser is a parent control for all other controls, windows, iframes, canvases, etc.
            this.Width = _defaultSize.Width;
            this.Height = _defaultSize.Height;
            InitializeComponent();
            Name = "Browser";
            
            _window = new Window();
            _window.innerHeight = (int)_defaultSize.Height;
            _window.innerWidth = (int)_defaultSize.Width;
            this.Content = _window;

        }

        public Window Window
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
    }
}
