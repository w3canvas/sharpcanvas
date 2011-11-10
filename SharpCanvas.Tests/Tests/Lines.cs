using System;
using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Lines : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample14);
            tests.Add(sample13);
            tests.Add(sample12);
            tests.Add(sample11);

            return tests;
        }

        public string Name
        {
            get { return "Lines"; }
        }

        #endregion

        private string sample14(ICanvasRenderingContext2D ctx)
        {
            // Clear canvas
            ctx.clearRect(-5, 0, 250, 250);

            // Draw guides
            ctx.strokeStyle = "#09f";
            ctx.lineWidth = 2;
            ctx.strokeRect(-5, 50, 160, 50);
            // Set line styles
            ctx.strokeStyle = "#000";
            ctx.lineWidth = 10;

            // check input
            ctx.miterLimit = 10;
            ctx.lineJoin = "miter";


            // Draw lines
            ctx.beginPath();
            //ctx.moveTo(0, 0);
            ctx.moveTo(0, 100);
            for (int i = 0; i < 24; i++)
            {
                int dy = i%2 == 0 ? 25 : -25;
                ctx.lineTo(Math.Pow(i, 1.5)*2, 75 + dy);
            }
            ctx.stroke();

            return @"Originals\Lines\sample14.png";
        }

        private string sample13(ICanvasRenderingContext2D ctx)
        {
            var lineJoin = new[] {"round", "bevel", "miter"};
            ctx.lineWidth = 10;
            for (int i = 0; i < lineJoin.Length; i++)
            {
                ctx.lineJoin = lineJoin[i];
                ctx.beginPath();
                ctx.moveTo(-5, 5 + i*40);
                ctx.lineTo(35, 45 + i*40);
                ctx.lineTo(75, 5 + i*40);
                ctx.lineTo(115, 45 + i*40);
                ctx.lineTo(155, 5 + i*40);
                ctx.stroke();
            }
            return @"Originals\Lines\sample13.png";
        }

        private string sample12(ICanvasRenderingContext2D ctx)
        {
            var lineCap = new[] {"butt", "round", "square"};

            // Draw guides
            ctx.strokeStyle = "#09f";
            ctx.beginPath();
            ctx.moveTo(10, 10);
            ctx.lineTo(140, 10);
            ctx.moveTo(10, 140);
            ctx.lineTo(140, 140);
            ctx.stroke();

            // Draw lines
            ctx.strokeStyle = "black";

            for (int i = 0; i < lineCap.Length; i++)
            {
                ctx.lineWidth = 15;
                ctx.lineCap = lineCap[i];
                ctx.beginPath();
                ctx.moveTo(25 + i*50, 10);
                ctx.lineTo(25 + i*50, 140);
                ctx.stroke();
            }
            return @"Originals\Lines\sample12.png";
        }


        private string sample11(ICanvasRenderingContext2D ctx)
        {
            for (int i = 0; i < 10; i++)
            {
                ctx.beginPath();
                ctx.lineWidth = 1 + i;
                ctx.moveTo(5 + i*14, 5);
                ctx.lineTo(5 + i*14, 140);
                ctx.stroke();
            }
            return @"Originals\Lines\sample11.png";
        }
    }
}