using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Shadows : ITestCategory
    {
        #region Implementation of ITestCategory

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(FillRect);
            tests.Add(StrokeRect);
            tests.Add(Fill);
            tests.Add(CompositeOperationsXOR);
            return tests;
        }

        public string Name
        {
            get { return "Shadows"; }
        }

        private string FillRect(ICanvasRenderingContext2D ctx)
        {
            ctx.fillStyle = "#0f0";
            ctx.strokeStyle = "#0f0";
            ctx.shadowColor = "#00f";
            ctx.shadowOffsetX = 32;
            ctx.shadowOffsetY = 16;
            ctx.shadowBlur = 8;
            ctx.fillRect(32, 6, 128, 128);
            return @"Originals\Shadows\FillRect.png";
        }

        private string StrokeRect(ICanvasRenderingContext2D ctx)
        {
            ctx.fillStyle = "#0f0";
            ctx.strokeStyle = "#0f0";
            ctx.shadowColor = "#00f";
            ctx.shadowOffsetX = 32;
            ctx.shadowOffsetY = 16;
            ctx.shadowBlur = 8;
            ctx.strokeRect(32, 25, 128, 128);
            return @"Originals\Shadows\StrokeRect.png";
        }

        private string Fill(ICanvasRenderingContext2D ctx)
        {
            ctx.fillStyle = "#0f0";
            ctx.strokeStyle = "#0f0";
            ctx.shadowColor = "#00f";
            ctx.shadowOffsetX = 32;
            ctx.shadowOffsetY = 16;
            ctx.shadowBlur = 8;
            ctx.moveTo(32, 32);
            ctx.lineTo(128, 128);
            ctx.lineTo(128, 32);
            ctx.lineTo(32, 128);
            ctx.fill();

            return @"Originals\Shadows\Fill.png";
        }

        private string CompositeOperationsXOR(ICanvasRenderingContext2D ctx)
        {
            ctx.fillStyle = "#0f0";
            ctx.strokeStyle = "#0f0";
            ctx.shadowColor = "#00f";
            ctx.shadowOffsetX = 32;
            ctx.shadowOffsetY = 16;
            ctx.shadowBlur = 8;
            ctx.fillRect(32, 32, 128, 128);
            ctx.globalCompositeOperation = "xor";
            ctx.fillRect(96, 64, 128, 128);

            return @"Originals\Shadows\XOR.png";
        }

        #endregion
    }
}