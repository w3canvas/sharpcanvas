using System;
using System.Collections.Generic;
using System.Windows;

namespace SharpCanvas.Browser.Media
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread()]
        static void Main()
        {
            App app = new App();
            app.MainWindow = new Browser();
            app.MainWindow.Show();
            app.Run();
        }
    }
}
