using System;
using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Transformations : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample20);
            tests.Add(sample21);
            tests.Add(sample22);
            tests.Add(sample23);
            tests.Add(sample24);
            tests.Add(sample27);
            return tests;
        }

        public string Name
        {
            get { return "Transformations"; }
        }

        #endregion

        private string sample27(ICanvasRenderingContext2D ctx)
        {
            ctx.strokeStyle = "black";
            ctx.beginPath();
            ctx.rect(0, 0, 100, 100);
            ctx.stroke();
            ctx.strokeStyle = "#fc0";
            ctx.moveTo(0, 0);
            ctx.translate(100.0, 0.0);
            ctx.lineTo(0, 0);
            ctx.translate(0, 100);
            ctx.lineTo(0, 0);
            ctx.translate(-100, 0);
            ctx.lineTo(0, 0);
            ctx.stroke();
            return @"Originals\Transformations\sample27.png";
        }

        private string sample24(ICanvasRenderingContext2D ctx)
        {
            ctx.strokeStyle = "#fc0";
            ctx.lineWidth = 1.5;
            ctx.fillRect(0, 0, 300, 300);

            // Uniform scaling
            ctx.save();
            ctx.translate(50, 50);
            drawSpirograph(ctx, 22, 6, 5); // no scaling

            ctx.translate(100, 0);
            ctx.scale(0.75, 0.75);
            drawSpirograph(ctx, 22, 6, 5);

            ctx.translate(133.333, 0);
            ctx.scale(0.75, 0.75);
            drawSpirograph(ctx, 22, 6, 5);
            ctx.restore();

            // Non-uniform scaling (y direction)
            ctx.strokeStyle = "#0cf";
            ctx.save();
            ctx.translate(50, 150);
            ctx.scale(1, 0.75);
            drawSpirograph(ctx, 22, 6, 5);

            ctx.translate(100, 0);
            ctx.scale(1, 0.75);
            drawSpirograph(ctx, 22, 6, 5);

            ctx.translate(100, 0);
            ctx.scale(1, 0.75);
            drawSpirograph(ctx, 22, 6, 5);
            ctx.restore();

            // Non-uniform scaling (x direction)
            ctx.strokeStyle = "#cf0";
            ctx.save();
            ctx.translate(50, 250);
            ctx.scale(0.75, 1);
            drawSpirograph(ctx, 22, 6, 5);

            ctx.translate(133.333, 0);
            ctx.scale(0.75, 1);
            drawSpirograph(ctx, 22, 6, 5);

            ctx.translate(177.777, 0);
            ctx.scale(0.75, 1);
            drawSpirograph(ctx, 22, 6, 5);
            ctx.restore();
            return @"Originals\Transformations\sample24.png";
        }


        private string sample23(ICanvasRenderingContext2D ctx)
        {
            double sin = Math.Sin(Math.PI/6);
            double cos = Math.Cos(Math.PI/6);
            ctx.translate(150, 150);
            double c = 0;
            for (int i = 0; i <= 12; i++)
            {
                c = Math.Floor(Convert.ToDouble(255/12*i));
                ctx.fillStyle = "rgb(" + c + "," + c + "," + c + ")";
                ctx.fillRect(0, 0, 100, 10);
                ctx.transform(cos, sin, -sin, cos, 0, 0);
            }

            ctx.setTransform(-1, 0, 0, 1, 150, 150);
            ctx.fillStyle = "rgba(255, 128, 255, 0.5)";
            ctx.fillRect(0, 50, 100, 100);
            return @"Originals\Transformations\sample23.png";
        }

        private string sample22(ICanvasRenderingContext2D ctx)
        {
            ctx.fillRect(0, 0, 300, 300);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ctx.save();
                    ctx.strokeStyle = "#9CFF00";
                    ctx.translate(50 + j*100, 50 + i*100);
                    double R = 20*(j + 2.0)/(j + 1.0);
                    double r = -8*(i + 3.0)/(i + 1.0);
                    drawSpirograph(ctx, R, r, 10);
                    ctx.restore();
                }
            }
            return @"Originals\Transformations\sample22.png";
        }

        private void drawSpirograph(ICanvasRenderingContext2D ctx, double R, double r, double O)
        {
            double x1 = R - O;
            double y1 = 0;
            int i = 1;
            double x2, y2;
            ctx.beginPath();
            ctx.moveTo(x1, y1);
            do
            {
                if (i > 20000) break;
                double angle = i*Math.PI/72;
                x2 = (R + r)*Math.Cos(angle) - (r + O)*Math.Cos(((R + r)/r)*angle);
                y2 = (R + r)*Math.Sin(angle) - (r + O)*Math.Sin(((R + r)/r)*angle);
                ctx.lineTo(x2, y2);
                x1 = x2;
                y1 = y2;
                i++;
            } while (x2 != R - O && y2 != 0);
            ctx.stroke();
        }

        private string sample21(ICanvasRenderingContext2D ctx)
        {
            ctx.translate(75, 75);

            for (int i = 1; i < 6; i++)
            {
                // Loop through rings (from inside to out)
                ctx.save();
                ctx.fillStyle = "rgb(" + (51*i) + "," + (255 - 51*i) + ",255)";

                for (int j = 0; j < i*6; j++)
                {
                    // draw individual dots
                    ctx.rotate((float) Math.PI*2/(i*6));
                    ctx.beginPath();
                    ctx.arc(0, i*12.5F, 5, 0, Math.PI*2, true);
                    ctx.fill();
                    ctx.commit();
                }

                ctx.restore();
            }
            return @"Originals\Transformations\sample21.png";
        }

        private string sample20(ICanvasRenderingContext2D ctx)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ctx.save();
                    ctx.strokeStyle = "#9CFF00";
                    ctx.translate(50 + j*100, 50 + i*100);
                    ctx.fillRect(0, 0, 20, 20);
                    ctx.restore();
                }
            }
            return @"Originals\Transformations\sample20.png";
        }
    }
}