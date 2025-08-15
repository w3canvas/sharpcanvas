using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class SaveRestore : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample19);
            return tests;
        }

        public string Name
        {
            get { return "save and restore"; }
        }

        #endregion

        private string sample19(ICanvasRenderingContext2D ctx)
        {
            ctx.fillRect(0, 0, 150, 150); //  draw a rectangle with default settings
            ctx.save(); //  save the default state

            ctx.fillStyle = "#09F"; // Make changes to the settings
            ctx.fillRect(15, 15, 120, 120); // Draw a rectangle with new settings

            ctx.save(); // save the current state
            ctx.fillStyle = "#FFF"; // Make changes to the settings
            ctx.globalAlpha = 0.5;
            ctx.fillRect(30, 30, 90, 90); // Draw a rectangle with new settings

            ctx.restore(); // restore previous state
            ctx.fillRect(45, 45, 60, 60); // Draw a rectangle with restored settings

            ctx.restore(); // restore original state
            ctx.fillRect(60, 60, 30, 30); // Draw a rectangle with restored settings

            return @"Originals\SaveRestore\sample19.png";
        }
    }
}