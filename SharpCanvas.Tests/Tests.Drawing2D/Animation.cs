using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SharpCanvas.Tests;
using SharpCanvas.Forms;

namespace SharpCanvas.Tests.Forms
{
    public class Animation : ITestCategory, IAnimation
    {
        private readonly int interval;
        private readonly ToolStripStatusLabel label;
        private readonly Control surface;
        private FlyingDragon dragon;

        public Animation(Control panel, int interval, ToolStripStatusLabel label)
        {
            surface = panel;
            this.interval = interval;
            IsStarted = false;
            this.label = label;
        }

        public bool IsStarted { get; set; }

        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample28);
            return tests;
        }

        public string Name
        {
            get { return "Animation"; }
        }

        #endregion

        private string sample28(ICanvasRenderingContext2D ctx)
        {
            Thread.Sleep(interval);
            Graphics g = surface.CreateGraphics();
            g.Clear(Color.White);
            dragon = new FlyingDragon(g, (int) g.VisibleClipBounds.Width, (int) g.VisibleClipBounds.Height,
                                      new Pen(Color.Black, 1), new Fill(Color.Black), interval);
            dragon.draw(label);
            IsStarted = true;
            return string.Empty;
        }

        public void Stop()
        {
            dragon.stop();
            IsStarted = false;
            Thread.Sleep(interval);
        }
    }
}