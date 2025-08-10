using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SharpCanvas.Shared;
using SharpCanvas.StandardFilter.FilterSet;
using SharpCanvas.StandardFilter.FilterSet.ColorMatrix;

namespace SharpCanvas.Tests
{
    public class Filters : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(InvertFilter);
            return tests;
        }

        public string Name
        {
            get { return "Filters"; }
        }

        #endregion

        private string InvertFilter(ICanvasRenderingContext2D ctx)
        {
            var di = new DirectoryInfo(Application.StartupPath);
            string url = di.Parent.Parent.Parent.FullName + "\\SharpCanvas.Tests\\" + @"Originals\Filters\initial.jpg";
            ctx.drawImage(url, 0, 0);
            var data = (ImageData) ctx.getImageData(0, 0, 121, 121);
            /*
            var chain = (FilterChain) ctx.createFilterChain();
            var filter = new InvertFilter();
            chain.AddFilter(filter);
            data.applyFilters(chain);

            ctx.putImageData(data, 0, 0);
            */
            return @"Originals\Filters\InvertFilter.png";
        }
    }
}