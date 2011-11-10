using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SharpCanvas.Tests;
using SharpCanvas.Forms;

namespace SharpCanvas.Tests.Forms
{
    public partial class TestEngine : Form
    {
        private readonly Animation animation;
        private readonly List<ITestCategory> categories = new List<ITestCategory>();
        private int interval = 100;

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
            animation = new Animation(ctResults.Panel1, interval, lblFps);
            categories.Add(new BrowserContext(ctResults.Panel1));

            categories.Add(animation);

            foreach (ITestCategory testCategory in categories)
            {
                trvTests.Nodes.Add(testCategory.Name, testCategory.Name);
                TreeNode categoryNode = trvTests.Nodes[testCategory.Name];
                List<TestCase> tests = testCategory.GetTestCases();
                foreach (TestCase testCase in tests)
                {
                    categoryNode.Nodes.Add(testCase.Method.Name, testCase.Method.Name);
                }
            }
        }

        private void trvTests_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes != null && e.Node.Nodes.Count > 0)
            {
                return;
            }
            string categoryName = e.Node.Parent.Name;
            foreach (ITestCategory testCategory in categories)
            {
                if (testCategory.Name == categoryName)
                {
                    foreach (TestCase test in testCategory.GetTestCases())
                    {
                        if (test.Method.Name == e.Node.Name)
                        {
                            if (testCategory is IAnimation)
                                DoAnimationTest(test);
                            else
                                DoTest(test);
                            lblTestName.Text = "Name: " + test.Method.Name;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void DoAnimationTest(TestCase test)
        {
            if (animation.IsStarted)
                animation.Stop();
            pctOriginal.Hide();
            //lblFps.Text = "fps: " + 1000/interval;
            test(null);
        }

        private void DoTest(TestCase test)
        {
            if (animation.IsStarted)
                animation.Stop();

            Graphics g = ctResults.Panel1.CreateGraphics();
            //this.CreateGraphics();
            g.Clear(Color.White);
            var bmp = new Bitmap((int) g.VisibleClipBounds.Width, (int) g.VisibleClipBounds.Height);
            Graphics tmp = Graphics.FromImage(bmp);
            var ctx = new CanvasRenderingContext2D(tmp, bmp, new Pen(Color.Black, 1), new Fill(Color.Black), false);
            string url = test(ctx);
            g.DrawImage(bmp, 0, 0);
            var di = new DirectoryInfo(Application.StartupPath);
            url = di.Parent.Parent.Parent.FullName + "\\SharpCanvas.Tests\\" + url;
            if (File.Exists(url))
            {
                pctOriginal.Load(url);
                pctOriginal.Show();
            }
            else
            {
                pctOriginal.Hide();
            }
        }
    }
}