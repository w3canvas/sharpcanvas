using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SharpCanvas.Tests;
using SharpCanvas.Media;
using SharpCanvas.Media.Tests;

namespace SharpCanvas.Tests.Media
{
    /// <summary>
    /// Interaction logic for TestEngine.xaml
    /// </summary>
    public partial class TestEngine : Window
    {
        private readonly List<ITestCategory> categories = new List<ITestCategory>();

        public TestEngine()
        {
            InitializeComponent();
            categories.Add(new Gradients());
            categories.Add(new Transformations());
            categories.Add(new Styles());
            categories.Add(new SaveRestore());
            categories.Add(new Shapes());
            categories.Add(new Lines());
            categories.Add(new Curves());
            categories.Add(new Text());
            categories.Add(new Images());
            categories.Add(new Compositions());
            categories.Add(new Shadows());
            categories.Add(new Filters());
            categories.Add(new Animation(ctResults, 20));
            // categories.Add(new BrowserContext(ctResults));
            // new BrowserContext(ctResults);

            foreach (ITestCategory testCategory in categories)
            {
                var categoryNode = new TreeViewItem();
                categoryNode.Header = testCategory.Name;
                trvTests.Items.Add(categoryNode);
                List<TestCase> tests = testCategory.GetTestCases();
                foreach (TestCase testCase in tests)
                {
                    var item = new TreeViewItem();

                    item.Header = testCase.Method.Name;
                    categoryNode.Items.Add(item);
                }
            }
        }

        private void trvTests_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (TreeViewItem) trvTests.SelectedItem;
            if (item == null || item.HasItems)
                return;
            string categoryName = ((TreeViewItem) item.Parent).Header.ToString();
            foreach (ITestCategory testCategory in categories)
            {
                if (testCategory.Name == categoryName)
                {
                    foreach (TestCase test in testCategory.GetTestCases())
                    {
                        if (test.Method.Name == item.Header.ToString())
                        {
                            DoTest(test);
                            //lblTestName.Text = "Name: " + test.Method.Name;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void DoTest(TestCase test)
        {
            var canvas = new Canvas();
            canvas.Width = 300;
            canvas.Height = 700;
            ICanvasRenderingContext2D ctx = new CanvasRenderingContext2D(canvas, this, true);
            string url = test(ctx);
            ctx.commit();
            string location = Application.ResourceAssembly.Location;
            string path = location.Remove(location.LastIndexOf("\\"));

            var di = new DirectoryInfo(path);
            url = di.Parent.Parent.Parent.FullName + "\\SharpCanvas.Tests\\" + url;
            if (File.Exists(url))
            {
                pctOriginal.Source = new BitmapImage(new Uri(url));
                pctOriginal.Visibility = Visibility.Visible;
            }
            else
            {
                pctOriginal.Visibility = Visibility.Hidden;
            }
            ctResults.Children.Clear();
            ctResults.Children.Add(canvas);
        }
    }
}