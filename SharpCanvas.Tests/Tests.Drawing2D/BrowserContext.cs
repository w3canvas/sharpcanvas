using System;
using System.Collections.Generic;
using System.Threading;
using SharpCanvas.Host;
using SharpCanvas.Browser.Forms;
using SharpCanvas.Browser;
using System.Windows.Forms;
using SharpCanvas;

namespace SharpCanvas.Tests
{
    public class BrowserContext : ITestCategory
    {
        private readonly Control container;
        public ICanvasRenderingContext2D _ctx;
        public SharpCanvas.Interop.IHTMLCanvasElement canvas;
        public BrowserContext(Control container)
        {
            // SharpCanvas.Host.Standalone.CreateCanvasElement()
            canvas = new SharpCanvas.Host.Browser.CanvasProxy();
            // canvas = new SharpCanvas.Host.Browser.HTMLCanvasElement();
            // FIXME: These are optional defaults.
            canvas.width = 300; canvas.height = 150; 
            _ctx = (ICanvasRenderingContext2D) canvas.getContext("2d");
            if (_ctx == null) throw new System.Exception("Could not fully instantiate Canvas object.");
            this.container = container;
            
        }

        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample29);
            return tests;
        }

        public string Name
        {
            get { return "BrowserContext"; }
        }

        #endregion
        
        private string sample29(ICanvasRenderingContext2D ctx)
        {
            container.Controls.Add((SharpCanvas.Host.Browser.HTMLElement) canvas); 
            _ctx.fillRect(0, 0, 100, 100);
            return string.Empty;
        }
    }
}