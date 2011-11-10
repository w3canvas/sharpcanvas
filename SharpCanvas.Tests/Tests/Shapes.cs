using System;
using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Shapes : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample4);
            tests.Add(sample3);
            tests.Add(Clip);
            tests.Add(ArcTo80);
            tests.Add(ArcTo50);
            tests.Add(ArcTo40);
            tests.Add(ArcTo30);
            tests.Add(ArcTo10);
            tests.Add(ArcTo5);
            tests.Add(ArcTo01);
            tests.Add(ArcTo0);
            return tests;
        }

        public string Name
        {
            get { return "Shapes"; }
        }

        #endregion

        private string ArcTo80(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            int y2 = 80;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo80.png";
        }

        private string ArcTo50(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            int y2 = 50;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo50.png";
        }

        private string ArcTo40(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            int y2 = 40;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo40.png";
        }

        private string ArcTo30(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            int y2 = 30;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo30.png";
        }

        private string ArcTo10(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            int y2 = 10;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo10.png";
        }

        private string ArcTo5(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            int y2 = 5;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo5.png";
        }

        private string ArcTo01(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            double y2 = 0.1;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo01.png";
        }

        private string ArcTo0(ICanvasRenderingContext2D ctx)
        {
            int x0 = 200, x1 = 250, x2 = 150, x3 = 200, y = 10, y3 = 80;
            int y2 = 0;
            int r = 10;
            disc(ctx, x0, y, 4, "red");
            disc(ctx, x1, y, 4, "red");
            disc(ctx, x2, y + y2, 4, "red");
            disc(ctx, x3, y3, 4, "green");
            line(ctx, x0, y, x1, y, 1, "blue");
            line(ctx, x1, y, x2, y + y2, 1, "blue");

            ctx.lineWidth = 2;
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.moveTo(x0, y);
            ctx.arcTo(x1, y, x2, y + y2, r);
            ctx.lineTo(x3, y3);
            ctx.stroke();
            return @"Originals\Shapes\ArcTo0.png";
        }

        private void line(ICanvasRenderingContext2D ctx, double x0, double y0, double x1, double y1, double w,
                          string col)
        {
            ctx.beginPath();
            ctx.moveTo(x0, y0);
            ctx.lineTo(x1, y1);
            ctx.lineWidth = w;
            ctx.strokeStyle = col;
            ctx.stroke();
        }

        private void disc(ICanvasRenderingContext2D ctx, double x, double y, double r, string col)
        {
            ctx.beginPath();
            ctx.moveTo(x, y);
            ctx.arc(x, y, r, 0, 2*Math.PI, false);
            ctx.fillStyle = col;
            ctx.fill();
        }

        private string sample4(ICanvasRenderingContext2D ctx)
        {
            ctx.beginPath();
            ctx.moveTo(75, 40);
            ctx.bezierCurveTo(75, 37, 70, 25, 50, 25);
            ctx.bezierCurveTo(20, 25, 20, 62.5F, 20, 62.5F);
            ctx.bezierCurveTo(20, 80, 40, 102, 75, 120);
            ctx.bezierCurveTo(110, 102, 130, 80, 130, 62.5F);
            ctx.bezierCurveTo(130, 62.5F, 130, 25, 100, 25);
            ctx.bezierCurveTo(85, 25, 75, 37, 75, 40);
            ctx.fill();
            return @"Originals\Shapes\sample4.png";
        }

        private string Clip(ICanvasRenderingContext2D ctx)
        {
            ctx.fillStyle = "black";
            ctx.fillRect(0, 0, 150, 150);
            ctx.translate(75, 75);

            // Create a circular clipping path        
            ctx.beginPath();
            ctx.arc(0, 0, 60, 0, Math.PI*2, true);
            ctx.clip();

            // draw background
            var lingrad = (ILinearCanvasGradient) ctx.createLinearGradient(0, -75, 0, 75);
            lingrad.addColorStop(0, "#232256");
            lingrad.addColorStop(1, "#143778");

            ctx.fillStyle = lingrad;
            ctx.fillRect(-75, -75, 150, 150);

            // draw stars
            var r = new Random();
            for (int j = 1; j < 50; j++)
            {
                ctx.save();
                ctx.fillStyle = "#fff";
                ctx.translate(75 - (int) (r.NextDouble()*150),
                              75 - (int) (r.NextDouble()*150));
                drawStar(ctx, (int) (r.NextDouble()*4 + 2));
                ctx.restore();
            }
            return @"Originals\Shapes\Clip.png";
        }

        private void drawStar(ICanvasRenderingContext2D ctx, int r)
        {
            ctx.save();
            ctx.beginPath();
            ctx.moveTo(r, 0);
            for (int i = 0; i < 9; i++)
            {
                ctx.rotate(Math.PI/5);
                if (i%2 == 0)
                {
                    ctx.lineTo((r/0.525731)*0.200811, 0);
                }
                else
                {
                    ctx.lineTo(r, 0);
                }
            }
            ctx.closePath();
            ctx.fill();
            ctx.restore();
        }

        private string sample3(ICanvasRenderingContext2D ctx)
        {
            ctx.beginPath();
            ctx.arc(75, 75, 50, 0, Math.PI*2, true); // Outer circle
            ctx.moveTo(110, 75);
            ctx.arc(75, 75, 35, 0, Math.PI, false); // Mouth (clockwise)
            ctx.moveTo(65, 65);
            ctx.arc(60, 65, 5, 0, Math.PI*2, true); // Left eye
            ctx.moveTo(95, 65);
            ctx.arc(90, 65, 5, 0, Math.PI*2, true); // Right eye
            ctx.stroke();
            return @"Originals\Shapes\sample3.png";
        }
    }
}