using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using System;

namespace SharpCanvas.Tests.Skia.Minimal
{
    [TestFixture]
    public class SkiaContextTests
    {
        [Test]
        public void TestArc()
        {
            var context = new SkiaCanvasRenderingContext2D(100, 100);
            context.fillStyle = "blue";
            context.arc(50, 50, 40, 0, (float)Math.PI / 2, false);
            context.fill();

            var bitmap = context.GetBitmap();

            // Check if there are any blue pixels in the image
            var hasBlue = false;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y) == SKColors.Blue)
                    {
                        hasBlue = true;
                        break;
                    }
                }
                if (hasBlue)
                {
                    break;
                }
            }
            Assert.That(hasBlue, Is.True, "No blue pixels found in the image.");
        }
    }
}
