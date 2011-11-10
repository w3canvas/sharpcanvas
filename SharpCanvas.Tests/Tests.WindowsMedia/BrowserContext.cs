using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using SharpCanvas.Tests;
using SharpCanvas.Tests.Media;
using SharpCanvas.Host;
using SharpCanvas.Browser.Media;

namespace SharpCanvas.Media.Tests
{
    public class BrowserContext : ITestCategory
    {
        private readonly SharpCanvas.Tests.Media.HTMLElement container;
        public ICanvasRenderingContext2D _ctx;
        public SharpCanvas.Interop.IHTMLCanvasElement canvas;
        public BrowserContext(SharpCanvas.Tests.Media.HTMLElement container)
        {

            // canvas = SharpCanvas.Host.StandaloneBootstrapper.Factory.CreateCanvasElement();
            canvas = new SharpCanvas.Browser.Media.HTMLCanvasElement();
            _ctx = canvas.getCanvas();
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
            _ctx.fillRect(0, 0, 100, 100);
            return string.Empty;
        }
    }
}