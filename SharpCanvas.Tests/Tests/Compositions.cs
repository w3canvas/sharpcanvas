using System;
using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Compositions : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(SourceOver);
            tests.Add(SourceIn);
            tests.Add(SourceOut);
            tests.Add(SourceAtop);
            tests.Add(DestinationOver);
            tests.Add(DestinationIn);
            tests.Add(DestinationOut);
            tests.Add(DestinationAtop);
            tests.Add(Lighter);
            tests.Add(Darker);
            tests.Add(Copy);
            tests.Add(Xor);
            return tests;
        }

        public string Name
        {
            get { return "Compositions"; }
        }

        #endregion

        private string SourceOver(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "source-over");
        }

        private string SourceIn(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "source-in");
        }

        private string SourceOut(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "source-out");
        }

        private string SourceAtop(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "source-atop");
        }

        private string DestinationOver(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "destination-over");
        }

        private string DestinationIn(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "destination-in");
        }

        private string DestinationOut(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "destination-out");
        }

        private string DestinationAtop(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "destination-atop");
        }

        private string Lighter(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "lighter");
        }

        private string Darker(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "darker");
        }

        private string Copy(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "copy");
        }

        private string Xor(ICanvasRenderingContext2D ctx)
        {
            return Composite(ctx, "xor");
        }


        private string Composite(ICanvasRenderingContext2D ctx, string compositeType)
        {
            // draw rectangle
            ctx.fillStyle = "#09f";
            ctx.fillRect(15, 15, 70, 70);

            // set composite property
            ctx.globalCompositeOperation = compositeType;

            // draw circle
            ctx.fillStyle = "#f30";
            ctx.beginPath();
            ctx.arc(75, 75, 35, 0, Math.PI*2, true);
            ctx.fill();
            ctx.commit();
            return @"Originals\Compositions\" + compositeType + ".png";
        }
    }
}