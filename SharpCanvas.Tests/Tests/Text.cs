using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Text : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample18);
            tests.Add(TextSizes);
            tests.Add(TextStrokeFill);
            tests.Add(TextPositions);
            return tests;
        }

        public string Name
        {
            get { return "Text"; }
        }

        #endregion

        private string sample18(ICanvasRenderingContext2D ctx)
        {
            ctx.shadowOffsetX = 2;
            ctx.shadowOffsetY = 2;
            ctx.shadowBlur = 2;
            ctx.shadowColor = "rgba(0, 0, 0, 0.5)";

            ctx.font = "20px Times New Roman";
            //ctx.textAlign = "end";
            //ctx.textBaseLine = "bottom";
            ctx.fillStyle = "Black";
            ctx.fillText("Sample String", 35, 30);
            return @"Originals\Text\sample18.png";
        }

        private string TextPositions(ICanvasRenderingContext2D ctx)
        {
            int i, j;
            string text = "Hello world";
            var align = new[] {"left", "center", "right"};
            var baseline = new[] {"top", "hanging", "middle", "alphabetic", "ideographic", "bottom"};

            ctx.fillStyle = "#000";
            ctx.strokeStyle = "#f00";
            ctx.font = "20px Arial";
            ctx.translate(70, 30);

            for (i = 0; i < align.Length; i++)
            {
                for (j = 0; j < baseline.Length; j++)
                {
                    ctx.save();
                    ctx.translate(i*170 + 0.5, j*50 + 0.5);

                    ctx.textAlign = align[i];
                    ctx.textBaseLine = baseline[j];

                    ctx.fillText(text, 0, 0);

                    ctx.beginPath();
                    ctx.moveTo(-50, 0);
                    ctx.lineTo(50, 0);

                    ctx.moveTo(0, -10);
                    ctx.lineTo(0, 10);
                    ctx.closePath();

                    ctx.stroke();
                    ctx.restore();
                }
            }
            return @"Originals\Text\TextPositions.png";
        }

        private string TextSizes(ICanvasRenderingContext2D ctx)
        {
            int w = 500;
            int h = 800;
            string fillText = "AWawあ漢字!?@";
            ctx.textBaseLine = "top";

            grid(ctx, w, h, 10, 5, "SkyBlue", "steelblue");

            int i = 0;
            string txt;
            int v;
            var ary = new[] {6, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20, 24, 28, 32, 36};
            ctx.strokeStyle = "black";
            int iz = ary.Length;
            for (; i < iz; ++i)
            {
                v = ary[i];
                txt = v + "pt;" + fillText;
                ctx.font = v + "pt Arial";
                ctx.strokeText(txt, 10, i*35 + 10);
            }

            return @"Originals\Text\TextOnGrid.png";
        }

        private void grid(ICanvasRenderingContext2D ctx, int w, int h, int size, int unit, string color, string color2)
        {
            int x, y, i, j;
            for (i = 0, x = size; x < w; ++i, x += size)
            {
                ctx.beginPath();
                ctx.strokeStyle = (i%unit != 0) ? color : color2;
                ctx.moveTo(x, 0);
                ctx.lineTo(x, h);
                ctx.stroke();
                ctx.closePath();
            }
            for (j = 0, y = size; y < h; ++j, y += size)
            {
                ctx.beginPath();
                ctx.strokeStyle = (j%unit != 0) ? color : color2;
                ctx.moveTo(0, y);
                ctx.lineTo(w, y);
                ctx.stroke();
                ctx.closePath();
            }
        }

        private string TextStrokeFill(ICanvasRenderingContext2D ctx)
        {
            string fillText = "fill. う～ぱ～";
            string strokeText = "stroke. う～ぱ～";

            ctx.textBaseLine = "top";
            ctx.font = "32pt Arial";

            ctx.fillStyle = "orange"; // shadow color
            ctx.fillText(fillText, 22, 22);
            ctx.fillStyle = "red";
            ctx.fillText(fillText, 20, 20);

            ctx.strokeStyle = "blue";
            ctx.strokeText(strokeText, 20, 80);
            return @"Originals\Text\TextStrokeFill.png";
        }
    }
}