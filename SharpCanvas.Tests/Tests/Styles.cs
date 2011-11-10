using System;
using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Styles : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample10);
            tests.Add(sample9);
            tests.Add(sample8);
            tests.Add(sample1);
            return tests;
        }

        public string Name
        {
            get { return "Styles"; }
        }

        #endregion

        private string sample10(ICanvasRenderingContext2D ctx)
        {
            // Draw background
            ctx.fillStyle = "rgb(255,221,0)";
            ctx.fillRect(0, 0, 150, 37.5F);
            ctx.fillStyle = "rgb(102,204,0)";
            ctx.fillRect(0, 37.5F, 150, 37.5F);
            ctx.fillStyle = "rgb(0,153,255)";
            ctx.fillRect(0, 75, 150, 37.5F);
            ctx.fillStyle = "rgb(255,51,0)";
            ctx.fillRect(0, 112.5F, 150, 37.5F);

            // Draw semi transparent rectangles
            for (int i = 0; i < 10; i++)
            {
                ctx.fillStyle = "rgba(255,255,255," + (float) (i + 1)/10 + ')';
                for (int j = 0; j < 4; j++)
                {
                    ctx.fillRect(5 + i*14, 5 + j*37.5F, 14, 27.5F);
                }
            }
            return @"Originals\Styles\sample10.png";
        }

        private string sample9(ICanvasRenderingContext2D ctx)
        {
            // draw background
            ctx.fillStyle = "#FD0";
            ctx.fillRect(0, 0, 75, 75);
            ctx.fillStyle = "#6C0";
            ctx.fillRect(75, 0, 75, 75);
            ctx.fillStyle = "#09F";
            ctx.fillRect(0, 75, 75, 75);
            ctx.fillStyle = "#F30";
            ctx.fillRect(75, 75, 75, 75);
            ctx.fillStyle = "#FFF";

            // set transparency value
            ctx.globalAlpha = 0.2F;

            // Draw semi transparent circles
            for (int i = 0; i < 7; i++)
            {
                ctx.beginPath();
                ctx.arc(75, 75, 10 + 10*i, 0, Math.PI*2, true);
                ctx.fill();
            }
            return @"Originals\Styles\sample9.png";
        }

        private string sample8(ICanvasRenderingContext2D ctx)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    ctx.strokeStyle = "rgb(0," + Math.Floor(255 - 42.5*i) + ',' +
                                      Math.Floor(255 - 42.5*j) + ')';
                    ctx.beginPath();
                    ctx.arc(12.5F + j*25, 12.5F + i*25, 10, 0, Math.PI*2, true);
                    ctx.stroke();
                }
            }
            return @"Originals\Styles\sample8.png";
        }

        private string sample1(ICanvasRenderingContext2D ctx)
        {
            ctx.fillStyle = "rgb(200,0,0)";
            ctx.fillRect(10, 10, 55, 50);

            ctx.fillStyle = "rgba(0,0,200,0.5)";
            ctx.fillRect(30, 30, 55, 50);
            return @"Originals\Styles\sample1.png";
        }
    }
}